namespace Tooling.Markdown;

public enum NewLine
{
    Linux,
    Windows,
    Mac
}

internal static class NewLineExtensions
{
    public static char AsNewLineChar(this NewLine self) => self switch
    {
        NewLine.Linux => '\n',
        NewLine.Windows => '\r',
        NewLine.Mac => '\r'
    };
}

public static class MarkdownBlockReader
{
    public static NewLine NewLine = NewLine.Windows;
    
    public static IEnumerable<(string, string)> ReadHeaders(ReadOnlySpan<char> span)
    {
        if (span.IsEmpty)
        {
            yield break;
        }

        span = span.SkipNonSenseCharacters();
        if (span[0..3] != "---")
        {
            yield break;
        }
        span = span[3..].ConsumeNewline();

        while (true)
        {
            span = span.SkipNonSenseCharacters();
            if (span[0..3] == "---")
            {
                break;
            }

            int index = span.IndexOf(':');
            string key = span[..index].ToString();

            index += 1;
            span = span[index..];

            span = span.SkipNonSenseCharacters();
            
            index = span.IndexOf(NewLine.AsNewLineChar());
            string value = span[..index].ToString();
            
            span = span.ConsumeNewline();

            yield return (key, value);
        }

        span = span.ConsumeNewline();
    }
    
    public static IEnumerable<(MdBlockType, string)> ReadIntoBlocks(ReadOnlySpan<char> span)
    {
        if (span.IsEmpty)
        {
            yield break;
        }
    }
}

internal static class SpanExtensions
{
    public static ReadOnlySpan<char> SkipNonSenseCharacters(this ReadOnlySpan<char> self)
    {
        if (self.IsEmpty)
        {
            return self;
        }

        while (true)
        {
            if (self[0] == ' ')
            {
                self = self[1..];
            }
            else
            {
                break;
            }
        }

        return self;
    }

    public static ReadOnlySpan<char> ConsumeNewline(this ReadOnlySpan<char> self)
    {
        if (self[0] == '\n' || self[0] == '\r')
        {
            self = self[1..];
            if (self[0] == '\n')
            {
                self = self[1..];
            }
        }

        return self;
    }
}