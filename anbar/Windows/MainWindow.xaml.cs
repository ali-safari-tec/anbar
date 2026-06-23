using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using anbar.Tools;
using Microsoft.Win32;
using anbar.User_Controls;
using anbar.Data.Services;
using System.Linq;
using anbar.Data.Models;
using System.Windows.Media;

namespace anbar.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ClockViewModel();
            Load();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<FinancialYear>(db);
                int lastYear = db.financials.Select(c => c.LastYear).FirstOrDefault();
                var update = await context.GetId(1);
                update.ShowYear = lastYear;
                await context.Update(1, update);
            }
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_MouseEnter_Person(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelPerson.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave_Person(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelPerson.Visibility = Visibility.Collapsed;
        }

        private void DropdownPanelPerson_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelPerson.Visibility = Visibility.Visible;
        }

        private void DropdownPanelPerson_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelPerson.Visibility = Visibility.Collapsed;
        }

        private void Button_MouseEnter_Sale(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelSale.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave_Sale(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelSale.Visibility = Visibility.Collapsed;
        }

        private void Button_MouseEnter_Buy(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelBuy.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave_Buy(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelBuy.Visibility = Visibility.Collapsed;
        }

        private void DropdownPanelBuy_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelBuy.Visibility = Visibility.Visible;
        }

        private void DropdownPanelBuy_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DropdownPanelBuy.Visibility = Visibility.Collapsed;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (DropdownPanelMenu.Visibility == Visibility.Visible)
            {
                DropdownPanelMenu.Visibility = Visibility.Collapsed;
            }
            else if (DropdownPanelMenu.Visibility == Visibility.Collapsed)
            {
                DropdownPanelMenu.Visibility = Visibility.Visible;
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            DropdownPanelInfo.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            DropdownPanelInfo.Visibility = Visibility.Collapsed;
        }

        private void DropdownPanelInfo_MouseEnter(object sender, MouseEventArgs e)
        {
            DropdownPanelInfo.Visibility = Visibility.Visible;
        }

        private void DropdownPanelInfo_MouseLeave(object sender, MouseEventArgs e)
        {
            DropdownPanelInfo.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_Buyer(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelPerson.Visibility = Visibility.Collapsed;
            USPerson form = new USPerson();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_Seller(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelPerson.Visibility = Visibility.Collapsed;
            USProduct form = new USProduct();
            MainGrid.Children.Add(form);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && e.Key == Key.F4)
            {
                Button_Click(exit, null);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelBuy.Visibility = Visibility.Collapsed;
            var form = new USInvoice();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MenuGrid.Children.Clear();
            MenuGrid.Visibility = Visibility.Visible;
            MenuGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelMenu.Visibility = Visibility.Collapsed;
            var form = new Developer();
            MenuGrid.Children.Add(form);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MenuGrid.Children.Clear();
            MenuGrid.Visibility = Visibility.Visible;
            MenuGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelMenu.Visibility = Visibility.Collapsed;
            var form = new EditUser();
            MenuGrid.Children.Add(form);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                DropdownPanelMenu.Visibility = Visibility.Collapsed;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "RAR Files (*.rar)|*.rar",
                    Title = "محل ذخیره‌سازی بکاپ را انتخاب کنید"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string backupPath = saveFileDialog.FileName;
                    string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB", "db.sqlite");

                    if (string.IsNullOrEmpty(dbPath))
                    {
                        MessageBox.Show("❌ فایل دیتابیس پیدا نشد! عملیات بکاپ‌گیری متوقف شد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string password = "sa12fa34ri56";
                    string winrarExePath = rar.Path();

                    if (string.IsNullOrEmpty(winrarExePath) || !File.Exists(winrarExePath))
                    {
                        MessageBox.Show("❌ نرم‌افزار WinRAR روی این سیستم نصب نیست یا مسیر آن یافت نشد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Backup.createBackup(winrarExePath, dbPath, backupPath, password);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                DropdownPanelMenu.Visibility = Visibility.Collapsed;

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "RAR Files (*.rar)|*.rar",
                    Title = "بکاپ مورد نظر را انتخاب کنید"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string backupPath = openFileDialog.FileName;
                    string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB");
                    string dbPath = Path.Combine(dataFolder, "db.sqlite");

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (File.Exists(dbPath))
                    {
                        try
                        {
                            File.Delete(dbPath); // حذف بدون انتقال به Recycle Bin
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("❌ خطا در حذف فایل دیتابیس قبلی: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    string password = "sa12fa34ri56";
                    string winrarExePath = rar.Path();

                    if (string.IsNullOrEmpty(winrarExePath) || !File.Exists(winrarExePath))
                    {
                        MessageBox.Show("❌ نرم‌افزار WinRAR روی این سیستم نصب نیست یا مسیر آن یافت نشد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Restore.RestoreBackup(winrarExePath, backupPath, dataFolder, password);
                    Load();
                    MenuGrid.Children.Clear();
                    MainGrid.Children.Clear();
                    FincialGrid.Children.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_MouseEnter_Tool(object sender, MouseEventArgs e)
        {
            DropdownPanelTool.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave_Tool(object sender, MouseEventArgs e)
        {
            DropdownPanelTool.Visibility = Visibility.Collapsed;
        }

        private void DropdownPanelTool_MouseEnter(object sender, MouseEventArgs e)
        {
            DropdownPanelTool.Visibility = Visibility.Visible;
        }

        private void DropdownPanelTool_MouseLeave(object sender, MouseEventArgs e)
        {
            DropdownPanelTool.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelTool.Visibility = Visibility.Collapsed;
            var form = new CutForm();
            MainGrid.Children.Add(form);
        }

        private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        {
            DropdownPanelList.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            DropdownPanelList.Visibility = Visibility.Collapsed;
        }

        private void DropdownPanelList_MouseEnter(object sender, MouseEventArgs e)
        {
            DropdownPanelTool.Visibility = Visibility.Visible;
            DropdownPanelList.Visibility = Visibility.Visible;
        }

        private void DropdownPanelList_MouseLeave(object sender, MouseEventArgs e)
        {
            DropdownPanelTool.Visibility = Visibility.Collapsed;
            DropdownPanelList.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelTool.Visibility = Visibility.Collapsed;
            DropdownPanelList.Visibility = Visibility.Collapsed;
            var form = new PriceListO();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelTool.Visibility = Visibility.Collapsed;
            DropdownPanelList.Visibility = Visibility.Collapsed;
            var form = new PriceListM();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelTool.Visibility = Visibility.Collapsed;
            var form = new PaintPrice();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelBuy.Visibility = Visibility.Collapsed;
            var form = new UCConfirmInvoice();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelBuy.Visibility = Visibility.Collapsed;
            var form = new UCPreInvoice();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelBuy.Visibility = Visibility.Collapsed;
            var form = new USBuy();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelBuy.Visibility = Visibility.Collapsed;
            var form = new USBuy2();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Children.Clear();
            MainGrid.Visibility = Visibility.Visible;
            MainGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelBuy.Visibility = Visibility.Collapsed;
            var form = new USCost();
            MainGrid.Children.Add(form);
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            MainGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Children.Clear();
            FincialGrid.Visibility = Visibility.Visible;
            FincialGrid.Background = new SolidColorBrush(Colors.White);
            DropdownPanelMenu.Visibility = Visibility.Collapsed;
            var form = new FincialYear();
            FincialGrid.Children.Add(form);
            form.Save = Load;
            Layout();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
            MenuGrid.Visibility = Visibility.Collapsed;
            FincialGrid.Visibility = Visibility.Collapsed;
            MainGrid.Background = new SolidColorBrush(Colors.Transparent);
            MenuGrid.Background = new SolidColorBrush(Colors.Transparent);
            FincialGrid.Background = new SolidColorBrush(Colors.Transparent);
            Load();
        }

        private void Layout()
        {
            Introduction.IsEnabled = false;
            sellPart.IsEnabled = false;
            buyPart.IsEnabled = false;
            toolsPart.IsEnabled = false;
            btnMenu.IsEnabled = false;
            btnHome.IsEnabled = false;

        }

        private void Load()
        {
            using (var db = new DataBaseContext())
            {
                int year = db.financials.Select(c => c.ShowYear).FirstOrDefault();
                decimal sumInvoice = db.invoices.Where(c => c.DateYear == year).Sum(c => (decimal?)c.InvoiceTotalCost) ?? 0;
                decimal discountInvoice = db.invoices.Where(c => c.DateYear == year).Sum(c => (decimal?)c.InvoiceDiscount) ?? 0;
                decimal payInvoice = db.invoices.Where(c => c.DateYear == year).Sum(c => (decimal?)c.InvoicePay) ?? 0;
                decimal sumBuyInvoice = db.buyInvoices.Where(c => c.DateYear == year).Sum(c => (decimal?)c.BuyInvoiceTotalCost) ?? 0;
                decimal discountBuyInvoice = db.buyInvoices.Where(c => c.DateYear == year).Sum(c => (decimal?)c.BuyInvoiceDiscount) ?? 0;
                decimal payBuyInvoice = db.buyInvoices.Where(c => c.DateYear == year).Sum(c => (decimal?)c.BuyInvoicePay) ?? 0;
                decimal sumCost = db.costs.Where(c => c.year == year).Sum(c => (decimal?)c.Fee) ?? 0;
                decimal discountCost = db.costs.Where(c => c.year == year).Sum(c => (decimal?)c.Discount) ?? 0;
                decimal payCost = db.costs.Where(c => c.year == year).Sum(c => (decimal?)c.Pay) ?? 0;

                decimal invoice = sumInvoice - discountInvoice;
                decimal buyInvoice = sumBuyInvoice - discountBuyInvoice;
                decimal cost = sumCost - discountCost;
                decimal sumPay = payInvoice - payCost - payBuyInvoice;

                profitPay.Visibility = Visibility.Collapsed;
                lossPay.Visibility = Visibility.Collapsed;

                if (sumPay > 0)
                {
                    profitPay.Visibility = Visibility.Visible;
                }
                else if (sumPay < 0)
                {
                    lossPay.Visibility = Visibility.Visible;
                }
                else
                {
                    lossPay.Visibility = Visibility.Collapsed;
                    profitPay.Visibility = Visibility.Collapsed;
                }

                profitInvoice.Visibility = Visibility.Collapsed;
                lossInvoice.Visibility = Visibility.Collapsed;

                decimal sumTotal = invoice - cost - buyInvoice;
                if (sumTotal > 0)
                {
                    profitInvoice.Visibility = Visibility.Visible;
                }
                else if (sumTotal < 0)
                {
                    lossInvoice.Visibility = Visibility.Visible;
                }
                else
                {
                    lossInvoice.Visibility = Visibility.Collapsed;
                    profitInvoice.Visibility = Visibility.Collapsed;
                }

                sumS.Text = MoneyConverter.Money(invoice);
                sumB.Text = MoneyConverter.Money(buyInvoice);
                sumC.Text = MoneyConverter.Money(cost);
                sumPayS.Text = MoneyConverter.Money(payInvoice);
                sumPayB.Text = MoneyConverter.Money(payCost + payBuyInvoice);
                totalPay.Text = MoneyConverter.Money(sumPay);
                totalInvoice.Text = MoneyConverter.Money(sumTotal);
            }
        }
    }
}
