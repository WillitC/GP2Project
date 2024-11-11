using UnityEngine;

public class StateController : MonoBehaviour
{
    protected IState currentState;
    public StateType CurrentStateType => currentState?.Type ?? StateType.Patrol;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void Update()
    {
        currentState?.Execute();
    }
}