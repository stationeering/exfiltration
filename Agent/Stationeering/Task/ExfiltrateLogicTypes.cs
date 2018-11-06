using System;
using System.Xml;
using Stationeering.Utils;
using Assets.Scripts.Objects.Motherboards;

namespace Stationeering.Task
{
    public class ExfiltrateLogicTypes
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

            var element = xmlDocument.CreateElement("LogicType");

            var idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = logicTypeId.ToString();

            element.Attributes.Append(idAttribute);

            var textNode = xmlDocument.CreateTextNode(logicTypeName);
            element.AppendChild(textNode);

            root.AppendChild(element);
        }
    }
}