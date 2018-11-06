using System;
using System.Xml;
using Assets.Scripts.Objects.Electrical;
using Stationeering.Utils;
using Assets.Scripts.Objects.Motherboards;

namespace Stationeering.Task
{
    public class ExfiltrateInstructions
    {
        public static void Exfiltrate()
        {
            var outputDocument = XML.PrepareXML("Instructions");
            var rootXML = outputDocument.DocumentElement;

            foreach (ScriptCommand scriptCommand in Enum.GetValues(typeof(ScriptCommand)))
            {
                AddScriptCommand(scriptCommand, outputDocument, rootXML);
            }

            outputDocument.Save("/tmp/scriptcommand.xml");
        }

        private static void AddScriptCommand(ScriptCommand scriptCommand, XmlDocument xmlDocument, XmlNode root)
        {
            var element = xmlDocument.CreateElement("Instruction");

            var instructionAttribute = xmlDocument.CreateAttribute("instruction");
            instructionAttribute.Value = scriptCommand.ToString();

            element.Attributes.Append(instructionAttribute);

            var description = ProgrammableChip.GetCommandDescription(scriptCommand);

            var textNode = xmlDocument.CreateTextNode(description);
            element.AppendChild(textNode);

            root.AppendChild(element);
        }
    }
}