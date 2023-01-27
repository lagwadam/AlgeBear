# AlgeBear 🧸

AlgeBear is hear to help wit you alebra and calculus!

> 🚨 Fin the latest code in the Patch branch 🚨

## Installation

run this file:
```bash

```

## CLI Usage


To run from vs code consule, move to application folder and enter 

> dotnet run

In the console, enter an expression like an array of polynomials coefficents. The syntax is json, for easy deserialization

> [2,-1,3] ⟶ 2 - x + 2x^2

You can also enter sums of and products of polynomails

> [1, -1]\*[1, -1] ⟶ (1 - x)*(1 - x)

> [1] + [2, 2] + [2, 2, 2]  ⟶ 1 + (2 + 2x) + (3 + 3x + 3x^2)

You don't need to specify any operations to perform, and you enter the expressoin, the program will use the Class Library to show you a few examples like simplifying, expanding, differentiating, and integrating expressions.

## Contributing

Please make sure to update tests as appropriate.

## Publishing

This Microsoft [doc](https://learn.microsoft.com/en-us/dotnet/core/deploying/) explains everything, but (TLDR) just run one of these commands from the <code>AlgeBear\Application</code> folder<code>*</code>. 

###### <code>*</code>Note, this is the subfolder that contains the <code>Application.csproj</code> which is different than the root folder (AlgeBear) mentioned in the debuggin section below.


| Release | Command |
|-|:-|
| **Windows** | <code>dotnet publish -c Release -r win-x64 --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true</code> |
| **Mac** | <code>dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true</code> |
| **Linux** | <code>dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true</code> |
**With<RID\>**  | <code>dotnet publish -c Release -r <RID> --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true</code> |

For full specs on this publish command see [dotnet publish](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish). 
The **\<RID>** is the _Release Identifier_. For a full list of RIDs see the Microsoft [.NET RID Catalog](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)

<!-- > win-x64 // Windows
> osx-x64 // mac osx: Minimum OS version is macOS 10.12 Sierra
> linux-x64 // Linux: CentOS, Debian, Fedora, Ubuntu, etc. -->

Find published artifacts in one of these folders depending on the system for the release:

    Release\net6.0\win-x64\\publish
    Release\net6.0\osx-x64\\publish
    Release\net6.0\linux-x64\\publish 
    Release\net6.0\<RID>\\publish folder

## Testing & Debugging

To run the tests, just run this <code>dotnet test</code> from the solution's root folder __AlgeBear__. This will print out the results and any errors in the terminal. 

Instead of debugging all the tests, I find it easier to debug a single test buy clicking the "__debug__ button under the test's name in the editor. 

**Note**: You NEED to add these configurations to __Launch.json__. The first is for debugging tests, and the second is for debugging the console application.
<pre>
{
    "name": "Test Debugger",
    "type": "coreclr",
    "request": "launch",
    "preLaunchTask": "build",
    "program": "${workspaceFolder}/ExpressionLibraryTest/bin/Debug/net6.0/ExpressionLibraryTest.dll",
    "args": [],
    "cwd": "${workspaceFolder}/ExpressionLibraryTest",
    "console": "internalConsole",
    "stopAtEntry": false
}
</pre>
<pre>
    "name": "Console Debugger",
    "type": "coreclr",
    "request": "launch",
    "preLaunchTask": "build",
    "program": "${workspaceFolder}/Application/bin/Debug/net6.0/win-x64/Application.dll",
    "args": [],
    "cwd": "${workspaceFolder}/Application",
    "console": "integratedTerminal",
    "stopAtEntry": false
}
</pre>

You can run the console application from a command prompt or the vs code integrated terminal with the following command in the application folder:

    dotnet run
    

While this is nice, you cannot get detailed feedback unless you debug it. Navigate to the Application folder and enter

    F5
    

You can also use the vs code ui. Choose __Start Debugging__ from the __Run__ menu *(F5 is THE shortcut)*.

Lastly, make sure to build from the solution's root folder __AlgeBear__. If you are having trouble attaching the debugger all of a sudden, try closing vs code, deleting any __bin__ or __out__ folders, re-opening vs code, cleaning the solution, building the solution, and re-running the tests.



## License

[MIT](https://choosealicense.com/licenses/mit/) License
