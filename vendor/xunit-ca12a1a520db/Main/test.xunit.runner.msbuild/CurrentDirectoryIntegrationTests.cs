using System.IO;
using Microsoft.Build.Utilities;
using TestUtility;
using Xunit;
using Xunit.Runner.MSBuild;

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

            string workingDirectory = Directory.GetCurrentDirectory();
            string xmlFilename = Path.GetFileName(mockAssembly.FileName) + ".xml";
            string fullPathName = Path.Combine(workingDirectory, xmlFilename);
            xunit task = new xunit
                             {
                                 Assembly = new TaskItem(mockAssembly.FileName),
                                 Xml = new TaskItem(xmlFilename),
                                 BuildEngine = new StubBuildEngine()
                             };

            task.Execute();

            Assert.True(File.Exists(fullPathName));
            File.Delete(fullPathName);
        }
    }
}