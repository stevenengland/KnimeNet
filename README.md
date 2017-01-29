
		 _    _       _             ______             
		| |  / )     (_)           |  ___ \       _    
		| | / / ____  _ ____   ____| |   | | ____| |_  
		| |< < |  _ \| |    \ / _  ) |   | |/ _  )  _) 
		| | \ \| | | | | | | ( (/ /| |   | ( (/ /| |__ 
		|_|  \_)_| |_|_|_|_|_|\____)_|   |_|\____)\___)
                                         (c)Steven England             

KnimeNet
========================================
KnimeNet was started as a side project to support my master thesis. For now there is only the need to have some more flexible command line access to KNIME.
I wanted to use .NET Applications to hold the business logic whereas KNIME should follow this logic. Bash/Cmd represents the middleware in between. This is why I wrote KnimeNet.CommandLine.
As the time goes on I will have a look at what might be also useful. Ideas would be appreciated :)

Documentation
---
The assembly documentation can be found under [knimenet.steven-england.info](https://knimenet.steven-england.info).

KnimeNet.CommandLine
===

Do you use KNIME to automate workflow execution? And the following assumptions do also match?
- You don't have access to a KNIME Server installation where you can actually automate workflows via the HTTP interface.
- You cannot guarantee that your KNIME desktop application runs round about the clock so built in scheduleing mechanisms don't work for you.
- You would like to have a more flexible interface than standard shell access to the KNIME batch application.
Then KnimeNet.CommandLine may be interesting for you. A KNIME workflow called from command line seems to be very static and uncontrolled (almost fire and forget if you call it from minimalistic shell or batch scripts) at first sight. 
But with the options of providing runtime variables and access to the KNIME output during runtime it is getting more and more flexible. With the help of KnimeNet.CommandLine you can orchestrate your workflows in a .Net manner.

Quickstart
---------------------
**Getting the binaries**

Required:
You have two options:
- Dowload and reference this project.
- Getting the precompiled packages from Nuget.

**Write a very basic example**

Create the proxy instance:
```csharp
/* If KNIME is known in PATH */
var shellProxy = new ShellProxy();
/* otherwise */
var shellProxy = new ShellProxy("DirToKnimeExecutable");
```
Provide arguments:
```csharp
shellProxy.Arguments.WorkFlowDir = "DirToWorkFlow";
```
Execute the workflow:
```csharp
var exitStatus = await shellProxy.StartKnime();
/* Do something with the exit status */
if (exitStatus.ExitCode != 0)
{
	// Prepare a new execution, send mails, ...  
}
```
For more examples see the section below concerning examples.

Coverage
---------------------
- Covers all native KNIME command line options (~3.2.2).
- Covers important additional Eclipse options.
- Generic support for VM arguments.

Recommended command line options
---------------------
If you like to run completely unattended workflow executions these are the favorite options:
```csharp
NoSplash = true; // Avoids a splash screen.
NoExit = false; // Avoids that KNIME does not exit after execution.
ConsoleLog = false; // Avoids a second command line window during the execution.
SuppressError = true; // Avoids a blocking popup at the end of the execution.
```
Do you have a few examples for us?
===
KnimeNet ships with a [sample project](KnimeNet.Example/Program.cs) and a basic [test project](KnimeNet.Test/TestCommandLine.cs).
You're also invited to visit the [wiki](https://github.com/stevenengland/KnimeNet/wiki).

License
===
KnimeNet is released under MIT license. 
See [LICENSE.md](LICENSE.md) for details.