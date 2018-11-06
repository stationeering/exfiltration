using System;
using System.Xml;
using Stationeering.Utils;
using Assets.Scripts.Objects.Motherboards;

namespace Stationeering.Task
{
    public class ExfilrateLogicTypes
    {
        public static void Exfiltrate()
        {
            var outputDocument = XML.PrepareXML("LogicTypes");
            var rootXML = outputDocument.DocumentElement;

            foreach (LogicType logicType in Enum.GetValues(typeof(LogicType)))
            {
                AddLogicType(logicType, outputDocument, rootXML);
            }

            outputDocument.Save("/tmp/logictype.xml");
        }

        private static void AddLogicType(LogicType logicType, XmlDocument xmlDocument, XmlNode root)
        {
            string logicTypeName = logicType.ToString();
            int logicTypeId = (int) logicType;

            var logicElement = xmlDocument.CreateElement("LogicType");

            var idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = logicTypeId.ToString();

            logicElement.Attributes.Append(idAttribute);

            var textNode = xmlDocument.CreateTextNode(logicTypeName);
            logicElement.AppendChild(textNode);

            root.AppendChild(logicElement);
        }
    }
}