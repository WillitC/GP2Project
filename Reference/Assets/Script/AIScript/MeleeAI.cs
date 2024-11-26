using UnityEngine;
using UnityEngine.AI;

public class MeleeAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public NavMeshAgent agent; // NavMeshAgent for movement
    public float health = 100f; // AI health
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}