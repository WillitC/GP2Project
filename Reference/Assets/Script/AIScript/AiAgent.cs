using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AiType
{
    Melee,
    Shooter
}

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public Transform player;
    public AiType aiType;
    public Animator animator;
    public AiWeaponHandler weaponHandler;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        weaponHandler = GetComponent<AiWeaponHandler>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.ChangeState(initialState);
        if (aiType == AiType.Melee)
        {
            weaponHandler.EquipWeapon(weaponHandler.meleeWeaponPrefab);
        }
        else if (aiType == AiType.Shooter)
        {
            weaponHandler.EquipWeapon(weaponHandler.gunWeaponPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
