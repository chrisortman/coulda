namespace Coulda.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using Xunit;
    using Xunit.Sdk;

    public class CouldaBase
    {
        public CouldaTestContext Context(string description, Action<CouldaTestContext> test)
        {
            var context = new CouldaTestContext(description);
            test(context);
            return context;
        }
    }


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CouldaTestAttributeAttribute : FactAttribute
    {
        protected override IEnumerable<ITestCommand> EnumerateTestCommands(
            IMethodInfo method)
        {
            var testObject = method.CreateInstance();
            var testContext = (CouldaTestContext) method.MethodInfo.Invoke(testObject, null);
            var testDescriptions = testContext.GetTestDescriptions();

            foreach(var td in testDescriptions)
            {
                yield return new CouldaTestCommand(method, td, 0);
            }
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

    public class CouldaTestCommand : TestCommand
    {
        private IMethodInfo _method;

        public CouldaTestCommand(IMethodInfo method, string displayName, int timeout) : base(method, displayName, timeout)
        {
            _method = method;
        }

        public override MethodResult Execute(object testClass)
        {
            var contextObject = (CouldaTestContext) _method.MethodInfo.Invoke(testClass, null);
            var test = contextObject.GetTestByDescription(DisplayName);
            test.Should();

            return new PassedResult(_method, DisplayName);
        }
    }

    public class ShouldContext
    {
        public string Description { get; set; }
        public Action Should { get; set; }
    }
    public class CouldaTestContext
    {
        private string _description;
        private IMethodInfo _method;
        private Queue<ShouldContext> _shoulds;

        public CouldaTestContext(string description)
        {
            _description = description;
            _shoulds = new Queue<ShouldContext>();
        }

        //METHODS MENT FOR USER
        public void NoSetup() {}

        public void Should(string description, Action action)
        {
            _shoulds.Enqueue(new ShouldContext() { Description = description, Should = action});
        }

        //----------
        internal void SetMethodInfo(IMethodInfo methodInfo)
        {
            _method = methodInfo;
        }

        public IEnumerable<string> GetTestDescriptions()
        {
            return _shoulds.ToArray().Select(x => String.Format("{0} should {1}",_description, x.Description));
        }


        public ShouldContext GetTestByDescription(string description)
        {
            var lookup = _shoulds.ToDictionary(k => String.Format("{0} should {1}", _description, k.Description));
            return lookup[description];
        }
    }
}