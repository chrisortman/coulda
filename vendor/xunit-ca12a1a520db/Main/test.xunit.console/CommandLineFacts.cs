using System;
using System.Collections.Generic;
using Xunit;
using Xunit.ConsoleClient;

public class CommandLineFacts
{
    public class FilenameFacts
    {
        [Fact]
        public void AssemblyFileNameNotPresentThrows()
        {
            string[] arguments = new string[1];
            arguments[0] = "fileName";

            Exception exception = Record.Exception(() =>
                {
                    CommandLine.Parse(arguments);
                });

            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("file not found: fileName", exception.Message);
        }

        [Fact]
        public void AssemblyFilePresentDoesNotThrow()
        {
            string[] arguments = new[] { "assemblyName.dll" };

            Assert.DoesNotThrow(() =>
            {
                TestableCommandLine.Parse(arguments);
            });
        }

        [Fact]
        public void DllExistsConfigFileDoesNotExist()
        {
            string[] arguments = new[] { "assemblyName.dll", "badConfig.config" };

            Exception exception = Record.Exception(() => TestableCommandLine.Parse(arguments));

            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("config file not found: badConfig.config", exception.Message);
        }
    }

    public class InvalidOptionFacts
    {
        [Fact]
        public void OptionWithoutSlashThrows()
        {
            string[] arguments = new[] { "assembly.dll", "assembly.config", "teamcity" };

            Exception exception = Record.Exception(() => TestableCommandLine.Parse(arguments));

            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("unknown command line option: teamcity", exception.Message);
        }

        [Fact]
        public void SecondArgumentOptionWithoutSlashThrows()
        {
            string[] arguments = new[] { "assembly.xunit", "teamcity" };

            Exception exception = Record.Exception(() => TestableCommandLine.Parse(arguments));

            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("unknown command line option: teamcity", exception.Message);
        }
    }

