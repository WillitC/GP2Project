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
        return Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hit, detectionRange, playerLayer) && hit.transform == player;
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

    public StateType Type => StateType.MoveToCover;

    public MoveToCoverState(ShooterAI ai)
    {
        this.ai = ai;
    }

    public void Enter()
    {
        coverPoint = ai.FindClosestCover();
        if (coverPoint != null)
        {
            Debug.Log("Moving to cover point: " + coverPoint.position);
            ai.agent.destination = coverPoint.position;
            ai.agent.isStopped = false;  // Ensure the agent is not stopped
            Debug.Log("NavMeshAgent isStopped: " + ai.agent.isStopped); // Additional check
        }
        else
        {
            Debug.LogWarning("No cover points found for AI.");
        }
    }

    public void Execute()
    {
        if (ai.agent.pathPending)
        {
            Debug.Log("Waiting for path to be calculated...");
        }
        else if (ai.agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogError("Path is invalid. Agent can't reach the destination.");
        }
        else if (ai.agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogWarning("Partial path found. The agent may not be able to reach cover completely.");
        }
        else if (ai.agent.remainingDistance > ai.agent.stoppingDistance)
        {
            Debug.Log("Moving towards cover. Distance to cover: " + ai.agent.remainingDistance);
        }
        else if (ai.agent.remainingDistance <= ai.agent.stoppingDistance && ai.PlayerInSight())
        {
            Debug.Log("Reached cover point. Switching to ShootState.");
            ai.ChangeState(new ShootState(ai));
        }
        else
        {
            Debug.Log("Unexpected: Agent not moving.");
        }
    }

    public void Exit()
    {
        ai.agent.isStopped = true;
    }
}

public class ShootState : IState
{
    private ShooterAI ai;
    private WeaponController weaponController;

    public StateType Type => StateType.Shoot;

    public ShootState(ShooterAI ai)
    {
        this.ai = ai;
        weaponController = ai.GetComponent<WeaponController>(); // Access WeaponController
    }

    public void Enter()
    {
        // Optional: Start aiming animation or shooting preparation
    }

    public void Execute()
    {
        if (ai.PlayerInSight() && Vector3.Distance(ai.transform.position, ai.player.position) <= ai.shootingRange)
        {
            // Use weapon firing logic based on ai's weapon type
            if (weaponController.rangedType == "charge")
            {
                weaponController.chargeRanged();  // Adjust to AI-specific firing method if necessary
            }
            else
            {
                weaponController.autoRanged();  // Continuous fire for automatic weapons
            }
        }
        else
        {
            ai.ChangeState(new MoveToCoverState(ai));
        }
    }

    public void Exit()
    {
        // Optional: Stop aiming or reset state
    }
}
