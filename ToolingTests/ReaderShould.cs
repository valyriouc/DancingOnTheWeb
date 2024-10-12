using Tooling.FileFormat;

namespace ToolingTests;

[TestFixture]
public class ReaderShould
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task ReadMetadataHeader()
    {
        /*
         * # ## ### #### 
         * ---
         * HEADER /HEADER
         * 
         */
        string content =
            """
            HEADER 
            title: Testing
            key: value
            /HEADER
            """;

        MemoryDocument document = await TwoStepReader.ReadFromStringAsync(content);
        
        Assert.That(document.Title, Is.EqualTo("Testing"));
    }
}