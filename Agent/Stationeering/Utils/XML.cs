using System.Xml;

namespace Stationeering.Utils
{
    public class XML
    {
        public static XmlDocument PrepareXML(string rootName)
        {
            var xmlDocument = new XmlDocument();
            var root = xmlDocument.CreateElement(rootName);
            xmlDocument.AppendChild(root);

            // Create an XML declaration. 
            var declaration = xmlDocument.CreateXmlDeclaration("1.0", null, null);
            declaration.Encoding = "UTF-8";
            declaration.Standalone = "yes";

            // Add the new node to the document.
            xmlDocument.InsertBefore(declaration, root);

            return xmlDocument;
        }      
    }
}