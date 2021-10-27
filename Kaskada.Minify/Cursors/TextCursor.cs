namespace Kaskada.Minify.Cursors;

internal sealed class TextCursor : ICursor
{
    private string _text;
    private int _position = -1;

    public TextCursor(
        string text) =>
        _text = text;

    public char Current { get; private set; }

    public char Previous { get; private set; } = '\0';

    public bool HasMoreInput { get; private set; } = true;


    public void Dispose() =>
        _text = string.Empty;

    public void MoveNext()
    {
        HasMoreInput = _position < _text.Length - 1;

        if (HasMoreInput)
        {
            Previous = Current;
            Current = _text[++_position];
        }
    }

    public char? Peek()
    {
        if (_position >= _text.Length - 2)
        {
            return null;
        }

        return _text[_position + 1];
    }
}
