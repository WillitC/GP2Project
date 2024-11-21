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
    public Transform bestCover;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new MoveState(this, coverPoints));
    }

    public bool PlayerInSight()
    {
        return Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hit, detectionRange, playerLayer) && hit.transform == player;
    }

    public Transform FindClosestCover()
    {
        bestCover = null;
        float minDistance = Mathf.Infinity;
        foreach (Transform cover in coverPoints)
        {
            if (NavMesh.SamplePosition(cover.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                float distance = Vector3.Distance(transform.position, cover.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestCover = cover;
                }
            }
        }
        return bestCover;
    }
}

public class MoveState : IState
{
    private ShooterAI ai;
    private int currentWaypoint;

    public StateType Type => StateType.Patrol;

    public MoveState(ShooterAI ai, Transform[] coverPoints) { this.ai = ai; }

    public void Enter() { MoveToNextWaypoint(); }

    public void Execute()
    {
        if (ai.PlayerInSight()) ai.ChangeState(new MoveToCoverState(ai, ai.coverPoints[currentWaypoint]));
        if (ai.agent.remainingDistance <= ai.agent.stoppingDistance) MoveToNextWaypoint();
    }

    public void Exit() { }

    private void MoveToNextWaypoint()
    {
        ai.agent.destination = ai.coverPoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % ai.coverPoints.Length;
    }
}

// Shooter-specific State Implementations
public class MoveToCoverState : IState
{
    private ShooterAI ai;
    private Transform coverPoint;

    public StateType Type => StateType.MoveToCover;

    public MoveToCoverState(ShooterAI aiController, Transform cover)
    {
        ai = aiController;
        coverPoint = cover;
    }

    public void Enter()
    {
        MoveToCover();
    }

    public void Execute()
    {
        CheckIfStuck(); // Ensure AI stays on the NavMesh

        if (!ai.agent.pathPending && ai.agent.remainingDistance <= ai.agent.stoppingDistance)
        {
            if (CanSeePlayer())
            {
                ai.ChangeState(new TakeCoverState(ai, coverPoint)); // Transition to TakeCoverState
                return;
            }

            MoveToNewCoverPoint(); // Find another cover point
        }
    }

    public void Exit()
    {
        ai.agent.isStopped = false;
    }

    private void MoveToCover()
    {
        Vector3 offsetPosition = coverPoint.position - coverPoint.forward * 1.0f;
        ai.agent.SetDestination(offsetPosition);
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (ai.player.position - ai.transform.position).normalized;
        if (Physics.Raycast(ai.transform.position, directionToPlayer, out RaycastHit hit))
        {
            return hit.transform.CompareTag("Player");
        }
        return false;
    }

    private void MoveToNewCoverPoint()
    {
        Transform newCoverPoint = ai.FindClosestCover();
        if (newCoverPoint != null)
        {
            coverPoint = newCoverPoint;
            MoveToCover();
        }
        else
        {
            Debug.LogWarning("No valid cover points found!");
        }
    }

    private void CheckIfStuck()
    {
        if (!ai.agent.isOnNavMesh)
        {
            Debug.LogError("AI is off the NavMesh! Warping back.");
            NavMeshHit hit;
            if (NavMesh.SamplePosition(ai.transform.position, out hit, 2.0f, NavMesh.AllAreas))
            {
                ai.agent.Warp(hit.position);
            }
        }
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
            ai.ChangeState(new MoveToCoverState(ai, ai.bestCover));
        }
    }

    public void Exit()
    {
        // Optional: Stop aiming or reset state
    }
}

public class TakeCoverState : IState
{
    private ShooterAI ai;
    private Transform coverPoint;

    public StateType Type => StateType.TakeCover;

    public TakeCoverState(ShooterAI aiController, Transform cover)
    {
        ai = aiController;
        coverPoint = cover;
    }

    public void Enter()
    {
        Debug.Log("Taking cover at position: " + coverPoint.position);
        ai.agent.isStopped = true; // Stop movement when taking cover
    }

    public void Execute()
    {
        PeekFromCover(); // Method to peek and shoot
    }

    public void Exit()
    {
        Debug.Log("Exiting Take Cover State");
        ai.agent.isStopped = false;
    }

    private void PeekFromCover()
    {
        if (CanSeePlayer())
        {
            Debug.Log("Peeking and shooting at player.");
            ai.ChangeState(new ShootState(ai));
        }
        else
        {
            Debug.Log("Player not visible. Staying behind cover.");
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (ai.player.position - ai.transform.position).normalized;
        if (Physics.Raycast(ai.transform.position, directionToPlayer, out RaycastHit hit))
        {
            return hit.transform.CompareTag("Player");
        }
        return false;
    }
}