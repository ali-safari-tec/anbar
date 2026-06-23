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
    /// Interaction logic for USPerson.xaml
    /// </summary>
    public partial class USPerson : UserControl
    {
        private int lastSelectedIndex = -1;

        public USPerson()
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
            dgInvoice.IsEnabled = false;
            add.IsEnabled = false;
            edit.IsEnabled = false;
            delete.IsEnabled = false;
            type.IsEnabled = false;
            city.IsEnabled = false;
            province.IsEnabled = false;
            address.IsEnabled = false;
            detail.IsEnabled = false;
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
                using (var dbContext = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Person>(dbContext);
                    var people = (await context.GetAll())
                        .OrderBy(c => c.PersonIdByUser, new NaturalSortComparer())                             
                        .ToList();

                    RefreshDataGrid(people);
                }
                dgInvoice.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بارگذاری داده‌ها: {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshDataGrid(List<Person> newItems)
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
            if (dgInvoice.SelectedItem is Person selectedInvoice)
            {
                type.Text = selectedInvoice.PersonModel;
                city.Text = selectedInvoice.PersonCity;
                province.Text = selectedInvoice.PersonProvince;
                phone.Text = selectedInvoice.PersonPhone;
                phoneCode.Text = selectedInvoice.PersonPhoneCode;
                mobile.Text = selectedInvoice.PersonMobile;
                address.Text = selectedInvoice.PersonAddress;
                detail.Text = selectedInvoice.PersonDetail;
            }
        }

        private void LoadEmptyDG()
        {
            type.Text = string.Empty;
            city.Text = string.Empty;
            province.Text = string.Empty;
            phone.Text = string.Empty;
            phoneCode.Text = string.Empty;
            mobile.Text = string.Empty;
            address.Text = string.Empty;
            detail.Text = string.Empty;
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoice.SelectedItem == null)
            {
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic selectedItem = dgInvoice.SelectedItem;
            int id = selectedItem.PersonId;

            if (MessageBox.Show("آیا از حذف این رکورد مطمئن هستید؟", "تأیید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            try
            {
                using (var dbContext = new DataBaseContext())
                {
                    var repository = new GenericDataBaseServices<Person>(dbContext);

                    bool success = await repository.Delete(id);
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
                MessageBox.Show($"خطا در حذف رکورد: {ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowPersonList(string title, Person person = null)
        {

            var addOrEdit = new AddOrEditPerson();
            addOrEdit.TitlePerson = title;

            if (person != null)
            {
                addOrEdit.PersonToEdit = person;
            }

            show.Children.Clear();
            addOrEdit.PersonSave = Load;
            show.Children.Add(addOrEdit);
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {

            show.Visibility = Visibility.Visible;
            ShowPersonList("اضافه کردن شخص جدید");
            Layout();
        }

        private async void edit_Click(object sender, RoutedEventArgs e)
        {

            if (dgInvoice.SelectedItem == null)
            {
                show.Visibility = Visibility.Collapsed;
                MessageBox.Show("لطفاً یک رکورد را انتخاب کنید.", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                dynamic selectedItem = dgInvoice.SelectedItem;
                int selectedPersonId = selectedItem.PersonId;

                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Person>(db);
                    var personToEdit = await context.GetId(selectedPersonId);

                    if (personToEdit == null)
                    {
                        MessageBox.Show("شخص مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    show.Visibility = Visibility.Visible;
                    ShowPersonList("ویرایش مشخصات شخص", personToEdit);
                }
                Layout();
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
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (show.Visibility == Visibility.Visible)
                return;

            if (!dgInvoice.IsKeyboardFocusWithin & !search.Focusable)
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

        private async void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = search.Text.Trim();

            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<Person>(db);
                var result = await context.SearchPeople(searchText);
                dgInvoice.ItemsSource = result;
            }
        }
    }
}
