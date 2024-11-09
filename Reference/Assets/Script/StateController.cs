using System.Collections;
using System.Collections.Generic;
// StateController.cs
using UnityEngine;

public class StateController : MonoBehaviour
{
    private IState currentState;
    public StateType CurrentStateType => currentState.Type;

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