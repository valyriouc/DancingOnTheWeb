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
    
    public static ReadOnlySpan<char> ReadHeaders(ReadOnlySpan<char> span, out Dictionary<string, string> headers)
    {
        headers = new Dictionary<string, string>();
        
        if (span.IsEmpty)
        {
            return ReadOnlySpan<char>.Empty;
        }
        
        if (span[0..3] is not "---")
        {
            return span[3..];
        }

        span = span[3..];
        
        while (true)
        {
            span = span
                .SkipNonSenseCharacters()
                .ConsumeNewline();
            
            if (span[0..3] is "---")
            {
                span = span[3..];
                break;
            }

            int index = span.IndexOf(':');
            string key = span[..index]
                .ThrowWhenMalformedMdHeader()
                .ToString();

            index += 1;
            span = span[index..].SkipNonSenseCharacters();
            
            index = span.IndexOf(NewLine.AsNewLineChar());
            string value = span[..index]
                .ThrowWhenMalformedMdHeader()
                .ToString();
            
            span = span[index..].ConsumeNewline();
            
            headers.Add(key.Trim(), value.Trim());
        }

        span = span.ConsumeNewline();

        return span.Trim();
    }
    
    // public static IEnumerable<(MdBlockType, string)> ReadIntoBlocks(ReadOnlySpan<char> span)
    // {
    //     if (span.IsEmpty)
    //     {
    //         yield break;
    //     }
    // }
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
        if (self.IsEmpty)
        {
            return self;
        }
        
        if (self[0] == '\n')
        {
            return self[1..];
        }
        
        if (self[0] == '\r')
        {
            self = self[1..];
            
            if (self[0] == '\n')
            {
                self = self[1..];
            }
        }

        return self;
    }

    public static ReadOnlySpan<char> ThrowWhenMalformedMdHeader(this ReadOnlySpan<char> self)
    {
        if (self.IsEmpty)
        {
            return self;
        }

        if (self.Contains('\n') || self.Contains('\r'))
        {
            throw new MarkdownException("Malformed markdown header. Expected format 'key: value'");
        }

        return self;
    }
}