namespace Tooling.FileFormat;

public class Node
{
    public List<Part> Inner { get; } 
    public string Content { get; }

    private bool parsed = false;

    public Node(string content)
    {
        Inner = new();
        Content = content;
    }

    internal async Task ParseAsync()
    {
        if (parsed)
        {
            return;
        }

        
        parsed = true;
    }
}