using System;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace anbar.Tools
{
    public class Restore
    {
        public static void RestoreBackup(string rarPath, string backupPath, string extractPath, string password)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = rarPath,
                    Arguments = $"x -o+ -hp{password} \"{backupPath}\" \"{extractPath}\\\"",
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                }

                MessageBox.Show("بازیابی بکاپ با موفقیت انجام شد!", "موفقیت", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بازیابی بکاپ: {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
