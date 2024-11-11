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
        return Physics.Raycast(transform.position, (player.position - transform.position).normalized, out RaycastHit hit, detectionRange, playerLayer) && hit.transform == player;
    }
}

public class PatrolState : IState
{
    private MeleeAI ai;
    private int currentWaypoint;

    public StateType Type => StateType.Patrol;

    public PatrolState(MeleeAI ai, Transform[] waypoints) { this.ai = ai; }

    public void Enter() { MoveToNextWaypoint(); }

    public void Execute()
    {
        if (ai.PlayerInSight()) ai.ChangeState(new ChaseState(ai));
        if (ai.agent.remainingDistance <= ai.agent.stoppingDistance) MoveToNextWaypoint();
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
    private MeleeAI ai;  // or ShooterAI, depending on which AI type is using this state

    public StateType Type => StateType.Chase;

    public ChaseState(MeleeAI ai) { this.ai = ai; }

    public void Enter() { ai.agent.isStopped = false; }

    public void Execute()
    {
        ai.agent.destination = ai.player.position;

        // Make the AI face the player
        Vector3 direction = (ai.player.position - ai.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (Vector3.Distance(ai.transform.position, ai.player.position) < ai.attackRange)
            ai.ChangeState(new AttackState(ai));
    }

    public void Exit() { }
}

// Inside MeleeAI.cs
public class AttackState : IState
{
    private MeleeAI ai;
    private WeaponController weaponController;

    public StateType Type => StateType.Attack;

    public AttackState(MeleeAI ai)
    {
        this.ai = ai;
        weaponController = ai.GetComponent<WeaponController>(); // Access WeaponController
    }

    public void Enter()
    {
        // Start melee animation
        weaponController.MeleeWeapon.SetActive(true);  // Ensure melee weapon is active
    }

    public void Execute()
    {
        Vector3 direction = (ai.player.position - ai.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, lookRotation, Time.deltaTime * 5f);
        // Check distance to player for attack
        if (Vector3.Distance(ai.transform.position, ai.player.position) > ai.attackRange)
        {
            ai.ChangeState(new ChaseState(ai));  // Return to Chase if player is out of range
        }
        else
        {
            Debug.Log("Attacking Player!");
            // Trigger melee attack logic (e.g., play animation)
            // This could also include activating collider if using physics-based melee detection
        }
    }

    public void Exit()
    {
        weaponController.MeleeWeapon.SetActive(false); // Deactivate weapon if needed
    }
}