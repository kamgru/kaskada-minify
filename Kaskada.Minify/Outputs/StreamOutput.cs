namespace Kaskada.Minify.Outputs;

public sealed class StreamOutput : IOutput, IDisposable
{
    private readonly StreamWriter _streamWriter;

    public StreamOutput(
        Stream stream) =>
        _streamWriter = new StreamWriter(stream);

    public void Push(
        char value) =>
        _streamWriter.Write(value);

    public void Commit() =>
        _streamWriter.Flush();

    public void Dispose() =>
        _streamWriter.Dispose();
}
