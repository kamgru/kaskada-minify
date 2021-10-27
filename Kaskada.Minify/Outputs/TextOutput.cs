namespace Kaskada.Minify.Outputs;

public sealed class TextOutput : IOutput
{
    private readonly List<char> _output = new();

    private string _text = string.Empty;

    public void Push(
        char value) =>
        _output.Add(value);

    public void Commit() =>
        _text = string.Join("", _output);

    public override string ToString() =>
        _text;
}
