using System.Text;

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
                .ConsumeNewLine();
            
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
            
            span = span[index..].ConsumeNewLine();
            
            headers.Add(key.Trim(), value.Trim());
        }

        span = span.ConsumeNewLine();

        return span.Trim();
    }
    
    public static void ReadIntoBlocks(ReadOnlySpan<char> span, out List<(MdBlockType, string)> blocks)
    {
        blocks = new List<(MdBlockType, string)>();
        if (span.IsEmpty)
        {
            return;
        }

        MdBlockType mdType;
        string content = string.Empty;
        
        while (!span.IsEmpty)
        {
            span = span.SkipNonSenseCharacters();
            switch (span[0])
            {
                case '#':
                    span = ReadHeading(span, out mdType, out content);
                    break;
                default:
                    throw new MarkdownException("Unkown markdown block!");
                    break;
            }
            
            blocks.Add((mdType, content));
        }
    }

    private static ReadOnlySpan<char> ReadHeading(
        ReadOnlySpan<char> span, 
        out MdBlockType blockType,
        out string content)
    {
        int headingNumber = span.TakeWhile(c => c == '#').Length;
        if (headingNumber == 0)
        {
            throw new MarkdownException("Not a valid markdown heading.");
        }

        span = span[headingNumber..].ReadLine(out content);
        switch (headingNumber)
        {
            case 1:
                blockType = MdBlockType.H1;
                break;
            case 2:
                blockType = MdBlockType.H2;
                break;
            case 3:
                blockType = MdBlockType.H3;
                break;
            default:
                throw new MarkdownException("Only headings until level 3 are supported right now.");
        }

        return span;
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

    public static ReadOnlySpan<char> ConsumeNewLine(this ReadOnlySpan<char> self)
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

    public static ReadOnlySpan<char> ReadLine(this ReadOnlySpan<char> self, out string line)
    {
        line = string.Empty;
        
        if (self.IsEmpty)
        {
            return self;
        }

        StringBuilder sb = new();
        while (true)
        {
            if (self[0] == '\r' || self[0] == '\n' || self.IsEmpty)
            {
                break;
            }

            sb.Append(self[0]);
            self = self[1..];
        }

        line = sb.ToString();

        return self.ConsumeNewLine();
    }

    public static string TakeWhile(this ReadOnlySpan<char> self, Func<char, bool> predicate)
    {
        if (self.IsEmpty)
        {
            return string.Empty;
        }

        StringBuilder sb = new();
        while (true)
        {
            if (predicate(self[0])) 
            { 
                break;    
            }
            
            sb.Append(self[0]);
            self = self[1..];
        }

        return sb.ToString();
    }
}