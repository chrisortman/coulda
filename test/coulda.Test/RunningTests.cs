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
                ResultXmlUtility.AssertAttribute(resultNode, "name", "A simple example that does something interesting should be able to assert");
                ResultXmlUtility.AssertAttribute(resultNode, "type", "SimpleExample");
                ResultXmlUtility.AssertAttribute(resultNode, "method", "A_simple_example");
                ResultXmlUtility.AssertAttribute(resultNode, "result", "Pass");
            }
        }
    }
}