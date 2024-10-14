using NuGet.Frameworks;
using Tooling.Markdown;

namespace ToolingTests;

public class MarkdowBlockReaderShould
{
    [Test]
    public void ReadsHeaderWithSingleLine()
    {
        string content =
            """
            ---
            key: value
            ---
            """;

        (string, string)[] result = MarkdownBlockReader.ReadHeaders(content).ToArray();

        Assert.Equals(1, result.Length);
        Assert.Equals(("key", "value"), result[0]);
    } 
}