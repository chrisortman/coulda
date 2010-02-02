using System;
using System.IO;
using TestUtility;
using Xunit;
using Xunit.ConsoleClient;

public class CurrentDirectoryIntegrationTests
{
    [Fact]
    public void ConsoleRunnerPreservesCurrentDirectoryForXmlOutput()
    {
        string code = @"
                using System.IO;
                using Xunit;

                public class ChangeDirectoryTests
                {
                    [Fact]
                    public void ChangeDirectory()
                    {
                        Directory.SetCurrentDirectory(Path.GetTempPath());
                    }
                }
            ";

        using (MockAssembly mockAssembly = new MockAssembly())
        {
            mockAssembly.Compile(code);

            StringWriter writer = new StringWriter();

            string workingDirectory = Directory.GetCurrentDirectory();
            string xmlFilename = Path.GetFileName(mockAssembly.FileName) + ".xml";
            string fullPathName = Path.Combine(workingDirectory, xmlFilename);

            TextWriter oldOut = Console.Out;
            TextWriter oldError = Console.Error;

            try
            {
                Console.SetOut(writer);
                Console.SetError(writer);

                Program.Main(new[] { mockAssembly.FileName, "/xml", xmlFilename });
            }
            finally
            {
                Console.SetOut(oldOut);
                Console.SetError(oldError);
            }

            Assert.True(File.Exists(fullPathName));
            File.Delete(fullPathName);
        }
    }
}