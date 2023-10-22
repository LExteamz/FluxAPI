using FluxAPI.Classes;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace FluxAPI
{
    internal static class FluxFiles
    {
        internal static readonly string AccountName = "LExteamz";
        internal const string AccountNamee = "LExteamz";

        internal static readonly string ProgramData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        internal static readonly string FluxFolder = Path.Combine(ProgramData, "Flux");

        internal static readonly string Interfacer = Path.Combine(FluxFolder, "FluxteamAPI.dll");
        internal static readonly string Module = Path.Combine(FluxFolder, "Module.dll");

        internal static readonly string DLLsURL = $"https://raw.githubusercontent.com/{AccountName}/LInjector/master/Redistributables/DLLs";
        internal static readonly string DLLsJSON = $"{DLLsURL}/Modules.json";

        internal static string Executor { get; set; }
        internal static bool IsInitialized = false;
        internal static string InitString { get; set; }
    }

    public class Flux
    {
        public bool DoAutoAttach { get; set; }

        public async Task InitializeAsync(string Executor = "FluxAPI")
        {
            FluxFiles.Executor = Executor;

            if (Utility.IsAdmin() == false)
            {
                MessageBox.Show("The application must be executed with Administrator privileges.", $"{Executor}", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }

            await Updater.CheckForUpdates();
            FluxInterfacing.create_files(FluxFiles.Module);

            FluxFiles.InitString = $"local a=\"{FluxFiles.Executor}\"local b;function Export(c,d)getgenv()[c]=d end;function HookedRequest(e)local f=e.Headers or{{}}f['User-Agent']=a;return b({{Url=e.Url,Method=e.Method or\"GET\",Headers=f,Cookies=e.Cookies or{{}},Body=e.Body or\"\"}})end;b=hookfunction(request,HookedRequest)b=hookfunction(http.request,HookedRequest)b=hookfunction(http_request,HookedRequest)Export(\"identifyexecutor\",function()return a end)Export(\"getexecutorname\",function()return a end)";
            FluxFiles.IsInitialized = true;

            RunAutoAttachTimer();
        }

        public void Inject()
        {
            if (!FluxFiles.IsInitialized)
            {
                MessageBox.Show("Initialize API First!", FluxFiles.Executor, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var flag = !FluxInterfacing.is_injected(FluxInterfacing.pid);
                if (flag)
                {
                    try
                    {
                        try
                        {
                            FluxInterfacing.inject();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{FluxFiles.Executor} encountered a unrecoverable error" +
                                                $"\nDue to Hyperion Byfron, {FluxFiles.Executor} only supports the game from Microsoft Store." +
                                                "\nException:\n"
                                                + ex.Message
                                                + "\nStack Trace:\n"
                                                + ex.StackTrace,
                                $"{FluxFiles.Executor} | Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while injecting:\n" + ex.Message
                                                                 + "\nStack Trace:\n" + ex.StackTrace,
                            $"{FluxFiles.Executor} | Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Already injected", FluxFiles.Executor,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public void Execute(string script)
        {
            try
            {
                var flag = FluxInterfacing.is_injected(FluxInterfacing.pid);
                if (flag)
                {
                    FluxInterfacing.run_script(FluxInterfacing.pid, $"{FluxFiles.InitString}; {script}");
                }
                else
                {
                    MessageBox.Show("Inject before running script.", FluxFiles.Executor, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch
            {
                MessageBox.Show("Couldn't run the script.", FluxFiles.Executor,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void RunAutoAttachTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += AttachedDetectorTick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        internal void AttachedDetectorTick(object sender, EventArgs e)
        {
            if (DoAutoAttach == false) { return; }

            var processesByName = Process.GetProcessesByName("Windows10Universal");
            foreach (var Process in processesByName)
            {
                var FilePath = Process.MainModule.FileName;

                if (FilePath.Contains("ROBLOX") || FilePath.Contains("Fluster"))
                {
                    try
                    {
                        var flag = FluxInterfacing.is_injected(FluxInterfacing.pid);
                        if (flag)
                        { return; }

                        Inject();
                    }
                    catch { }
                }
            }
        }
    }
}
