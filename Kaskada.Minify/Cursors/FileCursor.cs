namespace Kaskada.Minify.Cursors;

internal sealed class FileCursor : ICursor
{
    private readonly string _filename;
    private StreamReader? _streamReader;

    public FileCursor(string filename) =>
        _filename = filename;

    public char Current { get; private set; } = '\0';

    public char Previous { get; private set; } = '\0';

    public bool HasMoreInput { get; private set; } = true;

    public char? Peek()
    {
        int next = GetReader()
            .Peek();

        if (next is -1)
        {
            return null;
        }

        return (char)next;
    }

    public void MoveNext()
    {
        int current = GetReader()
            .Read();

        HasMoreInput = current is not -1;

        Previous = Current;
        Current = (char)current;
    }

    public void Dispose() =>
        _streamReader?.Dispose();

    private StreamReader GetReader()
    {
        if (_streamReader is not null)
        {
            return _streamReader;
        }

        _streamReader = File.OpenText(_filename);

        return _streamReader;
    }
}
