using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool repeat = true;

    [Header("Detection Settings")]
    [SerializeField] private float viewDistance = 5f;
    [SerializeField, Range(0, 360)] private float viewAngle = 90f;
    [SerializeField] private float chaseSpeed = 1f;
    [SerializeField] private LayerMask playerLayer;

    private Vector2 targetPosition;
    private Transform playerTransform;
    private bool movingToEnd = true;
    private bool isPatrolling = true;
    private bool isChasing = false;

    [ContextMenu("Set Start Position Here")]
    private void SetStart() => startPosition = transform.position;

    [ContextMenu("Set End Position Here")]
    private void SetEnd() => endPosition = transform.position;

    void Start()
    {
        transform.position = startPosition;
        targetPosition = endPosition;
    }

    void Update()
    {
        DetectPlayer();

        if (isChasing && playerTransform != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, viewDistance, playerLayer);
        
        if (hit != null && hit.CompareTag("Player"))
        {
            Vector2 directionToPlayer = (hit.transform.position - transform.position).normalized;
            float angleBetween = Vector2.Angle(transform.right, directionToPlayer);

            if (angleBetween < viewAngle / 2f)
            {
                RaycastHit2D ray = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance);
                if (ray.collider != null && ray.collider.CompareTag("Player"))
                {
                    playerTransform = hit.transform;
                    isChasing = true;
                    return;
                }
            }
        }
        
        isChasing = false;
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime);
        RotateTowards(playerTransform.position);
    }

    private void Patrol()
    {
        if (!isPatrolling) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        RotateTowards(targetPosition);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (movingToEnd)
            {
                targetPosition = startPosition;
                movingToEnd = false;
            }
            else
            {
                if (repeat)
                {
                    targetPosition = endPosition;
                    movingToEnd = true;
                }
                else
                {
                    isPatrolling = false;
                }
            }
        }
    }

    private void RotateTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnDrawGizmos()
    {
        // patrol markers
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPosition, 0.3f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition, endPosition);

        // vision Cone Visualization
        Gizmos.color = isChasing ? Color.red : Color.cyan;
        Vector3 leftBoundary = Quaternion.AngleAxis(-viewAngle / 2f, Vector3.forward) * transform.right;
        Vector3 rightBoundary = Quaternion.AngleAxis(viewAngle / 2f, Vector3.forward) * transform.right;
        
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
