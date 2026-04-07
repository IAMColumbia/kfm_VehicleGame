using UnityEngine;

public class BathysphereCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 8f;
    public float height = 3f;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position - target.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position);
    }
}
