global using Kaskada.Minify.Cursors;
global using Kaskada.Minify.Outputs;
using Kaskada.Minify.States;

namespace Kaskada.Minify;

public sealed class Minifier : IDisposable
{
    private readonly ICursor _cursor;
    private readonly StateMachine _stateMachine;

    private Minifier(ICursor cursor)
    {
        _cursor = cursor;
        _stateMachine = new StateMachine();
    }

    public static Minifier FromFile(string filename) => new(new FileCursor(filename));

    public static Minifier FromText(string text) => new(new TextCursor(text));

    public void Dispose() => _cursor.Dispose();

    public void Minify(IOutput output)
    {
        _stateMachine.PushState<WhitespaceState>();
        while (_cursor.HasMoreInput)
        {
            _stateMachine.Process(_cursor, output);
            _cursor.MoveNext();
        }
        output.Commit();
    }
}
