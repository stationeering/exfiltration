using System;
using System.Xml;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using Assets.Scripts.Util;
using Reagents;
using Stationeering.Utils;

namespace Stationeering.Task
{
    public class ExfiltrateThings
    {
        public static void Exfiltrate()
        {
            var outputDocument = XML.PrepareXML("Things");
            var rootXML = outputDocument.DocumentElement;

            foreach (Thing thing in Thing.AllPrefabs)
            {
                AddThing(thing, outputDocument, rootXML);
            }           

            outputDocument.Save("/tmp/thing.xml");
        }

        private static void AddThing(Thing thing, XmlDocument xmlDocument, XmlNode root)
        {
            var element = xmlDocument.CreateElement("Thing");
            
            var prefabAttribute = xmlDocument.CreateAttribute("prefab");
            prefabAttribute.Value = thing.PrefabName;
            element.Attributes.Append(prefabAttribute);

            var prefabHashAttribute = xmlDocument.CreateAttribute("prefabHash");
            prefabHashAttribute.Value = thing.PrefabHash.ToString();
            element.Attributes.Append(prefabHashAttribute);
            
            var displayAttribute = xmlDocument.CreateAttribute("display");
            displayAttribute.Value = thing.DisplayName;
            element.Attributes.Append(displayAttribute);
            
            root.AppendChild(element);
            
            AddQuantity(thing, xmlDocument, element);
            AddNutrition(thing, xmlDocument, element);
            AddMeltable(thing, xmlDocument, element);
            AddPaintable(thing, xmlDocument, element);
            AddTemperatureLimits(thing, xmlDocument, element);

            AddCreatedReagent(thing, xmlDocument, element);
            AddCreatedGases(thing, xmlDocument, element);
            
            AddModeStrings(thing, xmlDocument, element);
            AddSlots(thing, xmlDocument, element);
            AddLogicTypes(thing, xmlDocument, element);
            
            AddConstructedBy(thing, xmlDocument, element);
            
			AddObjectHierachy(thing, xmlDocument, element);

            /*                
                AddConstructs(thing, ref text3);
				AddMadeBy(thing, ref text3);
				AddCreates(thing, ref text3);
             */
        }

        private static void AddQuantity(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            if (thing is IQuantity quantity)
            {
                var quantityElement = xmlDocument.CreateElement("Quantity");
                
                var maxStackAttribute = xmlDocument.CreateAttribute("stackSize");
                maxStackAttribute.Value = quantity.GetMaxQuantity.ToString();

                quantityElement.Attributes.Append(maxStackAttribute);

                parent.AppendChild(quantityElement);
            }
        }
        
