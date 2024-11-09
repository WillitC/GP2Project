using UnityEngine;
using UnityEngine.AI;

public class MeleeAI : StateController
{
    public Transform player;
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public LayerMask playerLayer;
    public Transform[] waypoints;

    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new PatrolState(this, waypoints));
    }

    public bool PlayerInSight()
    {
        // Raycast to check if there's a clear line of sight to the player
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hit, detectionRange, playerLayer))
        {
            return hit.transform == player;
        }
        return false;
    }
}

public class PatrolState : IState
{
    private MeleeAI ai;
    private int currentWaypoint;

    public StateType Type => StateType.Patrol;  // Implementing the Type property

    public PatrolState(MeleeAI ai, Transform[] waypoints) { this.ai = ai; }

    public void Enter() { MoveToNextWaypoint(); }

    public void Execute()
    {
        if (ai.PlayerInSight()) ai.ChangeState(new ChaseState(ai));
        if (ai.agent.remainingDistance < 1.0f) MoveToNextWaypoint();
    }

    public void Exit() { }

    private void MoveToNextWaypoint()
    {
        ai.agent.destination = ai.waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % ai.waypoints.Length;
    }
}

public class ChaseState : IState
{
    private MeleeAI ai;

    public StateType Type => StateType.Chase;  // Implementing the Type property

    public ChaseState(MeleeAI ai) { this.ai = ai; }

    public void Enter() { ai.agent.isStopped = false; }

    public void Execute()
    {
        ai.agent.destination = ai.player.position;
        if (Vector3.Distance(ai.transform.position, ai.player.position) < ai.attackRange)
            ai.ChangeState(new AttackState(ai));
    }

    public void Exit() { }
}

// Inside MeleeAI.cs
public class AttackState : IState
{
    private MeleeAI ai;

    public StateType Type => StateType.Attack;

    public AttackState(MeleeAI ai)
    {
        this.ai = ai;
    }

    public void Enter() { /* Start attack animation */ }

    public void Execute()
    {
        // Attack logic here

        if (Vector3.Distance(ai.transform.position, ai.player.position) > ai.attackRange)
        {
            ai.ChangeState(new ChaseState(ai));
        }
    }

    public void Exit() { /* Stop attack animation */ }
}