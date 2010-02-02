using System;
using System.Collections.Generic;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// Represents a single class with test methods.
    /// </summary>
    public class TestClass : ITestMethodEnumerator, ITestMethodRunner
    {
        IEnumerable<TestMethod> testMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestClass"/> class.
        /// </summary>
        /// <param name="typeName">The namespace-qualified type name that
        /// this class represents.</param>
        /// <param name="testMethods">The test methods inside this test class.</param>
        public TestClass(string typeName, IEnumerable<TestMethod> testMethods)
        {
            Guard.ArgumentNotNull("testMethods", testMethods);

            TypeName = typeName;
            this.testMethods = testMethods;

            foreach (TestMethod testMethod in testMethods)
                testMethod.TestClass = this;
        }

        /// <summary>
        /// Gets the test assembly that this class belongs to.
        /// </summary>
        public TestAssembly TestAssembly { get; internal set; }

        /// <summary>
        /// Gets the namespace-qualified type name of this class.
        /// </summary>
        public string TypeName { get; private set; }

        /// <inheritdoc/>
        public IEnumerable<TestMethod> EnumerateTestMethods()
        {
            return EnumerateTestMethods(m => true);
        }

        /// <inheritdoc/>
        public IEnumerable<TestMethod> EnumerateTestMethods(Predicate<TestMethod> filter)
        {
            Guard.ArgumentNotNull("filter", filter);

            foreach (TestMethod testMethod in testMethods)
                if (filter(testMethod))
                    yield return testMethod;
        }

        internal TestMethod GetMethod(string methodName)
        {
            foreach (TestMethod testMethod in testMethods)
                if (testMethod.MethodName == methodName)
                    return testMethod;

            throw new InvalidOperationException("Got callback for test method " + methodName +
                                                " outside the scope of running class " + TypeName);
        }

        /// <summary>
        /// Gets the test runner used to run tests. Exists as an overload primarily
        /// for the purposes of unit testing.
        /// </summary>
        /// <param name="callback">The run status information callback.</param>
        protected virtual ITestRunner GetTestRunner(ITestMethodRunnerCallback callback)
        {
            return new TestRunner(TestAssembly.ExecutorWrapper,
                                  new TestClassCallbackDispatcher(this, callback));
        }

        /// <inheritdoc/>
        public virtual void Run(IEnumerable<TestMethod> testMethods, ITestMethodRunnerCallback callback)
        {
            Guard.ArgumentNotNullOrEmpty("testMethods", testMethods);
            Guard.ArgumentNotNull("callback", callback);

            List<string> methodNames = new List<string>();

            foreach (TestMethod testMethod in testMethods)
            {
                if (testMethod.TestClass != this)
                    throw new ArgumentException("All test methods must belong to this test class");

                methodNames.Add(testMethod.MethodName);
                testMethod.RunResults.Clear();
            }

            GetTestRunner(callback).RunTests(TypeName, methodNames);
        }
    }
}