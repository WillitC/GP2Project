using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }

    public void Enter(AiAgent agent)
    {
        //Debug.Log("Entering Idle State");
    }

    public void Update(AiAgent agent)
    {
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.player.position);

        // Check if the player is within sight distance
        if (distanceToPlayer <= agent.config.maxSightDistance && HasLineOfSight(agent))
        {
            // Transition to ChasePlayer state
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }

    public void Exit(AiAgent agent)
    {
      //  Debug.Log("Exiting Idle State");
    }

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
