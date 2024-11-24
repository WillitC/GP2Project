using UnityEngine;
using UnityEngine.AI;

public class ShooterAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}