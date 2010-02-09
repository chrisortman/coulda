namespace Coulda.Test
{
    using System.IO;
    using System.Xml;

    using Examples;

    using TestUtility;

    using Xunit;

    public class RunningTests
    {
        [Fact]
        public void Should_be_able_to_run_the_simple_example()
        {
           
            using (MockAssembly assembly = new MockAssembly())
            {

                XmlNode lastNode = null;
                XmlNode returnValue = null;

                using (ExecutorWrapper wrapper = new ExecutorWrapper("Coulda.Examples.dll", null, false))
                    returnValue = wrapper.RunClass("Coulda.Examples.SimpleExample", node => { lastNode = node; return true; });

                XmlNode resultNode = ResultXmlUtility.GetResult(lastNode);
                ResultXmlUtility.AssertAttribute(resultNode, "name", "My example test should be able to assert");
                ResultXmlUtility.AssertAttribute(resultNode, "type", "Coulda.Examples.SimpleExample");
                ResultXmlUtility.AssertAttribute(resultNode, "method", "A_simple_example");
                ResultXmlUtility.AssertAttribute(resultNode, "result", "Pass");
            }
        }

        [Fact]
        public void Should_be_able_to_have_a_failing_test()
        {

            using (MockAssembly assembly = new MockAssembly())
            {

                XmlNode lastNode = null;
                XmlNode returnValue = null;

                using (ExecutorWrapper wrapper = new ExecutorWrapper("Coulda.Examples.dll", null, false))
                    returnValue = wrapper.RunClass("Coulda.Examples.FailingExample", node => { lastNode = node; return true; });

                XmlNode resultNode = ResultXmlUtility.GetResult(lastNode);
                ResultXmlUtility.AssertAttribute(resultNode, "name", "A test with an assertion should be able to fail");
                ResultXmlUtility.AssertAttribute(resultNode, "type", "Coulda.Examples.FailingExample");
                ResultXmlUtility.AssertAttribute(resultNode, "method", "A_failing_example");
                ResultXmlUtility.AssertAttribute(resultNode, "result", "Fail");
            }
        }

        [Fact]
        public void Ensure_context_behaves_as_expected_wrt_scope()
        {
            var result = TestHelper.RunExampleTest("Coulda.Examples.ScopeContextExample", "Nested_context_scope");
            Assert.True(result);
        }

        [Fact]
        public void Should_be_able_to_run_some_setup_code()
        {
           // var result = TestHelper.RunExampleTest("Coulda.Examples.ScopeContextExample", "Context_with_setup");
            var result = TestHelper<ScopeContextExample>.RunExampleTest(x => x.Context_with_setup());
            Assert.True(result);
        }
        
        [Fact]
        public void Should_be_able_to_use_should_change()
        {
            //var result = TestHelper.RunExampleTest("Coulda.Examples.ShouldChangeExample", "Should_change_pass_test");
            var result =
                TestHelper<ShouldChangeExample>.RunExampleTest(x => x.Should_change_pass_test());
            Assert.True(result);
        }

        [Fact]
        public void Should_change_should_faild_if_from_is_wrong()
        {
            Assert.False(
                TestHelper<ShouldChangeExample>.RunExampleTest(x => x.Should_change_wrong_from_test()));
        }

        [Fact]
        public void Should_change_should_fail_if_to_is_wrong()
        {
            Assert.False(
                TestHelper<ShouldChangeExample>.RunExampleTest(x => x.Should_change_wrong_to_test()));
        }
    }
}