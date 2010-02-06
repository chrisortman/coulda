namespace Coulda.Test
{
    using System.IO;
    using System.Xml;

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
    }
}