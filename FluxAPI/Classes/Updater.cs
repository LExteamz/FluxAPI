using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using File = System.IO.File;

namespace FluxAPI.Classes
{
    internal static class Updater
    {
        static WebClient webClient = new WebClient();

        internal static async Task CheckForUpdates()
        {
            try
            {
                string jsonContent = await webClient.DownloadStringTaskAsync(FluxFiles.DLLsJSON);

                if (!string.IsNullOrEmpty(jsonContent))
                {
                    JObject jsonObject = JObject.Parse(jsonContent);

                    string fluxUrl = jsonObject["FluxteamAPI"].ToString();
                    string moduleUrl = jsonObject["Module"].ToString();

                    string localFluxChecksum = DoChecksum(FluxFiles.Interfacer);
                    string localModuleChecksum = DoChecksum(FluxFiles.Module);

                    string newFluxChecksum = await GetChecksum(fluxUrl);
                    string newModuleChecksum = await GetChecksum(moduleUrl);

                    if (string.IsNullOrEmpty(localFluxChecksum) || localFluxChecksum != newFluxChecksum ||
                        string.IsNullOrEmpty(localModuleChecksum) || localModuleChecksum != newModuleChecksum)
                    {
                        RedownloadModules();

                        RegistryHandler.SetValue("FluxChecksum", newFluxChecksum);
                        RegistryHandler.SetValue("ModuleChecksum", newModuleChecksum);
                    }

                    MessageBox.Show(
                        $"Local FluxteamAPI.dll SHA1: {localFluxChecksum}\n" +
                        $"Extrn FluxteamAPI.dll SHA1: {newFluxChecksum}\n" +
                        $"Local Module.dll      SHA1: {localModuleChecksum}\n" +
                        $"Extrn Module.dll      SHA1: {newModuleChecksum}", FluxFiles.Executor);
                }
                else
                {
                    Console.WriteLine("Could not fetch JSON");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CheckForUpdates: " + ex.InnerException?.Message);
            }
        }


        internal static void DeleteFilesAndFoldersRecursively(string target_dir)
        {
            foreach (string file in Directory.GetFiles(target_dir))
            {
                try { File.Delete(file); } catch { }
            }

            foreach (string subDir in Directory.GetDirectories(target_dir))
            {
                DeleteFilesAndFoldersRecursively(subDir);
            }

            Task.Delay(1000);
            try { Directory.Delete(target_dir, true); } catch { }
        }

        internal static void RedownloadModules()
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    string jsonData = webClient.DownloadString(FluxFiles.DLLsJSON);
                    JObject jsonObject = JObject.Parse(jsonData);

                    string fluxteamAPI = jsonObject["FluxteamAPI"].ToString();
                    string module = jsonObject["Module"].ToString();

                    if (!string.IsNullOrEmpty(fluxteamAPI) && !string.IsNullOrEmpty(module))
                    {
                        var interfacer = new Uri(fluxteamAPI);
                        var moduleUri = new Uri(module);

                        if (Directory.Exists(FluxFiles.FluxFolder))
                        {
                            DeleteFilesAndFoldersRecursively(FluxFiles.FluxFolder);
                            Directory.CreateDirectory(FluxFiles.FluxFolder);
                        }
                        else
                        {
                            Directory.CreateDirectory(FluxFiles.FluxFolder);
                        }

                        webClient.DownloadFile(interfacer, Path.Combine(FluxFiles.FluxFolder, "FluxteamAPI.dll"));
                        webClient.DownloadFile(moduleUri, Path.Combine(FluxFiles.FluxFolder, "Module.dll"));
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch { }
        }

        private static async Task<string> GetChecksum(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        Stream stream = await response.Content.ReadAsStreamAsync();
                        using (SHA1 sha1 = SHA1.Create())
                        {
                            byte[] hashBytes = sha1.ComputeHash(stream);
                            return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
                        }
                    }
                    return null;
                }
            }
            catch
            {
                return "Couldn't calculate.";
            }
        }

        private static string DoChecksum(string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                using (SHA1 sha1 = SHA1.Create())
                {
                    byte[] hashBytes = sha1.ComputeHash(fileStream);
                    return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
                }
            }
            catch
            {
                return "Couldn't calculate.";
            }
        }
    }
}
