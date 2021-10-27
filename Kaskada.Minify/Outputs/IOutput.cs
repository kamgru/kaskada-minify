namespace Kaskada.Minify.Outputs;

public interface IOutput
{
    void Push(
        char value);

    void Commit();
}
