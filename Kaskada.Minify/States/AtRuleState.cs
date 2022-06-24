namespace Kaskada.Minify.States;

internal class AtRuleState : State
{
    private static readonly HashSet<char> PostDelimiters = new()
    {
        ')', '(', ':', '}'
    };

    private static readonly HashSet<char> PreDelimiters = new()
    {
        ')', '}', '@'
    };

    public AtRuleState(StateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Process(ICursor cursor, IOutput output)
    {
        if (char.IsWhiteSpace(cursor.Current))
        {
            if (char.IsWhiteSpace(cursor.Previous) || PostDelimiters.Contains(cursor.Previous))
            {
                return;
            }

            char? next = cursor.Peek();

            if (next is null || PreDelimiters.Contains(next.Value))
            {
                return;
            }
        }

        if (cursor.Current is '{')
        {
            StateMachine.PushState<SelectorState>();
        }
        else if (cursor.Current is ';' or '}')
        {
            StateMachine.PopState();
        }

        output.Append(cursor.Current);
    }
}
