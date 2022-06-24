namespace Kaskada.Minify.States;

internal class StateMachine
{
    private readonly Stack<State> _states;
    private readonly IDictionary<Type, State> _stateTable;

    public StateMachine()
    {
        _states = new();
        _stateTable = new Dictionary<Type, State>
        {
            {typeof(WhitespaceState), new WhitespaceState(this)},
            {typeof(SelectorState), new SelectorState(this)},
            {typeof(CommentState), new CommentState(this)},
            {typeof(AtRuleState), new AtRuleState(this)},
            {typeof(DeclarationState), new DeclarationState(this)},
        };
    }

    public void PushState<TState>() where TState : State
    {
        Type key = typeof(TState);
        if (!_stateTable.TryGetValue(
                key,
                out State? state))
        {
            throw new KaskadaException($"Unknown state type requested: {key.Name}");
        }

        _states.Push(state);
    }

    public void PushStateAndProcess<TState>(ICursor cursor,
        IOutput output) where TState : State
    {
        PushState<TState>();
        Process(cursor, output);
    }

    public void PopState()
        => _states.Pop();

    public void Process(ICursor cursor, IOutput output)
        => _states.Peek().Process(cursor, output);
}
