using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using anbar.Data.Dto;
using anbar.Data.Models;
using anbar.Data.Services;
using anbar.Tools;
using anbar.Windows;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for USCost.xaml
    /// </summary>
    public partial class USCost : UserControl
    {
        public int lastSelectedIndex = -1;

        public USCost()
        {
            InitializeComponent();
            Load();
        }

        private void RefreshDataGrid(List<CostDto> newItems)
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
                int year = db.financials.Select(c => c.ShowYear).FirstOrDefault();
                var context = new GenericDataBaseServices<CostDto>(db);

                var load = await context.GetSelectedCost(1, year);
                RefreshDataGrid(load.ToList());
            }
            dgInvoice.Focus();
        }

        private async void Check()
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفاً یک ردیف را انتخاب کنید!", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic select = dgInvoice.SelectedItem;
            int id = select.Id;

            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<Cost>(db);
                var update = await context.GetId(id);

                if (update == null)
                {
                    MessageBox.Show("رکورد موردنظر در دیتابیس یافت نشد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                decimal fee = update.Fee;
                decimal discount = update.Discount;
                decimal total = fee - discount;
                decimal pay = update.Pay;

                if (total == pay)
                {
                    type.Foreground = new SolidColorBrush(Colors.Green);
                    type.Text = "تسویه";
                }
                else if (total > pay) 
                {
                    type.Foreground = new SolidColorBrush(Colors.Red);
                    type.Text = "بدهکار";
                }
                else
                {
                    type.Foreground = new SolidColorBrush(Colors.YellowGreen);
                    type.Text = "طلبکار";
                }
            }
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
            add.IsEnabled = false;
            edit.IsEnabled = false;
            delete.IsEnabled = false;
            dgInvoice.IsEnabled = false;
            txtedit.IsEnabled = false;
            txtTotalFee.IsEnabled = false;
            txtDiscount.IsEnabled = false;
            txtTotalPay.IsEnabled = false;
            txtPay.IsEnabled = false;
            txtLeft.IsEnabled = false;
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
            if (dgInvoice.SelectedItem is CostDto selectedInvoice)
            {
                txtDay.Text = selectedInvoice.day.ToString("00");
                txtMonth.Text = selectedInvoice.month.ToString("00");
                txtYear.Text = selectedInvoice.year.ToString("0000");
                decimal totalcost = selectedInvoice.Fee;
                txtTotalFee.Text = MoneyConverter.Money(totalcost).ToString();
                decimal discount = selectedInvoice.Discount;
                txtDiscount.Text = MoneyConverter.Money(discount).ToString();
                decimal totalpay = totalcost - discount;
                txtTotalPay.Text = MoneyConverter.Money(totalpay).ToString();
                decimal pay = selectedInvoice.Pay;
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

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic select = dgInvoice.SelectedItem;
            int id = select.Id;
            if (MessageBox.Show("آیا از حذف این رکورد مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Cost>(db);
                    var delete = await context.Delete(id);
                    if (delete)
                    {
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

        private void Title(string title, Cost cost = null)
        {
            var addOrEdit = new AddOrEditCost();
            addOrEdit.TitleCost = title;
            if (cost != null)
            {
                addOrEdit.CostToEdit = cost;
            }
            show.Children.Clear();
            addOrEdit.CostSave = Load;
            show.Children.Add(addOrEdit);
        }

        private async void edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dynamic select = dgInvoice.SelectedItem;
            int id = select.Id;
            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<Cost>(db);
                var edit = await context.GetId(id);
                if (edit == null)
                {
                    MessageBox.Show("شخص مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                show.Visibility = Visibility.Visible;
                Title("2", edit);
                Layout();
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            show.Visibility = Visibility.Visible;
            Layout();
            Title("1");
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
                Check();
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
                    txtedit.Visibility = Visibility.Visible;
                    show1.Visibility = Visibility.Collapsed;
                }
                else
                {
                    add.Visibility = Visibility.Collapsed;
                    edit.Visibility = Visibility.Collapsed;
                    delete.Visibility = Visibility.Collapsed;
                    txtedit.Visibility = Visibility.Collapsed;
                    txtSave.Visibility = Visibility.Collapsed;
                    show1.Visibility = Visibility.Visible;
                }
                dgInvoice.Focus();
                DGLoad();
            }
        }

        private void txtedit_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفا یک آیتم را انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            txtPay.IsReadOnly = false;
            txtSave.Visibility = Visibility.Visible;
            txtedit.Visibility = Visibility.Collapsed;
            decimal pay = decimal.Parse(txtPay.Text.Replace(",", "").Replace(" ریال", "").Trim());
            txtPay.Text = pay.ToString();
            txtPay.Focus();
            txtPay.SelectAll();
        }

        private void txtPay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPay.Text))
            {
                decimal pay = decimal.Parse(txtPay.Text.Replace(",", "").Replace(" ریال", "").Trim());
                if (txtPay.IsReadOnly == false)
                {
                    txtPay.Text = pay.ToString();
                    if (!decimal.TryParse(txtPay.Text.Trim(), out decimal show))
                    {
                        MessageBox.Show("لطفا داخل فیلد فقط از مقادیر عددی استفاده کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    txtShow.Text = MoneyConverter.Money(show);
                }
                else
                {
                    txtShow.Text = string.Empty;
                }
            }
            else
            {
                txtShow.Text = string.Empty;
            }
        }

        private async void txtSave_Click(object sender, RoutedEventArgs e)
        {
            txtSave.Visibility = Visibility.Collapsed;
            txtedit.Visibility = Visibility.Visible;
            txtPay.IsReadOnly = true;
            decimal pay;

            if (!string.IsNullOrEmpty(txtPay.Text))
            {
                pay = decimal.Parse(txtPay.Text.Trim());
            }
            else
            {
                pay = 0;
            }

            dynamic select = dgInvoice.SelectedItem;
            int Id = select.Id;

            try
            {
                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Cost>(db);
                    var update = await context.GetId(Id);

                    update.Pay = pay;

                    var confirm = await context.Update(Id, update);
                    if (confirm == null)
                    {
                        MessageBox.Show("مقدار مبلغ پرداخت شده اضافه نشد", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Load();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"مقدار داخل باکس را فقط به صورت عددی وارد کنید \n {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void show1_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.ItemsSource == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dynamic select = dgInvoice.SelectedItem;
            int id = select.Id;
            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<Cost>(db);
                var edit = await context.GetId(id);
                if (edit == null)
                {
                    MessageBox.Show("شخص مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                show.Visibility = Visibility.Visible;
                Title("3", edit);
                Layout();
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (show.Visibility == Visibility.Visible)
                return;

            if (txtPay.IsKeyboardFocusWithin)
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
            }
        }

        private void dgInvoice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dgInvoice.Focus();
        }

        private void dgInvoice_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (txtPay.IsKeyboardFocusWithin)
                return;

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
        }

        private void txtPay_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtSave_Click(txtSave, null);
            }
        }
    }
}
