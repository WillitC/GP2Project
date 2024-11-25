using UnityEngine;

public class AiAttackState : AiState
{
    private float attackTimer = 0f; // Tracks cooldown between attacks

    public AiStateId GetId() => AiStateId.Attack;

    public void Enter(AiAgent agent)
    {
        // Reset attack timer on entering the attack state
        //attackTimer = 0f;
    }

    public void Update(AiAgent agent)
    {
        //float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.player.position);

        //// If player is out of range, transition back to ChasePlayer
        //if (distanceToPlayer > agent.config.maxSightDistance)
        //{
        //    agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        //    return;
        //}

        //// Increment the attack timer
        //attackTimer += Time.deltaTime;

        //// Determine attack behavior based on AI type
        //if (attackTimer >= GetCooldown(agent))
        //{
        //    attackTimer = 0f; // Reset attack timer

        //    if (agent.aiType == AiType.Shooter)
        //    {
        //        ShooterAttack(agent);
        //    }
        //    else if (agent.aiType == AiType.Melee)
        //    {
        //        MeleeAttack(agent);
        //    }
        //}
    }

    public void Exit(AiAgent agent)
    {
        // No ongoing coroutines to stop anymore, but you can add cleanup logic here if needed
    }

    private void ShooterAttack(AiAgent agent)
    {
        //agent.animator.SetTrigger("Shoot");

        //// Find the gun tip position
        //Transform gunTip = agent.weaponHandler.weaponSocket.GetChild(0).Find("GunTip");
        //if (gunTip != null)
        //{
        //    // Instantiate the bullet
        //    GameObject bullet = GameObject.Instantiate(agent.weaponHandler.gunWeaponPrefab, gunTip.position, gunTip.rotation);
        //    Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //    rb.velocity = gunTip.forward * 20f; // Set bullet speed
        //}
    }

    private void MeleeAttack(AiAgent agent)
    {
        //agent.animator.SetTrigger("Attack");

        // Add melee attack logic here
        // Example: Check if the player is within melee range and apply damage
    }

    //private float GetCooldown(AiAgent agent)
    //{
    //    // Return the appropriate cooldown based on AI type
    //    //return agent.aiType == AiType.Shooter ? agent.config.shootCooldown : agent.config.attackCooldown;
    //}
}
