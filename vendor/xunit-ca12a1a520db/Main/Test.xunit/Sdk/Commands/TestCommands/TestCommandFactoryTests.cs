using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

public class TestCommandFactoryTests
{
    [Fact]
    public void CallsTestClassCommandToGetTestCommandsAndWrapsTheminTimedCommands()
    {
        MethodInfo method = typeof(TestCommandFactoryTests).GetMethod("PublicTestMethod");
        List<ITestCommand> testCommands = new List<ITestCommand>();
        StubTestClassCommand classCommand = new StubTestClassCommand();
        testCommands.Add(new StubTestCommand());
        classCommand.EnumerateTestCommands__Result = testCommands;

        List<ITestCommand> result = new List<ITestCommand>(TestCommandFactory.Make(classCommand, Reflector.Wrap(method)));

        Assert.Same(method, classCommand.EnumerateTestCommands_TestMethod.MethodInfo);
        Assert.Equal(testCommands.Count, result.Count);
        var captureCommand = Assert.IsType<ExceptionAndOutputCaptureCommand>(result[0]);
        var timedCommand = Assert.IsType<TimedCommand>(captureCommand.InnerCommand);
        var lifetimeCommand = Assert.IsType<LifetimeCommand>(timedCommand.InnerCommand);
        var beforeAfterCommand = Assert.IsType<BeforeAfterCommand>(lifetimeCommand.InnerCommand);
        Assert.Same(testCommands[0], beforeAfterCommand.InnerCommand);
    }

    [Fact]
    public void DoesNotIncludeCreationCommandWhenTestCommandSaysNotToCreateInstance()
    {
        MethodInfo method = typeof(TestCommandFactoryTests).GetMethod("PublicTestMethod");
        List<ITestCommand> testCommands = new List<ITestCommand>();
        StubTestClassCommand classCommand = new StubTestClassCommand();
        StubTestCommand testCommand = new StubTestCommand();
        testCommand.ShouldCreateInstance__Result = false;
        testCommands.Add(testCommand);
        classCommand.EnumerateTestCommands__Result = testCommands;

        List<ITestCommand> result = new List<ITestCommand>(TestCommandFactory.Make(classCommand, Reflector.Wrap(method)));

        Assert.Same(method, classCommand.EnumerateTestCommands_TestMethod.MethodInfo);
        Assert.Equal(testCommands.Count, result.Count);
        var captureCommand = Assert.IsType<ExceptionAndOutputCaptureCommand>(result[0]);
        var timedCommand = Assert.IsType<TimedCommand>(captureCommand.InnerCommand);
        var beforeAfterCommand = Assert.IsType<BeforeAfterCommand>(timedCommand.InnerCommand);
        Assert.Same(testCommands[0], beforeAfterCommand.InnerCommand);
    }

    [Fact]
    public void IncludesTimeoutCommandWhenTestCommandSaysTheresATimeout()
    {
        MethodInfo method = typeof(TestCommandFactoryTests).GetMethod("PublicTestMethod");
        List<ITestCommand> testCommands = new List<ITestCommand>();
        StubTestClassCommand classCommand = new StubTestClassCommand();
        testCommands.Add(new StubTestCommand { Timeout__Result = 153 });
        classCommand.EnumerateTestCommands__Result = testCommands;

        List<ITestCommand> result = new List<ITestCommand>(TestCommandFactory.Make(classCommand, Reflector.Wrap(method)));

        Assert.Same(method, classCommand.EnumerateTestCommands_TestMethod.MethodInfo);
        Assert.Equal(testCommands.Count, result.Count);
        Assert.IsType<TimeoutCommand>(result[0]);
    }

    public void PublicTestMethod() { }
}