        private static void AddNutrition(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {           
            if (thing is INutrition nutrition)
            {
                var perValue = nutrition.Nutrition(1F);
                
                if (perValue > 0f)
                {
                    var nutritionElement = xmlDocument.CreateElement("Nutrition");
                    
                    var maxQuantityAttribute = xmlDocument.CreateAttribute("perQuantity");
                    maxQuantityAttribute.Value = perValue.ToString();

                    nutritionElement.Attributes.Append(maxQuantityAttribute);
                    
                    parent.AppendChild(nutritionElement);
                }
            }
        }
        
        
        private static void AddMeltable(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            if (thing is Ice ice)
            {
                var meltableElement = xmlDocument.CreateElement("Meltable");
                    
                var pressureAttribute = xmlDocument.CreateAttribute("pressure");               
                pressureAttribute.Value = (ice.MeltPressure * RocketMath.Thousand).ToString();
                meltableElement.Attributes.Append(pressureAttribute);

                var temperatureAttribute = xmlDocument.CreateAttribute("temperature");               
                temperatureAttribute.Value = ice.MeltTemperature.ToString();
                meltableElement.Attributes.Append(temperatureAttribute);
                
                parent.AppendChild(meltableElement);
            }
        }
        
        private static void AddPaintable(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
                var paintableElement = xmlDocument.CreateElement("Paintable");
                    
                var canBeAttribute = xmlDocument.CreateAttribute("canBe");               
                canBeAttribute.Value = (thing.PaintableMaterial != null).ToString().ToLower();
                paintableElement.Attributes.Append(canBeAttribute);
                
                parent.AppendChild(paintableElement);
        }
        
        private static void AddTemperatureLimits(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            var temperatureLimits = xmlDocument.CreateElement("TemperatureLimits");                    
            
            var shatterAttribute = xmlDocument.CreateAttribute("shatter");               
            shatterAttribute.Value = thing.ShatterTemperature.ToString();
            temperatureLimits.Attributes.Append(shatterAttribute);

            if (thing.FlashpointTemperature > 0)
            {
                var flashpointAttribute = xmlDocument.CreateAttribute("flashpoint");
                flashpointAttribute.Value = thing.FlashpointTemperature.ToString();
                temperatureLimits.Attributes.Append(flashpointAttribute);
            }

            if (thing.AutoignitionTemperature > 0)
            {
                var autoignitionAttribute = xmlDocument.CreateAttribute("autoignition");
                autoignitionAttribute.Value = thing.AutoignitionTemperature.ToString();
                temperatureLimits.Attributes.Append(autoignitionAttribute);
            }
            
            parent.AppendChild(temperatureLimits);
        }
        
        private static void AddLogicTypes(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            if (thing is ILogicable logicable)
            {
                var logicElement = xmlDocument.CreateElement("LogicTypes");

                foreach (LogicType logicType in Enum.GetValues(typeof(LogicType)))
                {
                    Boolean canRead = logicable.CanLogicRead(logicType);
                    Boolean canWrite = logicable.CanLogicWrite(logicType);

                    if (canRead || canWrite)
                    {
                        var logicTypeElement = xmlDocument.CreateElement("LogicType");

                        var readAttribute = xmlDocument.CreateAttribute("read");
                        readAttribute.Value = canRead.ToString().ToLower();
                        logicTypeElement.Attributes.Append(readAttribute);

                        var writeAttribute = xmlDocument.CreateAttribute("write");
                        writeAttribute.Value = canWrite.ToString().ToLower();
                        logicTypeElement.Attributes.Append(writeAttribute);

                        var nameNode = xmlDocument.CreateTextNode(logicType.ToString());
                        logicTypeElement.AppendChild(nameNode);
                        
                        logicElement.AppendChild(logicTypeElement);
                    }
                }                
                
                parent.AppendChild(logicElement);
            }
        }
        
        private static void AddSlots(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            if (thing.HasAnySlots)
            {
                var slotsElement = xmlDocument.CreateElement("Slots");
    
                for (int i = 0; i < thing.Slots.Count; i++)
                {
                    Slot slot = thing.Slots[i];
                    String name = string.IsNullOrEmpty(slot.DisplayName) ? ("Slot " + i) : slot.DisplayName;

                    var slotElement = xmlDocument.CreateElement("Slot");

                    var indexAttribute = xmlDocument.CreateAttribute("index");
                    indexAttribute.Value = i.ToString();
                    slotElement.Attributes.Append(indexAttribute);

                    var nameNode = xmlDocument.CreateTextNode(name);
                    slotElement.AppendChild(nameNode);                       

                    slotsElement.AppendChild(slotElement);
                }              
                
                parent.AppendChild(slotsElement);
            }            
        }

        private static void AddCreatedReagent(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            if (thing is Item item)
            {
                if (item.CreatedReagentMixture.TotalReagents > 0)
                {
                    var createdReagentsElement = xmlDocument.CreateElement("CreatedReagents");
                    
                    foreach (Reagent reagent in Reagent.AllReagents)
                    {
                        var reagentQuantity = item.CreatedReagentMixture.Get(reagent);

                        if (reagentQuantity > 0)
                        {
                            var singleReagent = xmlDocument.CreateElement("Reagent");
                            
                            var quantityAttribute = xmlDocument.CreateAttribute("quantity");
                            quantityAttribute.Value = reagentQuantity.ToString();
                            singleReagent.Attributes.Append(quantityAttribute);
                            
                            var nameText = xmlDocument.CreateTextNode(reagent.DisplayName);
                            singleReagent.AppendChild(nameText);

                            createdReagentsElement.AppendChild(singleReagent);
                        }
                    }

                    parent.AppendChild(createdReagentsElement);
                }
            }
        }
        
        private static void AddCreatedGases(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            if (thing is Ore ore)
            {
                if (ore.SpawnContents.Count > 0)
                {
                    var createdGasesElement = xmlDocument.CreateElement("CreatedGases");

                    foreach (SpawnGas spawnGas in ore.SpawnContents)
                    {
                        if (spawnGas.Quantity > 0)
                        {
                            var createdGasElement = xmlDocument.CreateElement("Gas");

                            var quantityAttribute = xmlDocument.CreateAttribute("quantity");
                            quantityAttribute.Value = spawnGas.Quantity.ToString();
                            createdGasElement.Attributes.Append(quantityAttribute);

                            var nameText = xmlDocument.CreateTextNode(spawnGas.Name);
                            createdGasElement.AppendChild(nameText);

                            createdGasesElement.AppendChild(createdGasElement);
                        }
                    }

                    if (parent.HasChildNodes)
                    {
                        parent.AppendChild(createdGasesElement);
                    }
                }                
            }
        }
        
        private static void AddModeStrings(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            if (thing.HasModeState && thing.ModeStrings != null)
            {
                var modesElement = xmlDocument.CreateElement("Modes");

                foreach (String mode in thing.ModeStrings)
                {
                    var modeElement = xmlDocument.CreateElement("Mode");
                    modeElement.AppendChild(xmlDocument.CreateTextNode(mode));
                    modesElement.AppendChild(modeElement);
                }

                parent.AppendChild(modesElement);
            }
        }

        private static void AddConstructedBy(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            if (thing is IConstructionKit constructionKit)
            {
                var constructedPrefabs = constructionKit.GetConstructedPrefabs();

                if (constructedPrefabs != null)
                {
                    var constructs = xmlDocument.CreateElement("Constructs");

                    foreach (Thing constructedThing in constructedPrefabs)
                    {
                        if (constructedThing != null)
                        {
                            var thingElement = xmlDocument.CreateElement("Thing");
                            var nameAttribute = xmlDocument.CreateAttribute("prefab");
                            nameAttribute.Value = constructedThing.PrefabName;
                            thingElement.Attributes.Append(nameAttribute);

                            constructs.AppendChild(thingElement);
                        }
                    }

                    if (constructs.HasChildNodes)
                    {
                        parent.AppendChild(constructs);
                    }
                }
            }            
        }

		private static void AddObjectHierachy(Thing thing, XmlDocument xmlDocument, XmlNode parent)
        {
            var hierachy = xmlDocument.CreateElement("CSharpHeirachy");

            Type type = thing.GetType();

            AddObjectHierachyChild(type, xmlDocument, hierachy);

            parent.AppendChild(hierachy);
		}

        private static void AddObjectHierachyChild(Type type, XmlDocument xmlDocument, XmlNode parent)
        {
            var typeElement = xmlDocument.CreateElement("Type");

            var nameAttribute = xmlDocument.CreateAttribute("name");
            nameAttribute.Value = type.FullName;
            typeElement.Attributes.Append(nameAttribute);

            if (type.GetInterfaces().Length > 0) {
                var interfacesElement = xmlDocument.CreateElement("Interfaces");

                foreach (Type intType in type.GetInterfaces()) {
                    if (!intType.FullName.StartsWith("System.IComparable", StringComparison.CurrentCulture))
                    {
                        var interfaceElement = xmlDocument.CreateElement("Interface");

                        var interfaceNameAttribute = xmlDocument.CreateAttribute("name");
                        interfaceNameAttribute.Value = intType.FullName;
                        interfaceElement.Attributes.Append(interfaceNameAttribute);

                        interfacesElement.AppendChild(interfaceElement);
                    }
                }

                if (interfacesElement.ChildNodes.Count > 0)
                {
                    typeElement.AppendChild(interfacesElement);
                }
            }

            if (type.BaseType != null && typeof(Thing).IsAssignableFrom(type.BaseType)) {
                AddObjectHierachyChild(type.BaseType, xmlDocument, typeElement);
            }

            parent.AppendChild(typeElement);
        }
    }
}