namespace Kaskada.Minify.Outputs;

public interface IOutput
{
    void Append(
        char value);

    void Commit();

    char? Peek();
}