    public class NoShadowOptionFacts
    {
        [Fact]
        public void NoShadowNotSetShadowCopyTrue()
        {
            string[] arguments = new[] { "assemblyName.dll" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            XunitProjectAssembly assembly = Assert.Single(commandLine.Project.Assemblies);
            Assert.True(assembly.ShadowCopy);
        }

        [Fact]
        public void NoShadowSetShadowCopyFalse()
        {
            string[] arguments = new[] { "assemblyName.dll", "/noshadow" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            XunitProjectAssembly assembly = Assert.Single(commandLine.Project.Assemblies);
            Assert.False(assembly.ShadowCopy);
        }
    }

    public class SilentOptionFacts
    {
        [Fact]
        public void SilentOptionNotPassedSilentFalse()
        {
            string[] arguments = new[] { "assemblyName.dll" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.False(commandLine.Silent);
        }

        [Fact]
        public void SilentOptionSilentIsTrue()
        {
            string[] arguments = new[] { "assemblyName.dll", "/silent" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.True(commandLine.Silent);
        }

        [Fact]
        public void SilentOptionIgnoreCaseSilentIsTrue()
        {
            string[] arguments = new[] { "assemblyName.dll", "/sIlEnT" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.True(commandLine.Silent);
        }
    }

    public class WaitOptionFacts
    {
        [Fact]
        public void WaitOptionNotPassedWaitFalse()
        {
            string[] arguments = new[] { "assemblyName.dll" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.False(commandLine.Wait);
        }

        [Fact]
        public void WaitOptionWaitIsTrue()
        {
            string[] arguments = new[] { "assemblyName.dll", "/wait" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.True(commandLine.Wait);
        }

        [Fact]
        public void WaitOptionIgnoreCaseWaitIsTrue()
        {
            string[] arguments = new[] { "assemblyName.dll", "/wAiT" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.True(commandLine.Wait);
        }
    }

    public class TeamCityArgumentFacts
    {
        [Fact, TeamCityEnvironmentRestore]
        public void TeamCityOptionNotPassedTeamCityFalse()
        {
            string[] arguments = new[] { "assemblyName.dll" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.False(commandLine.TeamCity);
        }

        [Fact, TeamCityEnvironmentRestore(Value = "TeamCity")]
        public void TeamCityOptionNotPassedEnvironmentSetTeamCityTrue()
        {
            string[] arguments = new[] { "assemblyName.dll" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.True(commandLine.TeamCity);
        }

        [Fact, TeamCityEnvironmentRestore]
        public void TeamCityOptionTeamCityTrue()
        {
            string[] arguments = new[] { "assemblyName.dll", "/teamcity" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.True(commandLine.TeamCity);
        }

        [Fact, TeamCityEnvironmentRestore]
        public void TeamCityOptionIgnoreCaseTeamCityTrue()
        {
            string[] arguments = new[] { "assemblyName.dll", "/tEaMcItY" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            Assert.True(commandLine.TeamCity);
        }

        class TeamCityEnvironmentRestore : BeforeAfterTestAttribute
        {
            string originalValue;

            public string Value { get; set; }

            public override void Before(System.Reflection.MethodInfo methodUnderTest)
            {
                originalValue = Environment.GetEnvironmentVariable("TEAMCITY_PROJECT_NAME");
                Environment.SetEnvironmentVariable("TEAMCITY_PROJECT_NAME", Value);
            }

            public override void After(System.Reflection.MethodInfo methodUnderTest)
            {
                Environment.SetEnvironmentVariable("TEAMCITY_PROJECT_NAME", originalValue);
            }
        }
    }

    public class IsProjectFilenameFacts
    {
        [Fact]
        public void IsProjectFileNameTrue()
        {
            string fileName = "xUnit.xUnit";

            bool isProjectFilename = CommandLine.IsProjectFilename(fileName);

            Assert.True(isProjectFilename);
        }

        [Fact]
        public void IsProjectFileNameTrueIgoresCase()
        {
            string fileName = "xUnit.XUNIT";

            bool isProjectFilename = CommandLine.IsProjectFilename(fileName);

            Assert.True(isProjectFilename);
        }

        [Fact]
        public void IsProjectFileNameFalse()
        {
            string fileName = "xUnit.sln";

            bool isProjectFilename = CommandLine.IsProjectFilename(fileName);

            Assert.False(isProjectFilename);
        }
    }

    public class TransformFacts
    {
        [Fact]
        public void OutputMissingFilename()
        {
            string[] arguments = new[] { "assemblyName.dll", "/xml" };

            Exception ex = Record.Exception(() => TestableCommandLine.Parse(arguments));

            Assert.IsType<ArgumentException>(ex);
            Assert.Equal("missing filename for /xml", ex.Message);
        }

        [Fact]
        public void OutputOnProjectFile()
        {
            string[] arguments = new[] { "assemblyName.xunit", "/xml", "foo.xml" };

            Exception ex = Record.Exception(() => TestableCommandLine.Parse(arguments));

            Assert.IsType<ArgumentException>(ex);
            Assert.Equal("the /xml command line option isn't valid for .xunit projects", ex.Message);
        }

        [Fact]
        public void OutputOnNonProjectFile()
        {
            string[] arguments = new[] { "assemblyName.dll", "/xml", "foo.xml" };

            TestableCommandLine commandLine = TestableCommandLine.Parse(arguments);

            XunitProjectAssembly assembly = Assert.Single(commandLine.Project.Assemblies);
            KeyValuePair<string, string> output = Assert.Single(assembly.Output);
            Assert.Equal("xml", output.Key);
            Assert.Equal("foo.xml", output.Value);
        }
    }

    class TestableCommandLine : CommandLine
    {
        private TestableCommandLine(string[] arguments)
            : base(arguments) { }

        public new static TestableCommandLine Parse(string[] arguments)
        {
            return new TestableCommandLine(arguments);
        }

        protected override XunitProject GetMultiAssemblyProject(string filename)
        {
            return new XunitProject();
        }

        protected override XunitProject Parse()
        {
            return Parse(filename => filename != "badConfig.config");
        }
    }
}
