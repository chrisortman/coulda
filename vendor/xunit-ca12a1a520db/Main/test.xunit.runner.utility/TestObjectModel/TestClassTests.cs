using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

public class TestClassTests
{
    public class Constructor
    {
        [Fact]
        public void NullTestMethodListThrows()
        {
            Exception ex = Record.Exception(() => new TestClass("typeName", null));

            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void SetsTestClassOnTestMethods()
        {
            TestMethod testMethod = new TestMethod(null, null, null);

            var testClass = new TestClass("typeName", new[] { testMethod });

            Assert.Same(testClass, testMethod.TestClass);
        }
    }

    public class EnumerateTestMethods
    {
        [Fact]
        public void Unfiltered()
        {
            TestMethod[] tests = new[]
            {
                new TestMethod("method1", null, null),
                new TestMethod("method2", null, null),
                new TestMethod("method3", null, null)
            };
            TestClass testClass = new TestClass("foo", tests);

            var results = testClass.EnumerateTestMethods();

            Assert.Contains(tests[0], results);
            Assert.Contains(tests[1], results);
            Assert.Contains(tests[2], results);
        }

        [Fact]
        public void NullFilterThrows()
        {
            TestClass testClass = new TestClass("foo", new TestMethod[0]);

            Exception ex = Record.Exception(() => testClass.EnumerateTestMethods(null).ToList());

            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void FilterWithTruePredicate()
        {
            TestMethod[] tests = new[]
            {
                new TestMethod("method1", null, null),
                new TestMethod("method2", null, null),
                new TestMethod("method3", null, null)
            };
            TestClass testClass = new TestClass("foo", tests);

            var results = testClass.EnumerateTestMethods(testMethod => true);

            Assert.Contains(tests[0], results);
            Assert.Contains(tests[1], results);
            Assert.Contains(tests[2], results);
        }

        [Fact]
        public void FilterWithFalsePredicate()
        {
            TestMethod[] tests = new[]
            {
                new TestMethod("method1", null, null),
                new TestMethod("method2", null, null),
                new TestMethod("method3", null, null)
            };
            TestClass testClass = new TestClass("foo", tests);

            var results = testClass.EnumerateTestMethods(testMethod => false);

            Assert.Empty(results);
        }
    }

    public class Run
    {
        [Fact]
        public void NullTestMethodsThrows()
        {
            var callback = new Mock<ITestMethodRunnerCallback>();
            var testClass = new TestClass("typeName", new TestMethod[0]);

            Assert.Throws<ArgumentNullException>(() => testClass.Run(null, callback.Object));
        }

        [Fact]
        public void EmptyTestMethodsThrows()
        {
            var callback = new Mock<ITestMethodRunnerCallback>();
            var testClass = new TestClass("typeName", new TestMethod[0]);

            Assert.Throws<ArgumentException>(() => testClass.Run(new TestMethod[0], callback.Object));
        }

        [Fact]
        public void NullCallbackThrows()
        {
            var testMethod = new TestMethod(null, null, null);
            var testClass = new TestClass("typeName", new[] { testMethod });

            Assert.Throws<ArgumentNullException>(() => testClass.Run(new[] { testMethod }, null));
        }

        [Fact]
        public void TestMethodNotForThisTestClassThrows()
        {
            var callback = new Mock<ITestMethodRunnerCallback>();
            var testMethod = new TestMethod(null, null, null);
            var testClass = new TestClass("typeName", new TestMethod[0]);

            Assert.Throws<ArgumentException>(() => testClass.Run(new[] { testMethod }, callback.Object));
        }

        [Fact]
        public void CallsTestRunnerWithTestList()
        {
            var wrapper = new Mock<IExecutorWrapper>();
            var callback = new Mock<ITestMethodRunnerCallback>();
            var testMethod1 = new TestMethod("testMethod1", null, null);
            var testMethod2 = new TestMethod("testMethod2", null, null);
            var testMethod3 = new TestMethod("testMethod3", null, null);
            var testClass = new TestableTestClass("typeName", new[] { testMethod1, testMethod2, testMethod3 });
            var testAssembly = new TestAssembly(wrapper.Object, new[] { testClass });

            testClass.Run(new[] { testMethod1, testMethod2, testMethod3 }, callback.Object);

            testClass.TestRunner.Verify(r => r.RunTests("typeName", new List<string> { "testMethod1", "testMethod2", "testMethod3" }));
        }

        [Fact]
        public void EmptiesResultListBeforeRunning()
        {
            var wrapper = new Mock<IExecutorWrapper>();
            var callback = new Mock<ITestMethodRunnerCallback>();
            var testMethod = new TestMethod("testMethod", null, null);
            var testClass = new TestableTestClass("typeName", new[] { testMethod });
            var testAssembly = new TestAssembly(wrapper.Object, new[] { testClass });
            testMethod.RunResults.Add(new TestPassedResult(1.23, "displayName", null));

            testClass.Run(new[] { testMethod }, callback.Object);

            Assert.Empty(testMethod.RunResults);
        }
    }

    class TestableTestClass : TestClass
    {
        public Mock<ITestRunner> TestRunner;

        public TestableTestClass(string typeName, IEnumerable<TestMethod> testMethods)
            : base(typeName, testMethods)
        {
            TestRunner = new Mock<ITestRunner>();
        }

        protected override ITestRunner GetTestRunner(ITestMethodRunnerCallback callback)
        {
            return TestRunner.Object;
        }
    }
}