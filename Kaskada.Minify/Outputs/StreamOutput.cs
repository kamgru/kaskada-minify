namespace Kaskada.Minify.Outputs;

public sealed class StreamOutput : IOutput, IDisposable
{
    private readonly StreamWriter _streamWriter;
    private char? _lastValue;

    public StreamOutput(
        Stream stream) =>
        _streamWriter = new StreamWriter(stream);

    public void Push(
        char value)
    {
        _streamWriter.Write(value);
        _lastValue = value;
    }

    public void Commit() =>
        _streamWriter.Flush();

    public char? Peek() => _lastValue;

    public void Dispose() =>
        _streamWriter.Dispose();
}
