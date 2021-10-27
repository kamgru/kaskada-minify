namespace Kaskada.Minify.Cursors;

internal interface ICursor : IDisposable
{
    char Current { get; }
    char Previous { get; }
    bool HasMoreInput { get; }
    char? Peek();
    void MoveNext();
}
