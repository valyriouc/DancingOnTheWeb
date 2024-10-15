namespace Tooling.Markdown;

public class MarkdownException : Exception
{
    public MarkdownException()
    {
    }

    public MarkdownException(string message) : base(message)
    {
    }

    public MarkdownException(
        string message,
        Exception inner) : base(message, inner)
    {
    }
}