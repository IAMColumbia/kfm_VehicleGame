using UnityEngine;

public class CreatureWander : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float turnSpeed = 2f;
    public float waypointReachedDistance = 2f;

    [Header("Wander Bounds")]
    public float wanderRangeX = 20f;
    public float wanderRangeY = 8f;
    public float wanderRangeZ = 20f;
    public float centerY = 15f;

    [Header("Timing")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 4f;

    private Vector3 targetPosition;
    private bool waiting = false;
    private Animation anim;

    void Start()
    {
        // Legacy Animation component lives on the child mesh
        anim = GetComponentInChildren<Animation>();

        if (anim != null)
        {
            anim.wrapMode = WrapMode.Loop;
            anim.Play(); // plays the default clip automatically
        }

        PickNewTarget();
    }

    void FixedUpdate()
    {
        if (waiting) return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }

        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;

        if (Vector3.Distance(transform.position, targetPosition) <= waypointReachedDistance)
        {
            StartCoroutine(WaitThenWander());
        }
    }

    void PickNewTarget()
    {
        Vector3 origin = new Vector3(transform.position.x, centerY, transform.position.z);

        targetPosition = origin + new Vector3(
            Random.Range(-wanderRangeX, wanderRangeX),
            Random.Range(-wanderRangeY, wanderRangeY),
            Random.Range(-wanderRangeZ, wanderRangeZ)
        );
    }

    System.Collections.IEnumerator WaitThenWander()
    {
        waiting = true;

        // Slow animation while drifting
        if (anim != null)
            foreach (AnimationState state in anim)
                state.speed = 0.3f;

        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        // Resume normal animation speed
        if (anim != null)
            foreach (AnimationState state in anim)
                state.speed = 1f;

        waiting = false;
        PickNewTarget();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            waiting = false;
            PickNewTarget();
        }
    }
}