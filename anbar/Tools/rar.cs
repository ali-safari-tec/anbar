using System.IO;
using Microsoft.Win32;

namespace anbar.Tools
{
    public class rar
    {
        public static string Path()
        {
            string[] registryKeys =
    {
        @"SOFTWARE\WinRAR",
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe",
    };

            foreach (string regKey in registryKeys)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regKey))
                {
                    if (key != null)
                    {
                        // بررسی exe64 و exe32
                        object exe64 = key.GetValue("exe64");
                        object exe32 = key.GetValue("exe32");
                        object defaultValue = key.GetValue("");

                        if (exe64 != null && File.Exists(exe64.ToString()))
                            return exe64.ToString();

                        if (exe32 != null && File.Exists(exe32.ToString()))
                            return exe32.ToString();

                        if (defaultValue != null && File.Exists(defaultValue.ToString()))
                            return defaultValue.ToString();
                    }
                }
            }

            return null;
        }
    }
}
