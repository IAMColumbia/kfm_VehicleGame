using UnityEngine;

public class BubbleEmitter : MonoBehaviour
{
    public float waterSurfaceY = 7.599f;

    private ParticleSystem bubbles;
    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        bubbles = GetComponent<ParticleSystem>();
        emission = bubbles.emission;
    }

    void Update()
    {
        bool isSubmerged = transform.position.y < waterSurfaceY;
        emission.rateOverTime = isSubmerged ? 15f : 0f;
    }
}