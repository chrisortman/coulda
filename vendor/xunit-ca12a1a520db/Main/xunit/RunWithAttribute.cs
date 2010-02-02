using System;

namespace Xunit
{
    /// <summary>
    /// Attributes used to decorate a test fixture that is run with an alternate test runner.
    /// The test runner must implement the <see cref="Xunit.Sdk.ITestClassCommand"/> interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RunWithAttribute : Attribute
    {
        readonly Type commandType;

        /// <summary>
        /// Creates a new instance of the <see cref="RunWithAttribute"/> class.
        /// </summary>
        /// <param name="commandType">The class which implements ITestClassCommand and acts as the runner
        /// for the test fixture.</param>
        public RunWithAttribute(Type commandType)
        {
            this.commandType = commandType;
        }

        /// <summary>
        /// Gets the test class command.
        /// </summary>
        public Type TestClassCommand
        {
            get { return commandType; }
        }
    }
}