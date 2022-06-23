namespace Kaskada.Minify.States;

internal class WhitespaceState : State
{
    public WhitespaceState(StateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Process(ICursor cursor, IOutput output)
    {
        if (cursor.Current is '.' or '#' || char.IsLetter(cursor.Current))
        {
            StateMachine.PushStateAndProcess<SelectorState>(cursor, output);
            return;
        }

        if (cursor.Current == '/' && cursor.Peek() == '*')
        {
            StateMachine.PushState<CommentState>();
            return;
        }

        if (cursor.Current == '@')
        {
            StateMachine.PushStateAndProcess<AtRuleState>(cursor, output);
        }
    }
}
