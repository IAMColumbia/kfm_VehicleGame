using UnityEngine;
using UnityEngine.InputSystem;
public class BathysphereController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float verticalSpeed = 5f;
    public float rotationSpeed = 80f;

    [Header("Physics Feel")]
    public float waterDrag = 2f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private float verticalInput;
    private Unity.Cinemachine.CinemachineInputAxisController cinemachineInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = waterDrag;

        // Find the Cinemachine input controller
        cinemachineInput = FindFirstObjectByType<Unity.Cinemachine.CinemachineInputAxisController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Read inputs in Update
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        if (keyboard == null || mouse == null) return;

        moveInput = new Vector2(
            (keyboard.dKey.isPressed ? 1 : 0) - (keyboard.aKey.isPressed ? 1 : 0),
            (keyboard.wKey.isPressed ? 1 : 0) - (keyboard.sKey.isPressed ? 1 : 0)
        );

        verticalInput = (keyboard.eKey.isPressed ? 1 : 0) - (keyboard.qKey.isPressed ? 1 : 0);
        // Hold Left Alt to free the cursor for scanning
        if (Keyboard.current.leftAltKey.isPressed)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (cinemachineInput != null) cinemachineInput.enabled = false;
        }
        else if (!ScanManager.journalOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (cinemachineInput != null) cinemachineInput.enabled = true;
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        FaceMovementDirection();
    }

    void HandleMovement()
    {
        // Get the camera's orientation
        Transform cam = Camera.main.transform;

        // Flatten forward to horizontal (ignore camera pitch)
        Vector3 camForward = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;
        Vector3 camRight = new Vector3(cam.right.x, 0f, cam.right.z).normalized;

        // Build move direction relative to camera
        Vector3 move = (camForward * moveInput.y + camRight * moveInput.x) * moveSpeed;

        // Vertical is still world-space up/down
        move += Vector3.up * verticalInput * verticalSpeed;

        rb.AddForce(move, ForceMode.Acceleration);
    }
    void FaceMovementDirection()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (horizontalVelocity.magnitude > 0.1f) // only rotate if actually moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
    }
}