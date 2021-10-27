using System.IO;
using System.Text;
using Kaskada.Minify.Outputs;
using Xunit;

namespace Kaskada.Minify.Tests.MinifierTests;

public class MinifyFromFileTests
{
    [Theory]
    [InlineData("test_01")]
    public void Test(string name)
    {
        string expected = File.ReadAllText($"TestData/{name}.minified.css");

        Minifier minifier = Minifier.FromFile($"TestData/{name}.css");

        using MemoryStream memoryStream = new();
        StreamOutput streamOutput = new(memoryStream);

        minifier.Minify(streamOutput);

        string actual = Encoding.UTF8.GetString(memoryStream.ToArray());

        Assert.Equal(expected, actual);
    }
}
