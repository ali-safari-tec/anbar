using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using anbar.Data.Dto;
using anbar.Data.Models;
using anbar.Data.Services;
using anbar.Tools;
using anbar.Windows;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for UCConfirmInvoice.xaml
    /// </summary>
    public partial class UCConfirmInvoice : UserControl
    {
        private int lastSelectedIndex = -1;

        public UCConfirmInvoice()
        {
            InitializeComponent();
            Load();
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is T parentT)
                    return parentT;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        private void Layout()
        {
            var mainWin = FindParent<MainWindow>(this);
            if (mainWin != null)
            {
                mainWin.btnHome.IsEnabled = false;
                mainWin.btnMenu.IsEnabled = false;
                mainWin.Introduction.IsEnabled = false;
                mainWin.buyPart.IsEnabled = false;
                mainWin.sellPart.IsEnabled = false;
                mainWin.toolsPart.IsEnabled = false;
            }
        }

        private void RefreshDataGrid(List<InvoiceDto> newItems)
        {
            lastSelectedIndex = dgInvoice.SelectedIndex;

            // مقداردهی مجدد
            dgInvoice.ItemsSource = null;
            dgInvoice.ItemsSource = newItems;

            Dispatcher.InvokeAsync(() =>
            {
                // اگر آیتمی باقی مونده، سعی کن همون یا یکی قبلی رو انتخاب کنی
                if (newItems.Count > 0)
                {
                    // اگه ایندکس قبلی معتبر نیست (مثلاً آیتم حذف شده)، یک پله برگرد عقب
                    if (lastSelectedIndex >= newItems.Count)
                        lastSelectedIndex = newItems.Count - 1;

                    if (lastSelectedIndex >= 0)
                    {
                        var selectedItem = dgInvoice.Items[lastSelectedIndex];

                        dgInvoice.SelectedIndex = lastSelectedIndex;
                        dgInvoice.SelectedItem = selectedItem;

                        dgInvoice.CurrentCell = new DataGridCellInfo(selectedItem, dgInvoice.Columns[0]);
                        dgInvoice.ScrollIntoView(selectedItem);

                        // فوکوس دقیق روی سلول اول
                        if (dgInvoice.ItemContainerGenerator.ContainerFromItem(selectedItem) is DataGridRow row)
                        {
                            if (dgInvoice.Columns[0].GetCellContent(row)?.Parent is DataGridCell cell)
                            {
                                cell.Focus();
                            }
                        }
                    }
                }
            }, DispatcherPriority.ApplicationIdle);
        }

        private async void Load()
        {
            using (var db = new DataBaseContext())
            {
                int dateYear = db.financials.Select(c => c.ShowYear).FirstOrDefault();
                var context = new GenericDataBaseServices<Invoice>(db);
                var load = await context.GetSelectedInvoicee(2, dateYear);
                RefreshDataGrid(load.ToList());
            }
            dgInvoice.Focus();
        }

        private void DGLoad()
        {
            if (dgInvoice.Items.Count > 0)
            {
                if (dgInvoice.SelectedIndex >= 0)
                {
                    lastSelectedIndex = dgInvoice.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                }

                // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                Dispatcher.InvokeAsync(() =>
                {
                    // بررسی ایندکس قبلی و تنظیم آن
                    if (lastSelectedIndex >= 0 && lastSelectedIndex < dgInvoice.Items.Count)
                    {
                        // دوباره سلکت کردن ایندکس قبلی
                        dgInvoice.SelectedIndex = lastSelectedIndex;
                    }
                    else
                    {
                        // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                        dgInvoice.SelectedIndex = 0;
                    }

                    // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                    dgInvoice.CurrentCell = new DataGridCellInfo(dgInvoice.SelectedItem, dgInvoice.Columns[0]);
                    dgInvoice.Focus(); // فوکوس روی دیتاگرید

                    // انتقال فوکوس به سلکت شده جدید
                    dgInvoice.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        private void LoadFullDG()
        {
            if (dgInvoice.SelectedItem is InvoiceDto selectedInvoice)
            {
                txtDay.Text = selectedInvoice.DateDay.ToString();
                txtMonth.Text = selectedInvoice.DateMonth.ToString();
                txtYear.Text = selectedInvoice.DateYear.ToString();
                decimal totalcost = selectedInvoice.InvoiceTotalCost;
                txtTotalFee.Text = MoneyConverter.Money(totalcost).ToString();
                decimal discount = selectedInvoice.InvoiceDiscount;
                txtDiscount.Text = MoneyConverter.Money(discount).ToString();
                decimal totalpay = totalcost - discount;
                txtTotalPay.Text = MoneyConverter.Money(totalpay).ToString();
                decimal pay = selectedInvoice.InvoicePay;
                txtPay.Text = MoneyConverter.Money(pay).ToString();
                decimal left = totalpay - pay;
                txtLeft.Text = MoneyConverter.Money(left).ToString();
            }
        }

        private void LoadEmptyDG()
        {
            txtDay.Text = string.Empty;
            txtMonth.Text = string.Empty;
            txtYear.Text = string.Empty;
            txtTotalFee.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            txtTotalPay.Text = string.Empty;
            txtPay.Text = string.Empty;
            txtLeft.Text = string.Empty;
        }

        private async void saveInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفا یک آیتم را انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("آیا از تبدیل این فاکتور تایید شده به فاکتور های فروش مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Invoice>(db);
                    dynamic select = dgInvoice.SelectedItem;
                    int Id = select.InvoiceId;

                    var update = await context.GetId(Id);
                    if (update != null)
                    {
                        update.Type = 1;
                    }
                    var confirm = await context.Update(Id, update);
                    if (confirm == null)
                    {
                        MessageBox.Show("خطا در بازگرداندن فاکتور تایید شده به حالت فاکتور", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Load();
        }

        private void dgInvoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                LoadEmptyDG();
            }
            else
            {
                LoadFullDG();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new DataBaseContext())
            {
                int lastYear = db.financials.Select(c => c.LastYear).FirstOrDefault();
                int year = db.financials.Select(c => c.ShowYear).FirstOrDefault();

                if (lastYear == year)
                {
                    saveInvoice.Visibility = Visibility.Visible;
                }
                else
                {
                    saveInvoice.Visibility = Visibility.Collapsed;
                }
                dgInvoice.Focus();
                DGLoad();
            }
        }

        private void LoadNewUC(string title, Invoice invoice = null)
        {
            var load = new UCConfirmSellForm();
            load.TitleInvoice = title;
            if (invoice != null)
            {
                load.InvoiceToEdit = invoice;
            }
            UserGrid.Children.Clear();
            load.InvoiceSave = Load;
            UserGrid.Children.Add(load);
        }

        private async void showInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.ItemsSource == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<Invoice>(db);

                dynamic select = dgInvoice.SelectedItem;

                if (select == null)
                {
                    MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int Id = select.InvoiceId;

                var update = await context.GetId(Id);
                if (update == null)
                {
                    MessageBox.Show("شخص مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                UserGrid.Visibility = Visibility.Visible;
                LoadNewUC("2", update);
            }
            Layout();
        }

        private void dgInvoice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dgInvoice.Focus();
        }

        private void dgInvoice_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                showInvoice_Click(showInvoice, null);
            }
            if (e.Key == Key.C)
            {
                saveInvoice_Click(saveInvoice, null);
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (UserGrid.Visibility == Visibility.Visible)
                return;

            if (!dgInvoice.IsKeyboardFocusWithin)
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    showInvoice_Click(showInvoice, null);
                }
                if (e.Key == Key.C)
                {
                    saveInvoice_Click(saveInvoice, null);
                }
            }
        }
    }
}
