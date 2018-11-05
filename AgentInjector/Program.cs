using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AgentInjector
{
    class MainClass
    {
        const String AGENT_TYPE = "Stationeering.Agent";
        const String AGENT_METHOD = "exfiltrate";

        public static void Main(string[] args)
        {
            Console.WriteLine("Stationeering Exfiltration Agent Injector");

            // Ensure we have everything we need.
            if (args.Length < 5) {
                Console.WriteLine("Provide: <Agent Assembly> <Source Assembly> <Destination Assembly> <Injection Type> <Injection Method>");
                Environment.Exit(-1);
            }

            String agentAssemblyFile = args[0];
            String sourceAssemblyFile = args[1];
            String destinationAssemblyFile = args[2];
            String injectionType = args[3];
            String injectionMethod = args[4];

            Console.WriteLine("Agent Assembly: " + agentAssemblyFile);
            Console.WriteLine("Source Assembly: " + sourceAssemblyFile);
            Console.WriteLine("Destination Assembly: " + destinationAssemblyFile);
            Console.WriteLine("Injection Point: " + injectionType + "#" + injectionMethod);
            Console.WriteLine("Exfiltration Agent: " + AGENT_TYPE + "#" + AGENT_METHOD);

            // Load the agent assembly with the exfiltration agent.
            Console.WriteLine("Loading agent assembly...");
            ModuleDefinition agentAssembly = ModuleDefinition.ReadModule(agentAssemblyFile);

            // Load the Stationeers assembly.
            Console.WriteLine("Loading source assembly...");
            ModuleDefinition sourceAssembly = ModuleDefinition.ReadModule(sourceAssemblyFile);

            // Find our exfiltration agent.
            Console.WriteLine("Identifying exfiltration Type and Method...");
            TypeDefinition exfiltrationAgentType = agentAssembly.Types.First(x => x.FullName == AGENT_TYPE);
            MethodDefinition exfiltrationAgentMethod = exfiltrationAgentType.Methods.First(x => x.Name == AGENT_METHOD);

            // Find the injection type.
            Console.WriteLine("Identifying injection Type and Method...");
            TypeDefinition injectionPointType = sourceAssembly.Types.First(x => x.FullName == injectionType);
            Boolean injectionPointMethodExists = injectionPointType.Methods.Where(x => x.Name == injectionMethod).Any();

            MethodDefinition injectionPointMethod;

            // Begin injection.
            Console.WriteLine("Beginning injection...");

            if (injectionPointMethodExists) {           
                // Injection point exists, remove the last statement which should be a ret.
                Console.WriteLine("Injection method exists, removing return statement to make way for agent injection....");

                injectionPointMethod = injectionPointType.Methods.Where(x => x.Name == injectionMethod).First();
                injectionPointMethod.Body.Instructions.Remove(injectionPointMethod.Body.Instructions.Last());
            } else {                
                // Injection does not exist, we need to add one.
                Console.WriteLine("Injection method DOES NOT exists, creating new method with call to base...");
                Console.WriteLine("Creating injection point, override will call " + injectionPointType.BaseType.FullName + "#" + injectionMethod);

                TypeDefinition item = sourceAssembly.Types.First(x => x.FullName == injectionPointType.BaseType.FullName);
                MethodDefinition itemAwakeMethod = item.Methods.First(x => x.Name == injectionMethod);

                injectionPointMethod = new MethodDefinition(injectionMethod, itemAwakeMethod.Attributes, sourceAssembly.ImportReference(typeof(void)));
                injectionPointType.Methods.Add(injectionPointMethod);

                injectionPointMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Nop));
                injectionPointMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
                injectionPointMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Call, injectionPointMethod.Module.ImportReference(itemAwakeMethod)));
            }

            // Write call to exfiltration agent.
            Console.WriteLine("Injecting call to exfiltration agent...");
            injectionPointMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Nop));
            injectionPointMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            injectionPointMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Call, injectionPointMethod.Module.ImportReference(exfiltrationAgentMethod)));

            // Ensure awake returns.
            Console.WriteLine("Injecting return call again...");
            injectionPointMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

            // Output assembly.
            Console.WriteLine("Writing out modified assembly with agent injection.");
            sourceAssembly.Write(destinationAssemblyFile);

            Console.WriteLine("Injection completed.");
        }
    }
}
