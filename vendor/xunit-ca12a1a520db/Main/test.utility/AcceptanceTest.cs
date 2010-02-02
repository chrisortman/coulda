using System.Xml;

namespace TestUtility
{
    public abstract class AcceptanceTest
    {
        public XmlNode Execute(string code)
        {
            return Execute(code, null);
        }

        public XmlNode Execute(string code,
                               string configFile,
                               params string[] references)
        {
            using (MockAssembly mockAssembly = new MockAssembly())
            {
                mockAssembly.Compile(code, references);
                return mockAssembly.Run(configFile);
            }
        }

        public XmlNode ExecuteWithReferences(string code,
                                             params string[] references)
        {
            return Execute(code, null, references);
        }
    }
}