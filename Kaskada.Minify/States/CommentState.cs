namespace Kaskada.Minify.States;

internal class CommentState : State
{
    public CommentState(StateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Process(ICursor cursor, IOutput output)
    {
        if (cursor.Current == '/' && cursor.Previous == '*')
        {
            StateMachine.PopState();
        }
    }
}
