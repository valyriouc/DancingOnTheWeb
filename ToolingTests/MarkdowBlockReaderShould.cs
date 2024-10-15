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

        ReadOnlySpan<char> result = MarkdownBlockReader.ReadHeaders(
            content.ToCharArray(), 
            out Dictionary<string, string> headers);
        
        Assert.That(result.Length, Is.EqualTo(0));
        Assert.That(headers.Count, Is.EqualTo(1));
        
        var header = headers.First();
        Assert.That(header.Key, Is.EqualTo("key"));
        Assert.That(header.Value, Is.EqualTo("value"));
    } 
    
    [Test]
    public void ReadsHeaderWithSingleLineExcludingWhitespaces()
    {
        string content =
            """
            ---
                key   :      value
            ---
            """;

        ReadOnlySpan<char> result = MarkdownBlockReader.ReadHeaders(
            content.ToCharArray(), 
            out Dictionary<string, string> headers);
        
        Assert.That(result.Length, Is.EqualTo(0));
        Assert.That(headers.Count, Is.EqualTo(1));
        
        var header = headers.First();
        Assert.That(header.Key, Is.EqualTo("key"));
        Assert.That(header.Value, Is.EqualTo("value"));
    } 
    
    [Test]
    public void ReadsHeaderWithSingleLineWhenEndsWithNewLine()
    {
        string content =
            """
            ---
            key: value
            ---
            
            """;

        ReadOnlySpan<char> result = MarkdownBlockReader.ReadHeaders(
            content.ToCharArray(), 
            out Dictionary<string, string> headers);
        
        Assert.That(result.Length, Is.EqualTo(0));
        Assert.That(headers.Count, Is.EqualTo(1));
        
        var header = headers.First();
        Assert.That(header.Key, Is.EqualTo("key"));
        Assert.That(header.Value, Is.EqualTo("value"));
    } 
    
    [Test]
    public void BreaksReadingWhenMalformedHeader()
    {
        string content =
            """
            ---
            key
            value:
            ---

            """;
        
        Assert.Throws<MarkdownException>(() => MarkdownBlockReader.ReadHeaders(
            content.ToCharArray(), 
            out Dictionary<string, string> headers));
    } 
    
    [Test]
    public void ReadsHeaderWithSingleLineWhenEndsWithEmptyLineBetween()
    {
        string content =
            """
            ---
            key: value
            
            hello: world
            ---

            """;

        ReadOnlySpan<char> result = MarkdownBlockReader.ReadHeaders(
            content.ToCharArray(), 
            out Dictionary<string, string> headers);
        
        Assert.That(result.Length, Is.EqualTo(0));
        Assert.That(headers.Count, Is.EqualTo(2));
        
        var header = headers.First();
        Assert.That(header.Key, Is.EqualTo("key"));
        Assert.That(header.Value, Is.EqualTo("value"));
    } 
    
    [Test]
    public void ReadsHeaderWithTwoLines()
    {
        string content =
            """
            ---
            key: value
            nice: value
            ---

            """;

        ReadOnlySpan<char> result = MarkdownBlockReader.ReadHeaders(
            content.ToCharArray(), 
            out Dictionary<string, string> headers);
        
        Assert.That(result.Length, Is.EqualTo(0));
        Assert.That(headers.Count, Is.EqualTo(2));

        var header = headers.First();
        Assert.That(header.Key, Is.EqualTo("key"));
        Assert.That(header.Value, Is.EqualTo("value"));

        header = headers.Skip(1).First();
        Assert.That(header.Key, Is.EqualTo("nice"));
        Assert.That(header.Value, Is.EqualTo("value"));
    } 
    
    [Test]
    public void ReadsHeaderWithThreeLines()
    {
        string content =
            """
            ---
            key: value
            nice: value
            hello: world
            ---

            """;

        ReadOnlySpan<char> result = MarkdownBlockReader.ReadHeaders(
            content.ToCharArray(), 
            out Dictionary<string, string> headers);
        
        Assert.That(result.Length, Is.EqualTo(0));
        Assert.That(headers.Count, Is.EqualTo(3));

        var header = headers.First();
        Assert.That(header.Key, Is.EqualTo("key"));
        Assert.That(header.Value, Is.EqualTo("value"));

        header = headers.Skip(1).First();
        Assert.That(header.Key, Is.EqualTo("nice"));
        Assert.That(header.Value, Is.EqualTo("value"));
        
        header = headers.Skip(2).First();
        Assert.That(header.Key, Is.EqualTo("hello"));
        Assert.That(header.Value, Is.EqualTo("world"));
    }
    
}