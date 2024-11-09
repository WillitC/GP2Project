// IState.cs
public interface IState
{
    StateType Type { get; }
    void Enter();
    void Execute();
    void Exit();
}