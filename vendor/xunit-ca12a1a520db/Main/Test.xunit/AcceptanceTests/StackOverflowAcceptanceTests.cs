using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestUtility;
using Xunit;
using System.Xml;

public class StackOverflowAcceptanceTests : AcceptanceTest
{
    [Fact(Skip = "Can't implement this until we run tests in a separate process")]
    public void AssertAreEqualTwoNumbersEqualShouldBePassedResult()
    {
        string code =
            @"
                using System;
                using Xunit;

                public class MockTestClass
                {
                    [Fact]
                    public void OverflowTest()
                    {
                        OverflowTest();
                    }
                }
            ";

        XmlNode assemblyNode = Execute(code);

        ResultXmlUtility.AssertResult(assemblyNode, "Fail", "MockTestClass.OverflowTest");
    }
}