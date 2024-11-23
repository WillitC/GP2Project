using UnityEngine;
using UnityEngine.AI;

public class MeleeAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float detectionRange = 15f; // Range to detect the player
    public float attackRange = 2f; // Range to attack the player
    public LayerMask whatIsGround, whatIsPlayer; // Layers for ground and player
    public Transform[] waypoints; // Patrol waypoints
    public NavMeshAgent agent; // NavMeshAgent for movement
    public float timeBetweenAttacks = 1f; // Cooldown between attacks
    public bool alreadyAttacked;
    public float walkPointRange = 10f; // Patrol range
    public float health = 100f; // AI health

    private Vector3 walkPoint; // Random patrol destination
    private bool walkPointSet;
    private bool playerInSightRange, playerInAttackRange;
    Animator animator;
    float timer = 0.0f;
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            float distance = (player.position - agent.destination).sqrMagnitude;
            if (distance > maxDistance * maxDistance)
            {
                agent.destination = player.position;
            }
            timer = maxTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
        //// Check if player is in sight or attack range
        //playerInSightRange = Physics.CheckSphere(transform.position, detectionRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //// State transitions
        //if (!playerInSightRange && !playerInAttackRange) Patroling();
        //if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        //if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    //private void Patroling()
    //{
    //    if (!walkPointSet) SearchWalkPoint();

    //    if (walkPointSet)
    //        agent.SetDestination(walkPoint);

    //    Vector3 distanceToWalkPoint = transform.position - walkPoint;

    //    // If the AI reached the walk point
    //    if (distanceToWalkPoint.magnitude < 1f)
    //        walkPointSet = false;
    //}

    //private void SearchWalkPoint()
    //{
    //    // Generate a random point within the patrol range
    //    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //    float randomX = Random.Range(-walkPointRange, walkPointRange);

    //    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    //    // Ensure the point is on the ground
    //    if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
    //        walkPointSet = true;
    //}

    //private void ChasePlayer()
    //{
    //    // Move towards the player
    //    agent.SetDestination(player.position);
    //}

    //private void AttackPlayer()
    //{
    //    // Stop moving
    //    agent.SetDestination(transform.position);

    //    // Face the player
    //    transform.LookAt(player);

    //    if (!alreadyAttacked)
    //    {
    //        // Attack logic here (e.g., damage the player)
    //        Debug.Log("Attacking Player!");

    //        alreadyAttacked = true;
    //        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //    }
    //}

    //private void ResetAttack()
    //{
    //    alreadyAttacked = false;
    //}

    //public void TakeDamage(int damage)
    //{
    //    health -= damage;

    //    if (health <= 0) DestroyEnemy();
    //}

    //private void DestroyEnemy()
    //{
    //    Destroy(gameObject);
    //}
}

//public class PatrolState : IState
//{
//    private MeleeAI ai;
//    private int currentWaypoint;

//    public StateType Type => StateType.Patrol;

//    public PatrolState(MeleeAI ai, Transform[] waypoints) { this.ai = ai; }

//    public void Enter() { MoveToNextWaypoint(); }

//    public void Execute()
//    {
//        if (ai.PlayerInSight())
//        {
//            ai.ChangeState(new ChaseState(ai));
//        }
//        else if (ai.agent.remainingDistance <= ai.agent.stoppingDistance)
//        {
//            MoveToNextWaypoint();
//        }
//    }

//    public void Exit() { }

//    private void MoveToNextWaypoint()
//    {
//        ai.agent.destination = ai.waypoints[currentWaypoint].position;
//        currentWaypoint = (currentWaypoint + 1) % ai.waypoints.Length;
//    }
//}

//public class ChaseState : IState
//{
//    private MeleeAI ai;  // or ShooterAI, depending on which AI type is using this state

//    public StateType Type => StateType.Chase;

//    public ChaseState(MeleeAI ai) { this.ai = ai; }

//    public void Enter() { ai.agent.isStopped = false; }

//    public void Execute()
//    {
//        ai.agent.destination = ai.player.position;

//        if (Vector3.Distance(ai.transform.position, ai.player.position) <= ai.attackRange)
//        {
//            ai.ChangeState(new AttackState(ai));
//        }
//    }

//    public void Exit() { }
//}

//// Inside MeleeAI.cs
//public class AttackState : IState
//{
//    private MeleeAI ai;
//    private MeleeAIWeaponController weaponController;

//    public StateType Type => StateType.Attack;

//    public AttackState(MeleeAI ai)
//    {
//        this.ai = ai;
//        weaponController = ai.GetComponent<MeleeAIWeaponController>(); // Access the melee weapon controller
//    }

//    public void Enter()
//    {
//        Debug.Log("Entering Melee Attack State");
//        weaponController.MeleeWeapon.SetActive(true); // Enable melee weapon visuals
//        if (Vector3.Distance(ai.transform.position, ai.player.position) <= ai.attackRange)
//        {
//            ai.ChangeState(new AttackState(ai));
//        }
//    }

//    public void Execute()
//    {
//        // Face the player
//        Vector3 direction = (ai.player.position - ai.transform.position).normalized;
//        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
//        ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, lookRotation, Time.deltaTime * 5f);

//        if (Vector3.Distance(ai.transform.position, ai.player.position) <= ai.attackRange)
//        {
//            Debug.Log("Attacking Player!");
//            weaponController.Attack(); // Trigger melee attack
//        }
//        else
//        {
//            ai.ChangeState(new ChaseState(ai)); // Switch to chasing if out of range
//        }
//    }

//    public void Exit()
//    {
//        Debug.Log("Exiting Melee Attack State");
//        weaponController.MeleeWeapon.SetActive(false); // Deactivate melee weapon visuals
//    }
//}