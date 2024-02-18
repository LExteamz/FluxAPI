# FluxAPI
FluxAPI, simple exploit API.

# No longer working
Since the game introduced Hyperion in the Universal Windows Platform version of it, and the executable file switched to x64 bits architecture. The owners of the modules discontinued it. Hence why, this project is discontinued too.

## DISCLAIMER
To whom it may concern,

I hereby declare that I have created an open-source exploit API named "FluxAPI" solely for educational purposes. I explicitly refuse any responsibility for any misuse or illegal activities that may arise from the use of this API. The code is provided "as is", without any warranty, and users assume all risks associated with its use.
<br><br>
I do not condone or support any unethical or malicious activities that may be facilitated using FluxAPI. It is the responsibility of the users to ensure that they comply with all applicable laws and regulations while using this API.
<br><br>
I urge all users to use FluxAPI only for educational purposes and to refrain from using it for any illegal or unethical activities. By using this API, users acknowledge and agree to assume all risks associated with its use.

## How this works
JJSploit, by WeAreDevs got dissasembled and a guy on [GitHub uploaded a repository](https://github.com/MoistMonkey420/MicrosoftRobloxFluxAPI/)
showing an example of how to use it.
There are more ports of this type, like this thread on [WeAreDevs](https://forum.wearedevs.net/t/34077)

This Class Library I created uses the [JJploit](https://wearedevs.net/dinfo/JJSploit) DLLs, provided by [Fluxus](https://fluxteam.net).

## Documentation
#### How to create a simple executor with FluxAPI:
Add the FluxAPI.dll to references of your Visual Studio Project, we need the next things:
- .NET Framework 4.8 Project (Windows Forms or WPF (Windows Presentation Framework))
- x86 Build Configuration
- Simple C# Knowledge<br>
- This will work on the UWP version, not Web Version
In the entry of your program. Put the class name above the declaration of the form, here is an example: 
```csharp
private protected readonly Flux GetFlux = new Flux(); // Here, we're declaring the API.

public MainWindow()
{
      InitializeComponent();
}
```

Then, we need to Initialize the API, this will download files and redistributables of the API, like FluxteamAPI.dll and Module.dll into folder, located in `C:\ProgramData\Flux`
If we want to add a custom _identifyexecutor()/getexecutorname_ and _request header_, we'll need to do:
```csharp
GetFlux.InitializeAsync("Executor Name");
```

If we want an auto attach, we can do this below the initialize function **BELOW THE INITIALIZE LINE**. You're responsible for the use of the boolean.
```csharp
GetFlux.DoAutoAttach = true;
GetFlux.InitializeAsync("Executor Name");
```

We start the nice things, how to inject, is super-simple, just do: 
```csharp
GetFlux.Inject(<void>);
```

For executing we need a Textbox in our project, here is an example:

```csharp
private void Execute(object sender, EventArgs e)
{
      GetFlux.Execute(<string> Text);
}
```

**Our code should be like this:**
```csharp
using System;
using System.Windows.Forms;
using FluxAPI;

namespace FluxTest
{
    public partial class MainWindow : Form
    {
        private protected readonly Flux GetFlux = new Flux();
        public MainWindow()
        {
            InitializeComponent(); 
            GetFlux.InitializeAsync();
        }

        private async void Attach_Click(object sender, EventArgs e)
        {
            GetFlux.Inject();
        }

        private void Run_Click(object sender, EventArgs e)
        {
            GetFlux.Execute(TextBox.Text);
        }
    }
}
 
```
