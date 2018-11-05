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
* Type must have a BaseType if method does not exist already.
