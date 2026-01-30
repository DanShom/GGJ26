using Pathfinding;
using TarodevController;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class AIPlayerController2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float activateDistance = 50f;
    [SerializeField] private float pathUpdateSeconds = 0.5f;

    [Header("Movement")]
    [SerializeField] private float nextWaypointDistance = 1.2f;
    [SerializeField] private float jumpNodeHeightRequirement = 0.8f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private PlayerController controller;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Collider2D col;

    private Path path;
    private int currentWaypoint;

    private bool grounded;
    private float lastJumpTime;
    private const float JumpCooldown = 0.2f;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateSeconds);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdatePath));
    }

    private void FixedUpdate()
    {
        if (!TargetInRange())
        {
            controller.SetAIInput(Vector2.zero, false, false);
            return;
        }

        FollowPath();
    }

    private void UpdatePath()
    {
        if (!TargetInRange() || !seeker.IsDone()) return;
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    private void FollowPath()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            controller.SetAIInput(Vector2.zero, false, false);
            return;
        }

        CheckGrounded();

        Vector2 toWaypoint = (Vector2)path.vectorPath[currentWaypoint] - rb.position;

        float moveX = Mathf.Sign(toWaypoint.x);
        bool wantsJump =
            grounded &&
            Time.time > lastJumpTime + JumpCooldown &&
            toWaypoint.y > jumpNodeHeightRequirement;

        controller.SetAIInput(
            new Vector2(moveX, 0),
            wantsJump,
            wantsJump
        );

        if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            currentWaypoint++;
    }

    private void CheckGrounded()
    {
        Vector2 origin = (Vector2)col.bounds.center +
                         Vector2.down * (col.bounds.extents.y + 0.02f);

        grounded = Physics2D.Raycast(
            origin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );
    }

    private bool TargetInRange()
    {
        return Vector2.Distance(rb.position, target.position) <= activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (p.error) return;
        path = p;
        currentWaypoint = 0;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (path == null) return;
        Gizmos.color = Color.red;
        for (int i = currentWaypoint; i < path.vectorPath.Count; i++)
        {
            Gizmos.DrawSphere(path.vectorPath[i], 0.1f);
        }
    }
#endif
}
