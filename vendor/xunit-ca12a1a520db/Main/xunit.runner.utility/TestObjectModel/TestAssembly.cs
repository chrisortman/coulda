using System;
using System.Collections.Generic;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// Represents a single test assembly with test classes.
    /// </summary>
    public class TestAssembly : ITestMethodEnumerator, ITestMethodRunner, IDisposable
    {
        IEnumerable<TestClass> testClasses;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAssembly"/> class.
        /// </summary>
        /// <param name="executorWrapper">The executor wrapper.</param>
        /// <param name="testClasses">The test classes.</param>
        public TestAssembly(IExecutorWrapper executorWrapper, IEnumerable<TestClass> testClasses)
        {
            Guard.ArgumentNotNull("executorWrapper", executorWrapper);
            Guard.ArgumentNotNull("testClasses", testClasses);

            ExecutorWrapper = executorWrapper;
            this.testClasses = testClasses;

            foreach (TestClass testClass in testClasses)
                testClass.TestAssembly = this;
        }

        /// <summary>
        /// Gets the assembly filename.
        /// </summary>
        public string AssemblyFilename
        {
            get { return ExecutorWrapper.AssemblyFilename; }
        }

        /// <summary>
        /// Gets the config filename.
        /// </summary>
        public string ConfigFilename
        {
            get { return ExecutorWrapper.ConfigFilename; }
        }

        /// <summary>
        /// Gets the executor wrapper.
        /// </summary>
        public IExecutorWrapper ExecutorWrapper { get; private set; }

        /// <summary>
        /// Gets the version of xunit.dll that the tests are linked against.
        /// </summary>
        public string XunitVersion
        {
            get { return ExecutorWrapper.XunitVersion; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            ExecutorWrapper.Dispose();
        }

        /// <summary>
        /// Enumerates the test classes in the assembly.
        /// </summary>
        public IEnumerable<TestClass> EnumerateClasses()
        {
            return testClasses;
        }

        /// <inheritdoc/>
        public IEnumerable<TestMethod> EnumerateTestMethods()
        {
            return EnumerateTestMethods(m => true);
        }

        /// <inheritdoc/>
        public IEnumerable<TestMethod> EnumerateTestMethods(Predicate<TestMethod> filter)
        {
            Guard.ArgumentNotNull("filter", filter);

            foreach (TestClass testClass in testClasses)
                foreach (TestMethod testMethod in testClass.EnumerateTestMethods(filter))
                    yield return testMethod;
        }

        /// <inheritdoc/>
        public virtual void Run(IEnumerable<TestMethod> testMethods, ITestMethodRunnerCallback callback)
        {
            Guard.ArgumentNotNullOrEmpty("testMethods", testMethods);
            Guard.ArgumentNotNull("callback", callback);

            var sortedMethods = new Dictionary<TestClass, List<TestMethod>>();

            foreach (TestClass testClass in testClasses)
                sortedMethods[testClass] = new List<TestMethod>();

            foreach (TestMethod testMethod in testMethods)
            {
                List<TestMethod> methodList = null;

                if (!sortedMethods.TryGetValue(testMethod.TestClass, out methodList))
                    throw new ArgumentException("Test method " + testMethod.MethodName +
                                                " on test class " + testMethod.TestClass.TypeName +
                                                " is not in this assembly", "testMethods");

                methodList.Add(testMethod);
            }

            callback.AssemblyStart(this);

            var callbackWrapper = new TestMethodRunnerCallbackWrapper(callback);

            foreach (var kvp in sortedMethods)
                if (kvp.Value.Count > 0)
                    kvp.Key.Run(kvp.Value, callbackWrapper);

            callback.AssemblyFinished(this, callbackWrapper.Total, callbackWrapper.Failed,
                                      callbackWrapper.Skipped, callbackWrapper.Time);
        }

        class TestMethodRunnerCallbackWrapper : ITestMethodRunnerCallback
        {
            public int Total = 0;
            public int Failed = 0;
            public int Skipped = 0;
            public double Time = 0.0;

            ITestMethodRunnerCallback innerCallback;

            public TestMethodRunnerCallbackWrapper(ITestMethodRunnerCallback innerCallback)
            {
                this.innerCallback = innerCallback;
            }

            public void AssemblyFinished(TestAssembly testAssembly, int total, int failed, int skipped, double time)
            {
                throw new NotImplementedException();
            }

            public void AssemblyStart(TestAssembly testAssembly)
            {
                throw new NotImplementedException();
            }

            public bool ClassFailed(TestClass testClass, string exceptionType, string message, string stackTrace)
            {
                return innerCallback.ClassFailed(testClass, exceptionType, message, stackTrace);
            }

            public void ExceptionThrown(TestAssembly testAssembly, Exception exception)
            {
                innerCallback.ExceptionThrown(testAssembly, exception);
            }

            public bool TestFinished(TestMethod testMethod)
            {
                ++Total;

                var lastRunResult = testMethod.RunResults[testMethod.RunResults.Count - 1];

                if (lastRunResult is TestFailedResult)
                    ++Failed;
                if (lastRunResult is TestSkippedResult)
                    ++Skipped;

                Time += lastRunResult.Duration;

                return innerCallback.TestFinished(testMethod);
            }

            public bool TestStart(TestMethod testMethod)
            {
                return innerCallback.TestStart(testMethod);
            }
        }
    }
}