# Agent Injector

A very basic and low level C# injector, will load the Stationeers assembly, find
the specified type and method, then either append or create the method making a
call to `Stationeering.Agent#exfiltrate`.

## Usage

```
# mono AgentInjector.exe
Stationeering Exfiltration Agent Injector
Provide: <Agent Assembly> <Source Assembly> <Destination Assembly> <Injection Type> <Injection Method>
```

* Agent Assembly - Compiled C# exfiltration agent with `Stationeering.Agent#exfiltrate` as a static method.
* Source Assembly - Stationeers Assembly-CSharp.dll with injection.
* Destination Assembly - Output filename for new Assembly-CSharp.dll with injection.
* Injection Type - Type to inject into.
* Injection Method - Method of add onto end.

## Known Issues

* Method being injected into must not return a value, may not work if it does.
* Type must have the method being injected into somewhere in its heirachy.

## Successful Run

Output upon injection will look something like this.

```
$ mono ~/stationeering/exfiltration/AgentInjector/bin/Debug/AgentInjector.exe Agent.dll Assembly-CSharp.dll-original Assembly-CSharp.dll Assets.Scripts.Objects.MultiConstructor Awake
Stationeering Exfiltration Agent Injector

Agent Assembly: Agent.dll
Source Assembly: Assembly-CSharp.dll-original
Destination Assembly: Assembly-CSharp.dll
Injection Point: Assets.Scripts.Objects.MultiConstructor#Awake
Exfiltration Agent: Stationeering.Agent#Exfiltrate

Loading agent assembly...
Loading source assembly...
Identifying exfiltration Type and Method...
Identifying injection Type and Method...
Beginning injection...
Injection method DOES NOT exists, creating new method with call to base...
Checking for Assets.Scripts.Objects.MultiConstructor#Awake...
Not found, going up...
Checking for Assets.Scripts.Objects.Items.Stackable#Awake...
Not found, going up...
Checking for Assets.Scripts.Objects.Item#Awake...
Found!
Injecting call to exfiltration agent...
Injecting return call again...
Writing out modified assembly with agent injection.
Injection completed.
```
