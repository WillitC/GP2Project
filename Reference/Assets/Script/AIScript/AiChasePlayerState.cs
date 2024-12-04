using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    float timer = 0.0f;
    float graceTimer; // Timer for ChasePlayer exit grace period

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {
        Debug.Log("Entering Chase State");
        timer = 0.0f;
        graceTimer = 0.0f; // Reset grace period timer
        if (agent.navMeshAgent != null)
        {
            agent.navMeshAgent.isStopped = false; // Ensure the agent starts moving
        }
    }

    public void Update(AiAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;

        // Ensure the agent keeps moving towards the player
        if (!agent.navMeshAgent.hasPath || agent.navMeshAgent.remainingDistance < 0.5f)
        {
            agent.navMeshAgent.destination = agent.player.position;
        }

        // Adjust destination periodically based on distance
        if (timer < 0.0f)
        {
            Vector3 direction = (agent.player.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.player.position;
                }
            }
            timer = agent.config.maxTime;
        }

        // Dynamically set maxSightDistance based on AI type
        agent.config.maxSightDistance = (agent.aiType == AiType.Shooter)
            ? agent.config.shooterMaxSightDistance
            : agent.config.meleeMaxSightDistance;

        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.player.position);

        if (distanceToPlayer <= agent.config.maxSightDistance && HasLineOfSight(agent))
        {
            graceTimer = 0.0f; // Reset the grace timer if player is in sight

            if (agent.aiType == AiType.Melee && distanceToPlayer <= agent.config.meleeAttackRange)
            {
                agent.stateMachine.ChangeState(AiStateId.Attack);
            }
            else if (agent.aiType == AiType.Shooter)
            {
                agent.stateMachine.ChangeState(AiStateId.Attack);
            }
        }
        else
        {
            // Increment the grace timer if player is out of sight
            graceTimer += Time.deltaTime;

            if (graceTimer > agent.config.chaseGracePeriod)
            {
                // Transition to Idle state if grace period expires
                agent.stateMachine.ChangeState(AiStateId.Idle);
            }
        }
    }

    public void Exit(AiAgent agent)
    {
        if (agent.navMeshAgent != null)
        {
            agent.navMeshAgent.isStopped = true; // Stop the agent when leaving the state
        }
    }

    // Helper function to check if the AI has a clear line of sight to the player
    private bool HasLineOfSight(AiAgent agent)
    {
        Vector3 directionToPlayer = (agent.player.position - agent.transform.position).normalized;
        if (Physics.Raycast(agent.transform.position + Vector3.up * 1.0f, directionToPlayer, out RaycastHit hit, agent.config.maxSightDistance))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
}
