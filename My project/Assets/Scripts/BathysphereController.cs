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
    private Vector2 lookInput;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = waterDrag;

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

        lookInput = mouse.delta.ReadValue();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 move = (transform.forward * moveInput.y + transform.right * moveInput.x) * moveSpeed;
        move += Vector3.up * verticalInput * verticalSpeed;
        rb.AddForce(move, ForceMode.Acceleration);
    }

    void HandleRotation()
    {
        transform.Rotate(Vector3.up, lookInput.x * rotationSpeed * Time.fixedDeltaTime);
    }
}