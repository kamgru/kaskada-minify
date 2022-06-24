namespace Kaskada.Minify.States;

internal abstract class State
{
    protected readonly StateMachine StateMachine;

    protected State(StateMachine stateMachine)
        => StateMachine = stateMachine;

    public abstract void Process(ICursor cursor, IOutput output);
}
