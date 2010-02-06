namespace Coulda.Test
{
    using System;
    using System.Linq.Expressions;
    using System.Xml;

    using TestUtility;

    using Xunit;

    public class TestHelper
    {
        /// <summary>
        /// Runs the specified test.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns>True to indicate success, false to indicate fail</returns>
        public static bool RunExampleTest(string className, string methodName)
        {
            using (MockAssembly assembly = new MockAssembly())
            {

                XmlNode lastNode = null;
                XmlNode returnValue = null;

                using (ExecutorWrapper wrapper = new ExecutorWrapper("Coulda.Examples.dll", null, false))
                {
                    returnValue = wrapper.RunTest(className, methodName, node =>
                    {
                        lastNode = node;
                        return true;
                    });
                }
                XmlNode resultNode = ResultXmlUtility.GetResult(lastNode);
                ResultXmlUtility.AssertAttribute(resultNode, "result", "Pass");
                if(resultNode.Attributes["result"].Value == "Pass")
                {
                    return true;
                }
               
            }
            return false;
        }

    }
}