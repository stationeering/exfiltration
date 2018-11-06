using System;
using System.Xml;
using Stationeering.Utils;
using Assets.Scripts.Objects.Motherboards;

namespace Stationeering.Task
{
    public class ExfilrateLogicSlotTypes
    {
        public static void Exfiltrate()
        {
            var outputDocument = XML.PrepareXML("LogicSlotTypes");
            var rootXML = outputDocument.DocumentElement;

            foreach (LogicSlotType logicSlotType in Enum.GetValues(typeof(LogicSlotType)))
            {
                AddLogicSlotType(logicSlotType, outputDocument, rootXML);
            }

            outputDocument.Save("/tmp/logicslottype.xml");
        }

        private static void AddLogicSlotType(LogicSlotType logicSlotType, XmlDocument xmlDocument, XmlNode root)
        {
            string logicTypeName = logicSlotType.ToString();
            int logicTypeId = (int) logicSlotType;

            var element = xmlDocument.CreateElement("LogicSlotType");

            var idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = logicTypeId.ToString();

            element.Attributes.Append(idAttribute);

            var textNode = xmlDocument.CreateTextNode(logicTypeName);
            element.AppendChild(textNode);

            root.AppendChild(element);
        }
    }
}