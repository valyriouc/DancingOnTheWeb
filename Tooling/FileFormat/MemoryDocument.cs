namespace Tooling.FileFormat;

public class MemoryDocument 
{
    public string Title { get; }

    public Dictionary<string, string> Headers { get; }

    private Node[] Nodes { get; }

    public MemoryDocument(Node[] nodes)
    {
        Nodes = nodes;
    }
}