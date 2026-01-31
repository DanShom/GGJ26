using Pathfinding;
using TarodevController;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class AICharacterAgent2D : MonoBehaviour
{
    [Header("Pathfinding")]
    [SerializeField] private float pathUpdateSeconds = 0.5f;
    [SerializeField] private float nextWaypointDistance = 1.2f;

    [Header("Jumping")]
    [SerializeField] private float jumpNodeHeightRequirement = 0.8f;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Arrival")]
    [SerializeField] private float arrivalTolerance = 0.2f;

    private CharacterMovement controller;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Collider2D col;

    private Path path;
    private int currentWaypoint;
    private Transform target;
    private Vector2 destination;

    private bool hasTarget;
    private bool grounded;
    private float lastJumpTime;
    private bool agentEnabled = true;

    #region Unity

    private void Awake()
    {
        controller = GetComponent<CharacterMovement>();
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
        if (!agentEnabled || !hasTarget)
        {
            controller.SetInput(Vector2.zero, false, false);
            return;
        }

        FollowPath();
    }

    #endregion

    #region Public API (AI as a Service)

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasTarget = target != null;

        if (hasTarget)
            RequestPath();
    }

    public void MoveTo(Vector2 worldPosition)
    {
        destination = worldPosition;
        target = null;
        hasTarget = true;
        RequestPath();
    }

    public void ClearTarget()
    {
        hasTarget = false;
        path = null;
        currentWaypoint = 0;
        controller.SetInput(Vector2.zero, false, false);
    }

    public void Stop()
    {
        ClearTarget();
    }

    public void ForceJump()
    {
        controller.SetInput(Vector2.zero, true, false);
    }

    public void SetEnabled(bool enabled)
    {
        agentEnabled = enabled;

        if (!enabled)
            controller.SetInput(Vector2.zero, false, false);
    }

    /// <summary>
    /// True when the agent has reached the final path position
    /// within a small tolerance.
    /// </summary>
    public bool HasReachedDestination()
    {
        if (path == null || path.vectorPath == null || path.vectorPath.Count == 0)
            return true;

        if (currentWaypoint < path.vectorPath.Count)
            return false;

        Vector2 endPoint = path.vectorPath[path.vectorPath.Count - 1];
        return Vector2.Distance(rb.position, endPoint) <= arrivalTolerance;
    }

    public bool IsPathPending()
    {
        return !seeker.IsDone();
    }

    public Vector2 GetCurrentDestination()
    {
        return target ? (Vector2)target.position : destination;
    }

    #endregion

    #region Internal Logic

    private void UpdatePath()
    {
        if (!agentEnabled || !hasTarget || !seeker.IsDone())
            return;

        Vector2 end = GetCurrentDestination();
        seeker.StartPath(rb.position, end, OnPathComplete);
    }

    private void RequestPath()
    {
        if (!seeker.IsDone())
            return;

        seeker.StartPath(rb.position, GetCurrentDestination(), OnPathComplete);
    }

    private void FollowPath()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            controller.SetInput(Vector2.zero, false, false);
            return;
        }

        CheckGrounded();

        Vector2 toWaypoint = (Vector2)path.vectorPath[currentWaypoint] - rb.position;
        float moveX = Mathf.Sign(toWaypoint.x);

        bool wantsJump =
            grounded &&
            Time.time > lastJumpTime + jumpCooldown &&
            toWaypoint.y > jumpNodeHeightRequirement;

        if (wantsJump)
            lastJumpTime = Time.time;

        controller.SetInput(
            new Vector2(moveX, 0),
            wantsJump,
            wantsJump
        );

        if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            currentWaypoint++;
    }

    private void CheckGrounded()
    {
        Vector2 origin =
            (Vector2)col.bounds.center +
            Vector2.down * (col.bounds.extents.y + 0.02f);

        grounded = Physics2D.Raycast(
            origin,
            Vector2.down,
            0.1f,
            groundLayer
        );
    }

    private void OnPathComplete(Path p)
    {
        if (p.error)
            return;

        path = p;
        currentWaypoint = 0;
    }

    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (path == null)
            return;

        Gizmos.color = Color.cyan;
        for (int i = currentWaypoint; i < path.vectorPath.Count; i++)
        {
            Gizmos.DrawSphere(path.vectorPath[i], 0.1f);
        }
    }
#endif
}
