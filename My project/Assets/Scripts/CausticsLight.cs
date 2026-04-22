using UnityEngine;

public class CausticsLight : MonoBehaviour
{
    public float rotateSpeed = 2f;
    public float scaleSpeed = 0.5f;
    public float minCookieSize = 18f;
    public float maxCookieSize = 22f;

    private Light causticsLight;
    private float baseSize;

    void Start()
    {
        causticsLight = GetComponent<Light>();
        baseSize = causticsLight.cookieSize;
    }

    void Update()
    {
        // Slowly rotate to animate the pattern
        transform.Rotate(Vector3.down, rotateSpeed * Time.deltaTime);

        // Gently pulse the cookie size for a ripple effect
        causticsLight.cookieSize = Mathf.Lerp(minCookieSize, maxCookieSize,
            (Mathf.Sin(Time.time * scaleSpeed) + 1f) * 0.5f);
    }
}