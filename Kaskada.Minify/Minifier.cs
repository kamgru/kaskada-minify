global using Kaskada.Minify.Cursors;
global using Kaskada.Minify.Outputs;

namespace Kaskada.Minify;

public sealed class Minifier : IDisposable
{
    private readonly ICursor _cursor;

    private bool _hasSemicolonInBuffer;

    private static readonly HashSet<char> PreDelimiters = new()
    {
        '[', '{', ':', ';', '=', '!', '&', '|', '?', '+', '-', '~', '*', '/', '\n', '>', ',', '@', ')'
    };

    private static readonly HashSet<char> PostDelimiters = new()
    {
        '(', '[', '{', ':', ';', '=', '!', '&', '|', '?', '+', '-', '~', '*', '/', '\n', '>', ',', '@', ')', '}'
    };

    private Minifier(ICursor cursor) => _cursor = cursor;

    public static Minifier FromFile(string filename) => new(new FileCursor(filename));

    public static Minifier FromText(string text) => new(new TextCursor(text));

    public void Dispose() => _cursor.Dispose();

    public void Minify(IOutput output)
    {
        while (_cursor.HasMoreInput)
        {
            _cursor.MoveNext();

            if (_cursor.Current == '/' && _cursor.Peek() == '*')
            {
                while (_cursor.HasMoreInput)
                {
                    _cursor.MoveNext();

                    if (_cursor.Current == '*' && _cursor.Peek() == '/')
                    {
                        _cursor.MoveNext();
                        _cursor.MoveNext();

                        break;
                    }
                }
            }

            if (!_cursor.HasMoreInput)
            {
                break;
            }

            if (_cursor.Current is '\n')
            {
                continue;
            }

            if (char.IsWhiteSpace(_cursor.Current))
            {
                char? next = _cursor.Peek();

                if (next is null)
                {
                    continue;
                }

                if (char.IsWhiteSpace(_cursor.Previous) || char.IsWhiteSpace(next.Value))
                {
                    continue;
                }

                if (IsPostDelimiter(_cursor.Previous) || IsPreDelimiter(next.Value))
                {
                    continue;
                }
            }

            if (_cursor.Current is ';')
            {
                _hasSemicolonInBuffer = true;
                continue;
            }

            if (_hasSemicolonInBuffer)
            {
                if (_cursor.Current is not '}')
                {
                    output.Push(';');
                }

                _hasSemicolonInBuffer = false;
            }

            output.Push(_cursor.Current);
        }

        if (_hasSemicolonInBuffer)
        {
            output.Push(';');
        }

        output.Commit();
    }

    private static bool IsPreDelimiter(char value) =>
        PreDelimiters.Contains(value);

    private static bool IsPostDelimiter(char value) =>
        PostDelimiters.Contains(value);
}
