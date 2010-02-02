using System;
using System.Collections.Generic;
using System.Reflection;

namespace Xunit.Sdk
{
    /// <summary>
    /// Implementation of <see cref="ITestCommand"/> which executes the
    /// <see cref="BeforeAfterTestAttribute"/> instances attached to a test method.
    /// </summary>
    public class BeforeAfterCommand : DelegatingTestCommand
    {
        MethodInfo testMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeAfterCommand"/> class.
        /// </summary>
        /// <param name="innerCommand">The inner command.</param>
        /// <param name="testMethod">The method.</param>
        public BeforeAfterCommand(ITestCommand innerCommand, MethodInfo testMethod)
            : base(innerCommand)
        {
            this.testMethod = testMethod;
        }

        /// <summary>
        /// Executes the test method.
        /// </summary>
        /// <param name="testClass">The instance of the test class</param>
        /// <returns>Returns information about the test run</returns>
        public override MethodResult Execute(object testClass)
        {
            List<BeforeAfterTestAttribute> beforeCalled = new List<BeforeAfterTestAttribute>();

            try
            {
                foreach (BeforeAfterTestAttribute attr in testMethod.GetCustomAttributes(typeof(BeforeAfterTestAttribute), true))
                {
                    attr.Before(testMethod);
                    beforeCalled.Add(attr);
                }

                return InnerCommand.Execute(testClass);
            }
            finally
            {
                List<Exception> exceptions = new List<Exception>();
                beforeCalled.Reverse();

                foreach (BeforeAfterTestAttribute attr in beforeCalled)
                    try
                    {
                        attr.After(testMethod);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }

                if (exceptions.Count > 0)
                    throw new AfterTestException(exceptions);
            }
        }
    }
}
