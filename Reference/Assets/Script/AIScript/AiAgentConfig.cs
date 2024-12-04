using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float shooterMaxSightDistance = 15.0f;
    public float meleeMaxSightDistance = 10.0f;
    public float maxSightDistance = 5.0f;
    public float shootCooldown = 1.0f;
    public float attackCooldown = 1.0f;
    public float meleeAttackRange = 1.5f; // Range for melee attacks
    public float chaseGracePeriod = 2.0f; // Time to wait before exiting ChasePlayer
}
