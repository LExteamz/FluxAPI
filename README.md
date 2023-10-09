# FluxAPI
FluxAPI, simple exploit API.

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
private protected readonly Flux Fluxus = new Flux(); // Here, we're declaring the API.
public MainWindow()
{
      InitializeComponent(); 
}
```

Then, we need to Initialize the API, this will download files and redistributables of the API, like FluxteamAPI.dll and Module.dll into `./ProgramData` folder, located in C:\ProgramData
```csharp
Fluxus.InitializeAPI();
/* If you want to ensure it downloads the DLLs you can put Fluxus.DownloadDLLs();
below the InitializeAPI line (not recommended). */
```
If we want to add a custom identifyexecutor()/getexecutorname, we'll need to do:
```csharp
Fluxus.InitializeAsync("Executor Name");
```

We start the nice things, how to inject, is super-simple, just do: 
```csharp
Fluxus.Inject();
```

For executing we need a Textbox in our project, here is an example:

```csharp
private void Execute(object sender, EventArgs e)
{
      Fluxus.Execute(TextBox.Text);
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
        private protected readonly Flux Fluxus = new Flux();
        public MainWindow()
        {
            InitializeComponent(); 
            Fluxus.InitializeAsync();
        }

        private async void Attach_Click(object sender, EventArgs e)
        {
            Fluxus.Inject();
        }

        private void Run_Click(object sender, EventArgs e)
        {
            Fluxus.Execute(TextBox.Text);
        }
    }
}
 
```