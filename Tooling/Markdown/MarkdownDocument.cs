using System.Collections;

namespace Tooling.Markdown;

/// <summary>
/// Represents single markdown block element
/// </summary>
public class MarkdownBlock
{
    
}

/// <summary>
/// Represents a single markdown document
/// </summary>
public class MarkdownDocument : IEnumerable<MarkdownBlock>
{
    private readonly List<MarkdownBlock> blocks;

    public int BlockCount => blocks.Count;
    
    public Dictionary<string, string> Headers { get; }

    public MarkdownDocument()
    {
        this.blocks = new List<MarkdownBlock>();
        this.Headers = new Dictionary<string, string>();
    }

    public void Add(MarkdownBlock block)
    {
        if (block is null)
        {
            throw new ArgumentNullException(nameof(block));
        }    
        
        this.blocks.Add(block);
    }

    public void Remove(int index)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));   
        }
        
        this.blocks.RemoveAt((int)index);
    }
    
    public MarkdownBlock this[int index]
    {
        get
        {
            if (this.blocks.Count <= index)
            {
                throw new IndexOutOfRangeException($"No block at index {index}");
            }

            return this.blocks[index];
        }
        set
        {
            if (this.blocks.Count <= index)
            {
                throw new IndexOutOfRangeException($"No block at index {index}");
            }
        }
    }

    public string ToHtml()
    {
        
    }

    public string ToHtmlDocument()
    {
        
    }
    
    #region Enumerable
    
    public IEnumerator<MarkdownBlock> GetEnumerator() => 
        this.blocks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    #endregion
}