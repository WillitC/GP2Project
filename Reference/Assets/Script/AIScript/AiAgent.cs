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
    public AiType aiType; // Set this in the Inspector
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.ChangeState(initialState);
        AiWeaponHandler weaponHandler = GetComponent<AiWeaponHandler>();
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
