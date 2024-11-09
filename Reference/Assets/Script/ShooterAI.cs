using UnityEngine;
using UnityEngine.AI;

public class ShooterAI : StateController
{
    public Transform player;
    public float detectionRange = 20f;
    public float shootingRange = 15f;
    public Transform[] coverPoints;
    public LayerMask playerLayer;

    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new MoveToCoverState(this));
    }

    public bool PlayerInSight()
    {
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hit, detectionRange, playerLayer))
        {
            return hit.transform == player;
        }
        return false;
    }

    public Transform FindClosestCover()
    {
        Transform bestCover = null;
        float minDistance = Mathf.Infinity;
        foreach (Transform cover in coverPoints)
        {
            float distance = Vector3.Distance(transform.position, cover.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                bestCover = cover;
            }
        }
        return bestCover;
    }
}

// Shooter-specific State Implementations
public class MoveToCoverState : IState
{
    private ShooterAI ai;
    private Transform coverPoint;

    public StateType Type => StateType.MoveToCover;  // Implementing the Type property

    public MoveToCoverState(ShooterAI ai)
    {
        this.ai = ai;
        coverPoint = ai.FindClosestCover();
    }

    public void Enter() { ai.agent.destination = coverPoint.position; }

    public void Execute()
    {
        if (ai.agent.remainingDistance < 1.0f && ai.PlayerInSight())
            ai.ChangeState(new ShootState(ai));
    }

    public void Exit() { }
}

public class ShootState : IState
{
    private ShooterAI ai;

    public StateType Type => StateType.Shoot;  // Implementing the Type property

    public ShootState(ShooterAI ai) { this.ai = ai; }

    public void Enter() { /* Start shooting animation */ }

    public void Execute()
    {
        if (!ai.PlayerInSight() || Vector3.Distance(ai.transform.position, ai.player.position) > ai.shootingRange)
            ai.ChangeState(new MoveToCoverState(ai));
    }

    public void Exit() { /* Stop shooting animation */ }
}
