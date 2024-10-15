namespace Tooling.Markdown;

public class MarkdownUnit
{
    public MdUnitType UnitType { get; }
    
    public string Content { get; }
    
    public MarkdownUnit(MdUnitType unitType, string content)
    {
        UnitType = unitType;
        Content = content;
    }
}