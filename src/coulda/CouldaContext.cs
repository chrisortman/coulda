namespace Coulda.Test
{
    using System;
    using System.Collections.Generic;

    using Xunit;
    using Xunit.Sdk;

    public class CouldaBase
    {
        public CouldaTestContext Context(string description, Action<CouldaTestContext> test)
        {
            return null;
        }
    }


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CouldaTestAttributeAttribute : FactAttribute
    {
        protected override IEnumerable<ITestCommand> EnumerateTestCommands(
            IMethodInfo method)
        {
            yield return new StubTestCommand(method, "none", 0);
        }
    }

    public class StubTestCommand : TestCommand
    {
        public StubTestCommand(IMethodInfo method, string displayName, int timeout) : base(method, displayName, timeout) {}

        public override MethodResult Execute(object testClass)
        {
            return new PassedResult("A_simple_example", "SimpleExample",
                                    "A simple example that does something interesting should be able to assert", null);
        }
    }

    public class CouldaTestContext
    {
        public void NoSetup() {}

        public void Should(string description, Action action) {}
    }
}