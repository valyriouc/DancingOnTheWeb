namespace Tooling.FileFormat;

/// <summary>
/// Separation of the content blocks
/// Parsing inline stuff in each content block
/// </summary>
public sealed class TwoStepReader
{
    private ReadOnlyMemory<char> buffer;

    private List<Task<Node>> tasks;

    private CancellationToken cancellationToken;
    
    private TwoStepReader(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken)
    {
        this.buffer = buffer;
        this.tasks = new List<Task<Node>>();
        this.cancellationToken = cancellationToken;
    }

    private void Read()
    {
        ReadOnlySpan<char> span = buffer.Span;
        
    }
    
    // DIVIDER:
    // Textblöcke 
    // Überschriften 
    // Images/Videos
    // Codeblock 
    // Hervorhebung 
    // Metadaten 
    // Listen 
    // Unterstrich divider 
    // Tabellen 
    
    
    public static async Task<MemoryDocument> ReadFromFileAsync(
        string path, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }
        
        string content = await File.ReadAllTextAsync(path, cancellationToken);
        TwoStepReader reader = new(
            new Memory<char>(content.ToCharArray()),
            cancellationToken);

        reader.Read();
        Node[] nodes = await Task.WhenAll(reader.tasks);
        
        return new MemoryDocument(nodes);
    }

    public static async Task<MemoryDocument> ReadFromStringAsync(
        string content, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentNullException(nameof(content));
        }
        
        TwoStepReader reader = new(
            new Memory<char>(content.ToCharArray()), 
            cancellationToken);

        reader.Read();
        Node[] nodes = await Task.WhenAll(reader.tasks);

        return new MemoryDocument(nodes);
    }
}