using System.Security.Principal;

namespace FluxAPI.Classes
{
    internal static class Utility
    {
        internal static bool IsUser()
        {
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.User);
            }

            return isElevated;
        }
    }
}
