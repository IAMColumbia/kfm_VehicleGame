using UnityEngine;
using UnityEngine.InputSystem;

public class BathysphereCamera : MonoBehaviour
{
    public Transform target;

    [Header("Orbit Settings")]
    public float distance = 8f;
    public float sensitivity = 3f;
    public float smoothSpeed = 25f;

    [Header("Vertical Clamp")]
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float yaw = 0f;
    private float pitch = 20f; // start slightly above

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // At the top of LateUpdate, before the yaw/pitch lines:
        var mouse = Mouse.current;
        if (mouse == null) return;
        Vector2 mouseDelta = mouse.delta.ReadValue() * 0.1f; // scale it down, delta is in pixels

        yaw += mouseDelta.x * sensitivity;
        pitch -= mouseDelta.y * sensitivity;


        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Calculate desired position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = target.position + rotation * new Vector3(0, 0, -distance);

        // Smooth and apply
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position);
    }
}
