using UnityEngine;

public class AiAttackState : AiState
{
    private float attackTimer = 0f; // Tracks cooldown between attacks

    public AiStateId GetId() => AiStateId.Attack;

    public void Enter(AiAgent agent)
    {
        // Reset attack timer on entering the attack state
        attackTimer = 0f;
        // Set maxSightDistance based on AI type
        agent.config.maxSightDistance = (agent.aiType == AiType.Shooter)
            ? agent.config.shooterMaxSightDistance
            : agent.config.meleeMaxSightDistance;
        //Debug.Log("Entering Attack state");
        // Optional: Face the player immediately
        FacePlayer(agent);

        // Trigger an initial attack
        PerformAttack(agent);
    }

    public void Update(AiAgent agent)
    {
        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.player.position);

        // Return to ChasePlayer if the player is out of melee range for melee AI
        if (agent.aiType == AiType.Melee && distanceToPlayer > agent.config.meleeAttackRange)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            return;
        }

        // If player is out of range, transition back to ChasePlayer
        if (distanceToPlayer > agent.config.maxSightDistance || !HasLineOfSight(agent))
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            return;
        }

        // Increment the attack timer
        attackTimer += Time.deltaTime;

        // Determine attack behavior based on AI type
        if (attackTimer >= GetCooldown(agent))
        {
            attackTimer = 0f; // Reset attack timer
            PerformAttack(agent);
        }
    }

    public void Exit(AiAgent agent)
    {
        
    }

    private void PerformAttack(AiAgent agent)
    {
        if (agent.aiType == AiType.Shooter)
        {
            ShooterAttack(agent);
        }
        else if (agent.aiType == AiType.Melee)
        {
            MeleeAttack(agent);
        }
    }

    private void ShooterAttack(AiAgent agent)
    {
        // Face the player before shooting
        FacePlayer(agent);

        agent.animator.SetTrigger("Shoot");

        // Call the AI weapon controller to handle shooting
        AIWeaponController weaponController = agent.GetComponent<AIWeaponController>();
        if (weaponController != null)
        {
             weaponController.ShootAtTarget(agent.player.position);
        }
    }

    private void MeleeAttack(AiAgent agent)
    {
        // Face the player before attacking
        FacePlayer(agent);

        agent.animator.SetTrigger("attack");

        // Add your damage application logic here
        Debug.Log("Melee attack executed!");
    }

    private float GetCooldown(AiAgent agent)
    {
        // Return the appropriate cooldown based on AI type
        return agent.aiType == AiType.Shooter ? agent.config.shootCooldown : agent.config.attackCooldown;
    }

    private void FacePlayer(AiAgent agent)
    {
        Vector3 directionToPlayer = (agent.player.position - agent.transform.position).normalized;
        directionToPlayer.y = 0; // Keep only horizontal rotation
        agent.transform.forward = directionToPlayer;
    }

    private bool HasLineOfSight(AiAgent agent)
    {
        // Perform a raycast to check if the player is visible
        Vector3 directionToPlayer = (agent.player.position - agent.transform.position).normalized;
        if (Physics.Raycast(agent.transform.position, directionToPlayer, out RaycastHit hit, agent.config.maxSightDistance))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
}
