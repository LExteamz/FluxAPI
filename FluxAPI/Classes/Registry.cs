using Microsoft.Win32;
using System;

namespace FluxAPI.Classes
{
    internal class RegistryHandler
    {

        internal static string GetValue(string name, string fallback)
        {
            try
            {
                var registryAddress = $"SOFTWARE\\{FluxFiles.Executor}";
                using (RegistryKey reg = Registry.CurrentUser.OpenSubKey(registryAddress))
                {
                    string returnValue = fallback;
                    if (reg == null)
                    {
                        using (var newKey = Registry.CurrentUser.CreateSubKey(registryAddress))
                        {
                            if (newKey != null)
                            {
                                newKey.Close();
                            }
                            else
                            {
                                Console.WriteLine("Could not create the SubKey");
                                return fallback;
                            }
                        }
                    }

                    var value = reg.GetValue(name);
                    if (value != null)
                    {
                        returnValue = value.ToString();
                    }
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetValue: " + ex.Message);
                return fallback;
            }
        }

        internal static void SetValue(string name, string value)
        {
            try
            {
                var registryAddress = $"SOFTWARE\\{FluxFiles.Executor}";
                using (RegistryKey reg = Registry.CurrentUser.OpenSubKey(registryAddress, true))
                {
                    if (reg == null)
                    {
                        using (var newKey = Registry.CurrentUser.CreateSubKey(registryAddress))
                        {
                            if (newKey != null)
                            {
                                newKey.Close();
                            }
                            else
                            {
                                Console.WriteLine("Could not create the SubKey");
                                return;
                            }
                        }
                    }

                    reg.SetValue(name, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SetValue: " + ex.Message);
            }
        }
    }
}
