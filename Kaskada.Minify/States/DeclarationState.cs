namespace Kaskada.Minify.States;

internal class DeclarationState : State
{
    private bool _hasSemicolonInBuffer;

    private static readonly HashSet<char> PreDelimiters = new()
    {
        '{', ':', ';', '/'
    };

    private static readonly HashSet<char> PostDelimiters = new()
    {
        '}', ':', ')'
    };

    public DeclarationState(StateMachine stateMachine)
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

        if (cursor.Current == '/' && cursor.Peek() == '*')
        {
            StateMachine.PushState<CommentState>();
            return;
        }

        if (cursor.Current is ';')
        {
            _hasSemicolonInBuffer = true;
            return;
        }

        if (cursor.Current is '}')
        {
            _hasSemicolonInBuffer = false;
            StateMachine.PopState();
        }

        if (_hasSemicolonInBuffer)
        {
            _hasSemicolonInBuffer = false;
            output.Append(';');
        }

        output.Append(cursor.Current);
    }
}
