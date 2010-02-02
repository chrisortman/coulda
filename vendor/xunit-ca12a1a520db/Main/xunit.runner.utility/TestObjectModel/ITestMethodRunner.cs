using System.Collections.Generic;

namespace Xunit
{
    /// <summary>
    /// Represents the ability to run one or more test methods.
    /// </summary>
    public interface ITestMethodRunner
    {
        /// <summary>
        /// Runs the specified test methods.
        /// </summary>
        /// <param name="testMethods">The test methods to run.</param>
        /// <param name="callback">The run status information callback.</param>
        void Run(IEnumerable<TestMethod> testMethods, ITestMethodRunnerCallback callback);
    }
}