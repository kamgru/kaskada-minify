namespace Kaskada.Minify.Extensions;

public static class MinifierExtensions
{
    public static string ToText(
        this Minifier minifier)
    {
        TextOutput textOutput = new();

        minifier.Minify(textOutput);

        return textOutput.ToString();
    }

    public static void ToFile(
        this Minifier minifier,
        string filename)
    {
        using FileStream fileStream = new(filename, FileMode.OpenOrCreate);

        StreamOutput streamOutput = new(fileStream);

        minifier.Minify(streamOutput);

        fileStream.Flush();
    }

    public static void ToStream(
        this Minifier minifier,
        Stream stream)
    {
        StreamOutput streamOutput = new(stream);

        minifier.Minify(streamOutput);
    }
}
