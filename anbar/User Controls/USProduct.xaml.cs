using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using anbar.Data.Models;
using anbar.Data.Services;
using anbar.Tools;
using anbar.Windows;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for USProduct.xaml
    /// </summary>
    public partial class USProduct : UserControl
    {
        private int lastSelectedIndex = -1;

        public USProduct()
        {
            InitializeComponent();
            Load();
        }

        public class NaturalSortComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == y) return 0;
                if (string.IsNullOrEmpty(x)) return -1;
                if (string.IsNullOrEmpty(y)) return 1;

                int ix = 0, iy = 0;

                while (ix < x.Length && iy < y.Length)
                {
                    if (char.IsDigit(x[ix]) && char.IsDigit(y[iy]))
                    {
                        string numX = string.Empty;
                        string numY = string.Empty;

                        while (ix < x.Length && char.IsDigit(x[ix]))
                            numX += x[ix++];

                        while (iy < y.Length && char.IsDigit(y[iy]))
                            numY += y[iy++];

                        int valX = int.Parse(numX);
                        int valY = int.Parse(numY);

                        int cmp = valX.CompareTo(valY);
                        if (cmp != 0)
                            return cmp;
                    }
                    else
                    {
                        int cmp = x[ix].CompareTo(y[iy]);
                        if (cmp != 0)
                            return cmp;

                        ix++;
                        iy++;
                    }
                }

                return x.Length.CompareTo(y.Length);
            }
        }

        private async void Load()
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Product>(db);

                    var product = (await context.GetAll())
                        .OrderBy(p => p.ProductIdByUser, new NaturalSortComparer())
                        .ToList();

                    RefreshDataGrid(product);
                }
                dgInvoice.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بارگزاری داده ها : {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshDataGrid(List<Product> newItems)
        {
            // ذخیره‌ی ایندکس انتخاب‌شده قبلی
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
            if (dgInvoice.SelectedItem is Product selectedInvoice)
            {
                decimal c = selectedInvoice.ProductNumber;
                count.Text = MoneyConverter.Money1(c);
                decimal f = selectedInvoice.ProductPrice;
                fee.Text = MoneyConverter.Money(f);
                detail.Text = selectedInvoice.ProductDetail;
            }
        }

        private void LoadEmptyDG()
        {
            count.Text = string.Empty;
            fee.Text = string.Empty;
            detail.Text = string.Empty;
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
            dgInvoice.IsEnabled = false;
            Add.IsEnabled = false;
            Edit.IsEnabled = false;
            Delete.IsEnabled = false;
            count.IsEnabled = false;
            fee.IsEnabled = false;
            detail.IsEnabled = false;
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic selected = dgInvoice.SelectedItem;
            int Id = selected.ProductId;

            if (MessageBox.Show("آیا از حذف این رکورد مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Product>(db);
                    var success = await context.Delete(Id);
                    if (success)
                    {
                        Load();
                    }
                    else
                    {
                        MessageBox.Show("خطا در حذف رکورد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در حذف رکورد : {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowProductList(string title, Product product = null)
        {
            var addOrEdit = new AddOrEditProduct();
            addOrEdit.TitleProduct = title;

            if (product != null)
            {
                addOrEdit.ProductToEdit = product;
            }

            ProductGrid.Children.Clear();
            addOrEdit.ProductSave = Load;
            ProductGrid.Children.Add(addOrEdit);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ProductGrid.Visibility = Visibility.Visible;
            ShowProductList("افزودن محصول جدید");
            Layout();
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                ProductGrid.Visibility = Visibility.Collapsed;
                MessageBox.Show("لطفا یک رکورد را انتخاب کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                dynamic selected = dgInvoice.SelectedItem;
                int Id = selected.ProductId;

                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Product>(db);
                    var productToEdit = await context.GetId(Id);

                    if (productToEdit == null)
                    {
                        MessageBox.Show("محصول مورد نظر یافت نشد", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    ProductGrid.Visibility = Visibility.Visible;
                    ShowProductList("ویرایش مشخصات محصول", productToEdit);
                }
                Layout();
            }
        }

        private void dgInvoice_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                Delete_Click(Delete, null);
            }
            if (e.Key == Key.A)
            {
                Add_Click(Add, null);
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Edit_Click(Edit, null);
            }
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
            dgInvoice.Focus();
            DGLoad();
        }

        private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (ProductGrid.Visibility == Visibility.Visible)
                return;

            if (!dgInvoice.IsKeyboardFocusWithin & !search.Focusable)
            {
                if (e.Key == Key.D)
                {
                    Delete_Click(Delete, null);
                }
                if (e.Key == Key.A)
                {
                    Add_Click(Add, null);
                }
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    Edit_Click(Edit, null);
                }
            }
        }

        private void dgInvoice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dgInvoice.Focus();
        }

        private async void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            {
                var searchText = search.Text.Trim();

                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Product>(db);
                     var result = await context.SearchProducts(searchText);
                    dgInvoice.ItemsSource = result;
                }
            }
        }
    }
}
