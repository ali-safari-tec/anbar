using System;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace anbar.Tools
{
    public class Backup
    {
        public static void createBackup(string rarPath, string dbPath, string backupPath, string password)
        {
            try
            {
                string dbDirectory = Path.GetDirectoryName(dbPath);
                string dbFileName = Path.GetFileName(dbPath);

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = rarPath,
                    WorkingDirectory = dbDirectory, // داخل فولدر دیتابیس میره که فقط فایل رو بگیره
                    Arguments = $"a -ep1 -hp{password} \"{backupPath}\" \"{dbFileName}\"",
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                }

                MessageBox.Show("بکاپ‌گیری با موفقیت انجام شد!", "موفقیت", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بکاپ‌گیری: {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
