namespace Kaskada.Minify.States;

internal class SelectorState : State
{
    private static readonly HashSet<char> PreDelimiters = new()
    {
        '[', '{', ':', ';', '=', '!', '&', '|', '?', '+', '-', '~', '*', '/', '\n', '\r', '>', ',', '@'
    };

    private static readonly HashSet<char> PostDelimiters = new()
    {
        '(', '[', '{', ':', ';', '=', '!', '&', '|', '?', '+', '-', '~', '*', '/', '\n', '\r', '>', ',', '@', ')', '}'
    };

    public SelectorState(StateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Process(ICursor cursor, IOutput output)
    {
        if (char.IsWhiteSpace(cursor.Current))
        {
            if (char.IsWhiteSpace(cursor.Previous) || PreDelimiters.Contains(cursor.Previous))
            {
                return;
            }

            char? next = cursor.Peek();
            if (next is null)
            {
                return;
            }

            if (char.IsWhiteSpace(next.Value) || PostDelimiters.Contains(next.Value))
            {
                return;
            }
        }

        if (cursor.Current is ';')
        {
            return;
        }

        if (cursor.Current is '{')
        {
            StateMachine.PopState();
            StateMachine.PushState<DeclarationState>();
        }

        output.Append(cursor.Current);
    }
}
