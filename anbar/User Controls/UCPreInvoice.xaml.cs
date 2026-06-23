using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for UCPreInvoice.xaml
    /// </summary>
    public partial class UCPreInvoice : UserControl
    {
        private int lastSelectedIndex = -1;

        public UCPreInvoice()
        {
            InitializeComponent();
            Load();
        }

        private void RefreshDataGrid(List<PreInvoiceDto> newItems)
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
                var context = new GenericDataBaseServices<PreInvoice>(db);

                var load = await context.GetSelectedPreInvoicee(dateYear);
                RefreshDataGrid(load.ToList());
            }
            dgInvoice.Focus();
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
            if (dgInvoice.SelectedItem is PreInvoiceDto selectedInvoice)
            {
                txtDay.Text = selectedInvoice.PreDateDay.ToString();
                txtMonth.Text = selectedInvoice.PreDateMonth.ToString();
                txtYear.Text = selectedInvoice.PreDateYear.ToString();
                txtDayS.Text = selectedInvoice.PreDateDayS.ToString();
                txtMonthS.Text = selectedInvoice.PreDateMonthS.ToString();
                txtYearS.Text = selectedInvoice.PreDateYearS.ToString();
                decimal totalcost = selectedInvoice.InvoiceTotalCost;
                txtTotalFee.Text = MoneyConverter.Money(totalcost).ToString();
                decimal discount = selectedInvoice.InvoiceDiscount;
                txtDiscount.Text = MoneyConverter.Money(discount).ToString();
                decimal totalpay = totalcost - discount;
                txtTotalPay.Text = MoneyConverter.Money(totalpay).ToString();
            }
        }

        private void LoadEmptyDG()
        {
            txtDay.Text = string.Empty;
            txtDayS.Text = string.Empty;
            txtMonth.Text = string.Empty;
            txtMonthS.Text = string.Empty;
            txtYear.Text = string.Empty;
            txtYearS.Text = string.Empty;
            txtTotalFee.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            txtTotalPay.Text = string.Empty;
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic select = dgInvoice.SelectedItem;
            int id = select.PreInvoiceId;
            if (MessageBox.Show("آیا از حذف این رکورد مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<PreInvoice>(db);
                    var delete = await context.DeletePreInvoice(id);
                    if (delete)
                    {
                        MessageBox.Show("رکورد با موفقیت حذف شد.", "حذف", MessageBoxButton.OK, MessageBoxImage.Information);
                        Load();
                    }
                    else
                    {
                        MessageBox.Show("خطا در حذف رکورد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در حذف رکورد: {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Title(string title, PreInvoice preInvoice = null)
        {
            var addOrEdit = new UCPreSellForm();
            addOrEdit.TitlePreInvoice = title;
            if (preInvoice != null)
            {
                addOrEdit.PreInvoiceToEdit = preInvoice;
            }
            UserGrid.Children.Clear();
            addOrEdit.PreInvoiceSave = Load;
            UserGrid.Children.Add(addOrEdit);
        }

        private async void edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dynamic select = dgInvoice.SelectedItem;
            int id = select.PreInvoiceId;
            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<PreInvoice>(db);
                var edit = await context.GetId(id);
                if (edit == null)
                {
                    MessageBox.Show("شخص مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                UserGrid.Visibility = Visibility.Visible;
                Title("2", edit);
            }
            Layout();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            UserGrid.Visibility = Visibility.Visible;
            Title("1");
            Layout();
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
                    add.Visibility = Visibility.Visible;
                    edit.Visibility = Visibility.Visible;
                    delete.Visibility = Visibility.Visible;
                    saveInvoice.Visibility = Visibility.Visible;
                    show.Visibility = Visibility.Collapsed;
                }
                else
                {
                    add.Visibility = Visibility.Collapsed;
                    edit.Visibility = Visibility.Collapsed;
                    delete.Visibility = Visibility.Collapsed;
                    saveInvoice.Visibility = Visibility.Collapsed;;
                    show.Visibility = Visibility.Visible;;
                }
                dgInvoice.Focus();
                DGLoad();
            }
        }

        private async void saveInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<PreInvoice>(db);

                    if (dgInvoice.ItemsSource == null)
                    {
                        MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    dynamic select = dgInvoice.SelectedItem;
                    int id = select.PreInvoiceId;
                    if (MessageBox.Show("آیا از تبدیل این پیش فاکتور به فاکتور مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                        return;

                    var convert = await context.ConvertProformaInvoiceAsync(id);
                    if (convert)
                    {
                        Load();
                    }
                    else
                    {
                        MessageBox.Show("خطا در تبدیل رکورد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در تبدیل رکورد: {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void show_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dynamic select = dgInvoice.SelectedItem;
            int id = select.PreInvoiceId;
            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<PreInvoice>(db);
                var edit = await context.GetId(id);
                if (edit == null)
                {
                    MessageBox.Show("شخص مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                UserGrid.Visibility = Visibility.Visible;
                Title("3", edit);
            }
            Layout();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (UserGrid.Visibility == Visibility.Visible)
                return;

            if (!dgInvoice.IsKeyboardFocusWithin)
            {
                if (e.Key == Key.D)
                {
                    delete_Click(delete, null);
                }
                if (e.Key == Key.A)
                {
                    add_Click(add, null);
                }
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    edit_Click(edit, null);
                }
                if (e.Key == Key.C)
                {
                    saveInvoice_Click(saveInvoice, null);
                }
            }
        }

        private void dgInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                delete_Click(delete, null);
            }
            if (e.Key == Key.A)
            {
                add_Click(add, null);
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                edit_Click(edit, null);
            }
            if (e.Key == Key.C)
            {
                saveInvoice_Click(saveInvoice, null);
            }
        }

        private void dgInvoice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dgInvoice.Focus();
        }
    }
}
