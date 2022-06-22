global using Kaskada.Minify.Cursors;
global using Kaskada.Minify.Outputs;

namespace Kaskada.Minify;

public enum State
{
    None,
    Comment,
    Selector,
    Declaration,
    AtRule
}

public sealed class Minifier : IDisposable
{
    private readonly ICursor _cursor;

    private readonly Stack<State> _states = new(new[] {State.None});

    private bool _hasSemicolonInBuffer;

    private static readonly HashSet<char> PreDelimiters = new()
    {
        '[',
        '{',
        ':',
        ';',
        '=',
        '!',
        '&',
        '|',
        '?',
        '+',
        '-',
        '~',
        '*',
        '/',
        '\n',
        '>',
        ',',
        '@',
    };

    private static readonly HashSet<char> PostDelimiters = new()
    {
        '(',
        '[',
        '{',
        ':',
        ';',
        '=',
        '!',
        '&',
        '|',
        '?',
        '+',
        '-',
        '~',
        '*',
        '/',
        '\n',
        '>',
        ',',
        '@',
        ')',
        '}'
    };

    private Minifier(ICursor cursor) => _cursor = cursor;

    public static Minifier FromFile(string filename) => new(new FileCursor(filename));

    public static Minifier FromText(string text) => new(new TextCursor(text));

    public void Dispose() => _cursor.Dispose();

    private void PushState(State state)
    {
        _states.Push(state);
    }
    
    private void HandleComment()
    {
        while (_cursor.HasMoreInput)
        {
            _cursor.MoveNext();

            if (_cursor.Current == '*' && _cursor.Peek() == '/')
            {
                _cursor.MoveNext();
                _cursor.MoveNext();
                _states.Pop();

                break;
            }
        }
    }

    private void HandleDeclaration(IOutput output)
    {
        if (char.IsWhiteSpace(_cursor.Current))
        {
            if (_cursor.Previous is '{' or ':' or ';' or '/')
                return;

            char? next = _cursor.Peek();
            if (next is '}' or ':' or ')')
                return;
        }
        
        if (_cursor.Current == '/' && _cursor.Peek() == '*')
        {
            PushState(State.Comment);
            return;
        }

        if (_cursor.Current is ';')
        {
            _hasSemicolonInBuffer = true;
            return;
        }
        
        if (_cursor.Current is '}')
        {
            _hasSemicolonInBuffer = false;
            _states.Pop();
        }
        
        if (_hasSemicolonInBuffer)
        {
            _hasSemicolonInBuffer = false;
            output.Push(';');
        }
        
        output.Push(_cursor.Current);
    }

    private void HandleSelector(IOutput output)
    {
        if (char.IsWhiteSpace(_cursor.Current))
        {
            if (IsPreDelimiter(_cursor.Previous))
            {
                return;
            }

            char? next = _cursor.Peek();
            if (next is null)
            {
                return;
            }

            if (IsPostDelimiter(next.Value))
            {
                return;
            }
        }
      
        if (_cursor.Current is ';')
        {
            return;
        }

        if (_cursor.Current is '{')
        {
            _states.Pop();
            PushState(State.Declaration);
        }

        output.Push(_cursor.Current);
    }

    private void HandleAtRule(IOutput output)
    {
        if (char.IsWhiteSpace(_cursor.Current))
        {
            if (_cursor.Previous == ' ')
            {
                return;
            }

            if (_cursor.Previous is ')' or '(')
            {
                return;
            }

            if (_cursor.Previous is ':')
            {
                return;
            }

            if (_cursor.Peek() is ')' or '}' or '@')
            {
                return;
            }
        }

        if (_cursor.Current is '{')
        {
            PushState(State.Selector);
        }
        else if (_cursor.Current is ';' or '}')
        {
            _states.Pop();
        }

        output.Push(_cursor.Current);
    }

    public void Minify(IOutput output)
    {
        while (_cursor.HasMoreInput)
        {
            if (char.IsWhiteSpace(_cursor.Current))
            {
                char? next = _cursor.Peek();
            
                if (next is null)
                {
                    _cursor.MoveNext();
                    continue;
                }
            
                if (char.IsWhiteSpace(_cursor.Previous) || char.IsWhiteSpace(next.Value))
                {
                    _cursor.MoveNext();
                    continue;
                }
            }

            State currentState = _states.Peek();

            if (currentState == State.None)
            {
                if (_cursor.Current is '.' or '#' || char.IsLetter(_cursor.Current))
                {
                    PushState(State.Selector);
                    continue;
                }
                
                if (_cursor.Current == '/' && _cursor.Peek() == '*')
                {
                    PushState(State.Comment);
                    continue;
                }

                if (_cursor.Current == '@')
                {
                    PushState(State.AtRule);
                    continue;
                }

                _cursor.MoveNext();

                continue;
            }

            
            if (currentState == State.Selector)
            {
                HandleSelector(output);
            }
            else if (currentState == State.Declaration)
            {
                HandleDeclaration(output);
            }
            else if (currentState == State.Comment)
            {
                HandleComment();
            }
            else if (currentState == State.AtRule)
            {
                HandleAtRule(output);
            }

            else if (!_cursor.HasMoreInput)
            {
                break;
            }

            _cursor.MoveNext();
        }
        output.Commit();
    }

    private static bool IsPreDelimiter(char value) =>
        PreDelimiters.Contains(value);

    private static bool IsPostDelimiter(char value) =>
        PostDelimiters.Contains(value);
}
