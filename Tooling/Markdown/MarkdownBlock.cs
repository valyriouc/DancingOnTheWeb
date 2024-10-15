using System.Collections;

namespace Tooling.Markdown;

/// <summary>
/// Represents a single markdown block element
/// </summary>
public class MarkdownBlock : IEnumerable<MarkdownUnit>
{
    public MdBlockType Type { get; private set; }

    public int UnitCount => units.Count;
    

    private List<MarkdownUnit> units;

    public MarkdownBlock(MdBlockType type)
    {
        this.Type = type;
        this.units = new();
    }

    public MarkdownBlock(MdBlockType type, string content)
    {
        Type = type;
        this.units = new();
    }

    
    /// <summary>
    /// Returns an enumerator that iterates through the units of the markdown block.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the collection of Markdown units.
    /// </returns>
    public IEnumerator<MarkdownUnit> GetEnumerator() =>
        this.units.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}