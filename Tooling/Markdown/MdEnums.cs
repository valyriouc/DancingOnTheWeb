namespace Tooling.Markdown;


// Markdown syntax units:
// Block level: H1, H2, H3, Ul, Ol, Underline, Codeblock, Quotes, Image, Table, Horizontal, Paragraph
// Inline level: ListItem, Bold, CodeBlockItem, Text, Italic, Code, Link
public enum MdBlockType
{
    H1,
    H2,
    H3,
    Ul,
    Ol,
    Underline,
    Codeblock,
    Quotes,
    Image,
    Table,
    Horizontal,
    Paragraph
}

public enum MdUnitType
{
    ListItem,
    Bold,
    CodeBlockItem,
    Text,
    Italic,
    Code,
    Link
}