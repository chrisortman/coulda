using System.Xml;

namespace Xunit.Sdk
{
    /// <summary>
    /// XML utility methods
    /// </summary>
    public class XmlUtility
    {
        /// <summary>
        /// Adds an attribute to an XML node.
        /// </summary>
        /// <param name="node">The XML node.</param>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        public static void AddAttribute(XmlNode node, string name, object value)
        {
            XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
            attr.Value = value.ToString();
            node.Attributes.Append(attr);
        }

        /// <summary>
        /// Adds a child element to an XML node.
        /// </summary>
        /// <param name="parentNode">The parent XML node.</param>
        /// <param name="name">The child element name.</param>
        /// <returns>The new child XML element.</returns>
        public static XmlNode AddElement(XmlNode parentNode, string name)
        {
            XmlNode element = parentNode.OwnerDocument.CreateElement(name);
            parentNode.AppendChild(element);
            return element;
        }
    }
}
