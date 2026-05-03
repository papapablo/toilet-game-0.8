using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 3f;
    public float waitTime = 2f;

    public float viewDistance = 10f;
    public float viewAngle = 60f;

    private int currentPoint = 0;
    private float waitTimer = 0f;

    public Transform player;
    private PlayerController playerScript;

    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        Patrol();
        CheckVision();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPoint];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        Vector3 dir = (target.position - transform.position).normalized;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                currentPoint++;
                if (currentPoint >= patrolPoints.Length)
                    currentPoint = 0;

                waitTimer = 0f;
            }
        }
    }

    void CheckVision()
    {
        if (playerScript.isDead) return;
        if (playerScript.isCrouching) return;

        Vector3 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance <= viewDistance)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);

            if (angle < viewAngle / 2f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, out hit, viewDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        playerScript.Die();
                    }
                }
            }
        }
    }
}
