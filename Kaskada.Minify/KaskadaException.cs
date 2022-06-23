namespace Kaskada.Minify;

[Serializable]
public class KaskadaException : Exception
{
    public KaskadaException()
    {
    }

    public KaskadaException(string message)
        : base(message)
    {
    }

    public KaskadaException(string message,
        Exception inner)
        : base(
            message,
            inner)
    {
    }

    protected KaskadaException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        : base(
            info,
            context)
    {
    }
}
