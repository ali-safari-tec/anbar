using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using anbar.Data.Models;
using anbar.Data.Services;
using anbar.Tools;
using anbar.Windows;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for UCPreSellForm.xaml
    /// </summary>
    public partial class UCPreSellForm : UserControl
    {
        private List<(int ProductId, int InitialCount)> backupItems = new List<(int, int)>();
        public ObservableCollection<Person> people { get; set; }
        public ObservableCollection<Product> products { get; set; }
        public ObservableCollection<PreInvoiceItem> invoiceItems { get; set; }
        public ObservableCollection<Detail> details { get; set; }
        public ObservableCollection<Detail2> details2 { get; set; }
        private readonly GenericDataBaseServices<Person> _personService;
        private readonly GenericDataBaseServices<Product> _productService;
        private readonly GenericDataBaseServices<Detail> _detailService;
        private readonly GenericDataBaseServices<Detail2> _detail2Service;
        private int lastSelectedIndex = -1;
        private bool isSaved = false;
        private int saveNumber = 0;

        public static readonly DependencyProperty TitlePreInvoiceProperty = DependencyProperty.Register("TitlePreInvoice", typeof(string), typeof(UCPreSellForm), new PropertyMetadata(""));

        public string TitlePreInvoice
        {
            get { return (string)GetValue(TitlePreInvoiceProperty); }
            set { SetValue(TitlePreInvoiceProperty, value); }
        }
        public Action PreInvoiceSave { get; set; }
        public PreInvoice PreInvoiceToEdit { get; set; }

        public UCPreSellForm()
        {
            InitializeComponent();
            _personService = new GenericDataBaseServices<Person>(new DataBaseContext());
            _productService = new GenericDataBaseServices<Product>(new DataBaseContext());
            _detailService = new GenericDataBaseServices<Detail>(new DataBaseContext());
            _detail2Service = new GenericDataBaseServices<Detail2>(new DataBaseContext());
            products = new ObservableCollection<Product>();
            invoiceItems = new ObservableCollection<PreInvoiceItem>();
            people = new ObservableCollection<Person>();
            details = new ObservableCollection<Detail>();
            details2 = new ObservableCollection<Detail2>();
            DataContext = this;
        }

        private void EditLoaded(object sender, RoutedEventArgs e)
        {
            if (PreInvoiceToEdit != null)
            {

                txtinvoiceId.Text = PreInvoiceToEdit.ChangeId.ToString("D5");
                txtNameS.Text = PreInvoiceToEdit.PreInvoiceSeller;
                txtNameB.Text = PreInvoiceToEdit.PreInvoiceBuyer;
                txtAddress.Text = PreInvoiceToEdit.PreInvoiceAddress;
                txtPhone.Text = PreInvoiceToEdit.PreInvoicePhone;
                txtPhoneCode.Text = PreInvoiceToEdit.PreInvoicePhoneCode;
                txtDay.Text = PreInvoiceToEdit.PreDateDay.ToString("00");
                txtMonth.Text = PreInvoiceToEdit.PreDateMonth.ToString("00");
                txtYear.Text = PreInvoiceToEdit.PreDateYear.ToString();
                txtDayS.Text = PreInvoiceToEdit.PreDateDayS.ToString("00");
                txtMonthS.Text = PreInvoiceToEdit.PreDateMonthS.ToString("00");
                txtYearS.Text = PreInvoiceToEdit.PreDateYearS.ToString();
                decimal total = PreInvoiceToEdit.InvoiceTotalCost;
                txtfinalPrice.Text = MoneyConverter.Money(total);
                decimal discount = PreInvoiceToEdit.InvoiceDiscount;
                txtdiscount.Text = MoneyConverter.Money(discount);
                txtfinalPay.Text = MoneyConverter.Money(total - discount);
                txtdetail.Text = PreInvoiceToEdit.Detail;
                txtdetail2.Text = PreInvoiceToEdit.Detail2;

                using (var db = new DataBaseContext())
                {
                    var invoiceItemsFromDb = db.preInvoiceItems
                                               .Where(i => i.PreInvoiceId == PreInvoiceToEdit.PreInvoiceId) // فقط آیتم‌های این فاکتور
                                               .ToList();

                    invoiceItems = new ObservableCollection<PreInvoiceItem>(invoiceItemsFromDb);
                }

                // مقداردهی DataGrid برای نمایش آیتم‌ها
                myDataGrid.ItemsSource = invoiceItems;
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

        private void CloseControl()
        {
            var parent = this.Parent as Grid;
            if (parent != null)
            {
                var mainWin = FindParent<MainWindow>(this);
                if (mainWin != null)
                {
                    mainWin.btnHome.IsEnabled = true;
                    mainWin.btnMenu.IsEnabled = true;
                    mainWin.Introduction.IsEnabled = true;
                    mainWin.buyPart.IsEnabled = true;
                    mainWin.sellPart.IsEnabled = true;
                    mainWin.toolsPart.IsEnabled = true;
                }
                parent.Visibility = Visibility.Collapsed;
                parent.Children.Remove(this);
            }
        }

        private void LoadFinalPrice()
        {
            decimal total = invoiceItems.Sum(p => p.InvoiceFinalPrice);
            var price = MoneyConverter.Money(total);
            txtfinalPrice.Text = price;
        }

        private void LoadDiscount() // taghir
        {
            var sum = invoiceItems.Sum(p => p.InvoiceFinalPrice);
            txtfinalPrice.Text = MoneyConverter.Money(sum);
            decimal discount = decimal.Parse(txtdiscount.Text.Replace(",", "").Replace(" ریال", "").Trim());
            var total = sum - discount;
            if (total > 0 && !string.IsNullOrEmpty(txtfinalPrice.Text))
            {
                var price = MoneyConverter.Money(total);
                txtfinalPay.Text = price;
            }
            else
            {
                txtfinalPay.Text = MoneyConverter.Money(0);
            }
        }

        private async void LoadPeople()
        {
            var peopleList = await _personService.GetSelectedPeople();

            if (peopleList != null)
            {
                people.Clear();
                foreach (var dto in peopleList)
                {
                    people.Add(new Person
                    {
                        PersonIdByUser = dto.PersonIdByUser,
                        PersonName = dto.PersonName,
                        PersonPhone = dto.PersonPhone,
                        PersonPhoneCode = dto.PersonPhoneCode,
                        PersonAddress = dto.PersonAddress
                    });
                }
            }
        }

        private void sellerList_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sellerList.Text == "جستجو...")
            {
                sellerList.Text = string.Empty;
                sellerList.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void sellerList_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(sellerList.Text))
            {
                sellerList.Foreground = new SolidColorBrush(Colors.LightGray);
                sellerList.Text = "جستجو...";
            }
        }

        private void dropSeller_Click(object sender, RoutedEventArgs e)
        {
            buyp.Visibility = Visibility.Collapsed;
            addGrid.Visibility = Visibility.Collapsed;
            bDetail.Visibility = Visibility.Collapsed;
            bDetail2.Visibility = Visibility.Collapsed;
            sellp.Visibility = (sellp.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
            LoadPeople();
            if (sGrid.Items.Count > 0)
            {
                if (sGrid.SelectedIndex >= 0)
                {
                    lastSelectedIndex = sGrid.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                }

                // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                Dispatcher.InvokeAsync(() =>
                {
                    // بررسی ایندکس قبلی و تنظیم آن
                    if (lastSelectedIndex >= 0 && lastSelectedIndex < sGrid.Items.Count)
                    {
                        // دوباره سلکت کردن ایندکس قبلی
                        sGrid.SelectedIndex = lastSelectedIndex;
                    }
                    else
                    {
                        // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                        sGrid.SelectedIndex = 0;
                    }

                    // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                    sGrid.CurrentCell = new DataGridCellInfo(sGrid.SelectedItem, sGrid.Columns[0]);
                    sGrid.Focus(); // فوکوس روی دیتاگرید

                    // انتقال فوکوس به سلکت شده جدید
                    sGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        private void buyerList_GotFocus(object sender, RoutedEventArgs e)
        {
            if (buyerList.Text == "جستجو...")
            {
                buyerList.Text = string.Empty;
                buyerList.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void buyerList_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(buyerList.Text))
            {
                buyerList.Foreground = new SolidColorBrush(Colors.LightGray);
                buyerList.Text = "جستجو...";
            }
        }

        private void dropBuyer_Click(object sender, RoutedEventArgs e)
        {
            sellp.Visibility = Visibility.Collapsed;
            addGrid.Visibility = Visibility.Collapsed;
            bDetail.Visibility = Visibility.Collapsed;
            bDetail2.Visibility = Visibility.Collapsed;
            buyp.Visibility = (buyp.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
            LoadPeople();
            if (bGrid.Items.Count > 0)
            {
                if (bGrid.SelectedIndex >= 0)
                {
                    lastSelectedIndex = bGrid.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                }

                // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                Dispatcher.InvokeAsync(() =>
                {
                    // بررسی ایندکس قبلی و تنظیم آن
                    if (lastSelectedIndex >= 0 && lastSelectedIndex < bGrid.Items.Count)
                    {
                        // دوباره سلکت کردن ایندکس قبلی
                        bGrid.SelectedIndex = lastSelectedIndex;
                    }
                    else
                    {
                        // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                        bGrid.SelectedIndex = 0;
                    }

                    // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                    bGrid.CurrentCell = new DataGridCellInfo(bGrid.SelectedItem, bGrid.Columns[0]);
                    bGrid.Focus(); // فوکوس روی دیتاگرید

                    // انتقال فوکوس به سلکت شده جدید
                    bGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        private void ShowSelectedPerson()
        {
            if (bGrid.SelectedItem is Person selectedPerson)
            {
                txtNameB.Text = selectedPerson.PersonName;
                txtPhone.Text = selectedPerson.PersonPhone;
                txtPhoneCode.Text = selectedPerson.PersonPhoneCode;
                txtAddress.Text = selectedPerson.PersonAddress;
            }
        }

        private void sGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sGrid.SelectedItem is Person selectedPerson)
            {
                txtNameS.Text = selectedPerson.PersonName;
            }
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T t)
                    return t;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void sGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                sellp.Visibility = Visibility.Collapsed;
                txtNameS.Text = string.Empty;
            }
            if (e.Key == Key.Enter)
            {
                if (sGrid.SelectedItem is Person selectedPerson)
                {
                    txtNameS.Text = selectedPerson.PersonName;
                    e.Handled = true;
                    sellp.Visibility = Visibility.Collapsed;
                }
            }
            if (e.Key == Key.Up)
            {
                if (sGrid.SelectedIndex == 0)
                {
                    e.Handled = true;
                    sellerList.Focus();
                    sellerList.SelectAll();
                }
            }
            if (e.Key == Key.End)
            {
                e.Handled = true;

                if (sGrid.Items.Count > 0)
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        int lastIndex = sGrid.Items.Count - 1;

                        // انتخاب آخرین ردیف
                        sGrid.SelectedIndex = lastIndex;
                        sGrid.CurrentCell = new DataGridCellInfo(sGrid.Items[lastIndex], sGrid.Columns[0]);
                        sGrid.ScrollIntoView(sGrid.Items[lastIndex]);

                        // اعمال فوکوس به سلول خاص در آخرین ردیف
                        DataGridRow row = (DataGridRow)sGrid.ItemContainerGenerator.ContainerFromIndex(lastIndex);
                        if (row != null)
                        {
                            DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
                            if (presenter != null)
                            {
                                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(0);
                                if (cell != null)
                                {
                                    cell.Focus(); // فوکوس واقعی به سلول
                                }
                            }
                        }
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
        }

        private void bGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedPerson();
        }

        private void bGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                buyp.Visibility = Visibility.Collapsed;
                txtNameB.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtPhoneCode.Text = string.Empty;
                txtAddress.Text = string.Empty;
            }
            if (e.Key == Key.Enter)
            {
                ShowSelectedPerson();
                e.Handled = true;
                buyp.Visibility = Visibility.Collapsed;
            }
            if (e.Key == Key.Up)
            {
                if (bGrid.SelectedIndex == 0)
                {
                    buyerList.Focus();
                    buyerList.SelectAll();
                }
            }
            if (e.Key == Key.End)
            {
                e.Handled = true;

                if (bGrid.Items.Count > 0)
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        int lastIndex = bGrid.Items.Count - 1;

                        // انتخاب آخرین ردیف
                        bGrid.SelectedIndex = lastIndex;
                        bGrid.CurrentCell = new DataGridCellInfo(bGrid.Items[lastIndex], bGrid.Columns[0]);
                        bGrid.ScrollIntoView(bGrid.Items[lastIndex]);

                        // اعمال فوکوس به سلول خاص در آخرین ردیف
                        DataGridRow row = (DataGridRow)bGrid.ItemContainerGenerator.ContainerFromIndex(lastIndex);
                        if (row != null)
                        {
                            DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
                            if (presenter != null)
                            {
                                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(0);
                                if (cell != null)
                                {
                                    cell.Focus(); // فوکوس واقعی به سلول
                                }
                            }
                        }
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
        }

        private async void sellerList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sellp.Visibility == Visibility.Visible)
            {
                var searchText = sellerList.Text.Trim();

                var context = await _personService.SearchPeople(searchText);
                sGrid.ItemsSource = context;
            }
        }

        private async void buyerList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (buyp.Visibility == Visibility.Visible)
            {
                var searchText = buyerList.Text.Trim();

                var context = await _personService.SearchPeople(searchText);
                bGrid.ItemsSource = context;
            }
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

        private async void LoadProduct()
        {
            var peopleList = (await _productService.GetAll()).OrderBy(c => c.ProductIdByUser, new NaturalSortComparer()).ToList();

            if (peopleList != null)
            {
                products.Clear();
                foreach (var dto in peopleList)
                {
                    products.Add(new Product
                    {
                        ProductId = dto.ProductId,
                        ProductIdByUser = dto.ProductIdByUser,
                        ProductName = dto.ProductName,
                        ProductNumber = dto.ProductNumber,
                        ProductPrice = dto.ProductPrice
                    });
                }
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            buyp.Visibility = Visibility.Collapsed;
            sellp.Visibility = Visibility.Collapsed;
            bDetail.Visibility = Visibility.Collapsed;
            bDetail2.Visibility = Visibility.Collapsed;
            editB.Visibility = Visibility.Collapsed;
            addGrid.Visibility = (addGrid.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;

            LoadProduct();
            productCode.Focus();
        }

        private void productDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productDG.SelectedItem is Product selectedPerson)
            {
                productCode.Text = selectedPerson.ProductIdByUser;
                productName.Text = selectedPerson.ProductName;
                productNumber.Text = selectedPerson.ProductNumber.ToString();
                productPrice.Text = selectedPerson.ProductPrice.ToString();
            }
        }

        private void productDG_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                addGrid.Visibility = Visibility.Collapsed;
            }
            if (e.Key == Key.Enter)
            {
                if (productDG.SelectedItem is Product selectedPerson)
                {
                    productCode.Text = selectedPerson.ProductIdByUser;
                    productName.Text = selectedPerson.ProductName;
                    productNumber.Text = selectedPerson.ProductNumber.ToString();
                    productPrice.Text = selectedPerson.ProductPrice.ToString();
                    productCode.Focus();
                    productCode.SelectAll();
                }
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                if (productDG.SelectedIndex == 0)
                {
                    e.Handled = true;
                    searchProduct.Focus();
                    searchProduct.SelectAll();
                }
            }
            if (e.Key == Key.End)
            {
                e.Handled = true;

                if (productDG.Items.Count > 0)
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        int lastIndex = productDG.Items.Count - 1;

                        // انتخاب آخرین ردیف
                        productDG.SelectedIndex = lastIndex;
                        productDG.CurrentCell = new DataGridCellInfo(productDG.Items[lastIndex], productDG.Columns[0]);
                        productDG.ScrollIntoView(productDG.Items[lastIndex]);

                        // اعمال فوکوس به سلول خاص در آخرین ردیف
                        DataGridRow row = (DataGridRow)productDG.ItemContainerGenerator.ContainerFromIndex(lastIndex);
                        if (row != null)
                        {
                            DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
                            if (presenter != null)
                            {
                                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(0);
                                if (cell != null)
                                {
                                    cell.Focus(); // فوکوس واقعی به سلول
                                }
                            }
                        }
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
            if (e.Key == Key.Home)
            {
                e.Handled = true;

                if (productDG.Items.Count > 0)
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        int firstIndext = 0;

                        // انتخاب آخرین ردیف
                        productDG.SelectedIndex = firstIndext;
                        productDG.CurrentCell = new DataGridCellInfo(productDG.Items[firstIndext], productDG.Columns[0]);
                        productDG.ScrollIntoView(productDG.Items[firstIndext]);

                        // اعمال فوکوس به سلول خاص در آخرین ردیف
                        DataGridRow row = (DataGridRow)productDG.ItemContainerGenerator.ContainerFromIndex(firstIndext);
                        if (row != null)
                        {
                            DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
                            if (presenter != null)
                            {
                                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(0);
                                if (cell != null)
                                {
                                    cell.Focus(); // فوکوس واقعی به سلول
                                }
                            }
                        }
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
        }

        private void searchProduct_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchProduct.Text == "جستجو...")
            {
                searchProduct.Text = string.Empty;
                searchProduct.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void searchProduct_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchProduct.Text))
            {
                searchProduct.Text = "جستجو...";
                searchProduct.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            addGrid.Visibility = Visibility.Collapsed;
            productCode.Text = string.Empty;
            productName.Text = string.Empty;
            productNumber.Text = string.Empty;
            productPrice.Text = string.Empty;
        }

        private async void addbtn_Click(object sender, RoutedEventArgs e) // kheily mohem baray test @@@@@@
        {
            if (string.IsNullOrWhiteSpace(productCode.Text) || string.IsNullOrWhiteSpace(productName.Text) ||
        string.IsNullOrWhiteSpace(productNumber.Text) || string.IsNullOrWhiteSpace(productPrice.Text))
            {
                MessageBox.Show("لطفاً تمام فیلدها را پر کنید.", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(productNumber.Text, out int quantity) || !decimal.TryParse(productPrice.Text, out decimal price))
            {
                MessageBox.Show("تعداد و قیمت باید عدد باشند.", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (productDG.SelectedItem != null)
            {
                dynamic select = productDG.SelectedItem;
                int Id = select.ProductId;

                var products = await _productService.GetId(Id);
                if (products == null)
                {
                    if (products == null)
                    {
                        MessageBox.Show($"محصول با کد '{Id}' یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                if (productCode.Text != products.ProductIdByUser || productName.Text != products.ProductName)
                {
                    MessageBox.Show("محصول با کد و یا نام وارد شدا پیدا نشد لطفا ابتدا محصول را معرفی کنید و سپس وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // بررسی می‌کنیم که آیا این محصول قبلاً در لیست بکاپ ذخیره شده است یا نه
                if (!backupItems.Any(x => x.ProductId == products.ProductId))
                {
                    backupItems.Add((products.ProductId, products.ProductNumber)); // ذخیره مقدار اولیه محصول
                }

                if (products.ProductNumber <= quantity)
                {
                    MessageBoxResult result = MessageBox.Show(
                        $"موجودی این محصول فقط {products.ProductNumber} عدد است. آیا می‌خواهید کل موجودی را به فاکتور اضافه کنید؟",
                        "تایید",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    else
                    {
                        products.ProductNumber = 0;
                        await _productService.Update(Id, products);
                    }
                }
                else
                {
                    products.ProductNumber -= quantity;
                    await _productService.Update(Id, products);
                }

                decimal total = quantity * price;

                // ایجاد یک محصول جدید
                PreInvoiceItem newProduct = new PreInvoiceItem
                {
                    ProductId = products.ProductId,
                    ProductCode = productCode.Text,
                    ProductName = productName.Text,
                    InvoiceItemNumber = quantity,
                    InvoiceItemPrice = price,
                    InvoiceItemPriceS = MoneyConverter.Money(price),
                    InvoiceFinalPrice = total,
                    InvoiceFinalPriceS = MoneyConverter.Money(total)
                };

                invoiceItems.Add(newProduct); // اضافه کردن محصول جدید به لیست

                myDataGrid.ItemsSource = null; // ریست کردن دیتاگرید برای بروزرسانی
                myDataGrid.ItemsSource = invoiceItems; // نمایش لیست به‌روزرسانی‌شده در دیتاگرید


                addGrid.Visibility = Visibility.Collapsed; //
                LoadFinalPrice();
                if (chk.IsChecked == true)
                {
                    LoadDarsad();
                }
                else
                {
                    LoadDiscount();
                }
            }
            else
            {
                decimal total = quantity * price;

                // ایجاد یک محصول جدید
                PreInvoiceItem newProduct = new PreInvoiceItem
                {
                    
                    ProductCode = productCode.Text,
                    ProductName = productName.Text,
                    InvoiceItemNumber = quantity,
                    InvoiceItemPrice = price,
                    InvoiceItemPriceS = MoneyConverter.Money(price),
                    InvoiceFinalPrice = total,
                    InvoiceFinalPriceS = MoneyConverter.Money(total)
                };

                invoiceItems.Add(newProduct); // اضافه کردن محصول جدید به لیست

                myDataGrid.ItemsSource = null; // ریست کردن دیتاگرید برای بروزرسانی
                myDataGrid.ItemsSource = invoiceItems; // نمایش لیست به‌روزرسانی‌شده در دیتاگرید


                addGrid.Visibility = Visibility.Collapsed; //
                productCode.Text = string.Empty;
                productName.Text = string.Empty;
                productNumber.Text = string.Empty;
                productPrice.Text = string.Empty;
                LoadFinalPrice();
                if (chk.IsChecked == true)
                {
                    LoadDarsad();
                }
                else
                {
                    LoadDiscount();
                }
            }
        }

        private void txtdiscount_GotFocus(object sender, RoutedEventArgs e)
        {
            txtdiscount.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#336784B3"));
            if (string.IsNullOrEmpty(txtdiscount.Text))
            {
                txtdiscount.Text = "0";
            }
            decimal discount = decimal.Parse(txtdiscount.Text.Replace(",", "").Replace(" ریال", "").Trim());
            txtdiscount.Text = discount.ToString();
            txtdiscount.IsReadOnly = false;
        }

        private void txtdiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            txtdiscount.Background = new SolidColorBrush(Colors.Transparent);
            if (string.IsNullOrEmpty(txtdiscount.Text))
            {
                txtdiscount.Text = MoneyConverter.Money(0);
            }
            decimal discount = decimal.Parse(txtdiscount.Text.Replace(",", "").Replace(" ریال", "").Trim());
            txtdiscount.Text = MoneyConverter.Money(discount);
            LoadDiscount();
            txtdiscount.IsReadOnly = true;
        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            txtdiscount.Text = string.Empty;
            txtdiscount.IsReadOnly = true;
            txtdiscount.Focusable = false;
            darsad.IsReadOnly = false;
            darsadb.Visibility = Visibility.Visible;
            darsad.Visibility = Visibility.Visible;
            darsad.Focus();
            darsad.SelectAll();
            darsadl.Visibility = Visibility.Visible;
        }

        private void chk_Unchecked(object sender, RoutedEventArgs e)
        {
            txtdiscount.IsReadOnly = false;
            txtdiscount.Focusable = true;
            darsadb.Visibility = Visibility.Collapsed;
            darsad.Visibility = Visibility.Collapsed;
            darsadl.Visibility = Visibility.Collapsed;
        }

        private void LoadDarsad()
        {
            if (string.IsNullOrEmpty(darsad.Text))
            {
                darsad.Text = "0";
                darsad.SelectAll();
            }
            if (double.Parse(darsad.Text) > 100)
            {
                darsad.Text = "100";
                darsad.SelectAll();
                MessageBox.Show("لطفا عددی کمتر از 100 انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (double.Parse(darsad.Text) < 0)
            {
                darsad.Text = "0";
                darsad.SelectAll();
                MessageBox.Show("لطفا عددی بیشتر از 0 انتخاب کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (invoiceItems != null)
            {
                var sum = invoiceItems.Sum(p => p.InvoiceFinalPrice);
                txtfinalPrice.Text = MoneyConverter.Money(sum);
                int discount = int.Parse(darsad.Text.Trim());
                decimal discountShow = ((sum * discount) / 100);
                decimal total = sum - discountShow;

                txtdiscount.Text = MoneyConverter.Money(discountShow);
                if (total > 0 && !string.IsNullOrEmpty(txtfinalPrice.Text))
                {
                    var price = MoneyConverter.Money(total);
                    txtfinalPay.Text = price;
                }
                else
                {
                    txtfinalPay.Text = MoneyConverter.Money(0);
                }
            }
        }

        private void darsad_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadDarsad();
        }

        private void darsad_GotFocus(object sender, RoutedEventArgs e)
        {
            darsad.SelectAll();
            LoadDarsad();
        }

        private async void del_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = myDataGrid.SelectedItem as PreInvoiceItem;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show(
                   "آیا از حذف این آیتم مطمئن هستید؟",
                   "حذف آیتم",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    int productId = selectedItem.ProductId;

                    if (productId > 0)
                    {
                        var query = await _productService.GetId(productId);
                        if (query != null) // چک کردن اینکه آیا query null است یا خیر
                        {
                            var get = selectedItem.InvoiceItemNumber;
                            query.ProductNumber = query.ProductNumber + get;
                            await _productService.Update(productId, query);
                        }
                        else
                        {
                            MessageBox.Show("محصول مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    invoiceItems.Remove(selectedItem); // حذف از لیست نمایش

                    myDataGrid.ItemsSource = null;
                    myDataGrid.ItemsSource = invoiceItems; // رفرش دیتاگرید
                }
            }
            LoadFinalPrice();
            if (chk.IsChecked == true)
            {
                LoadDarsad();
            }
            else
            {
                LoadDiscount();
            }
        }

        private void productCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                addGrid.Visibility = Visibility.Collapsed;
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                productName.Focus();
                productName.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                productName.Focus();
                productName.SelectAll();
            }
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                if (productDG.Items.Count > 0)
                {
                    if (productDG.SelectedIndex >= 0)
                    {
                        lastSelectedIndex = productDG.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                    }

                    // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                    Dispatcher.InvokeAsync(() =>
                    {
                        // بررسی ایندکس قبلی و تنظیم آن
                        if (lastSelectedIndex >= 0 && lastSelectedIndex < productDG.Items.Count)
                        {
                            // دوباره سلکت کردن ایندکس قبلی
                            productDG.SelectedIndex = lastSelectedIndex;
                        }
                        else
                        {
                            // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                            productDG.SelectedIndex = 0;
                        }

                        // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                        productDG.CurrentCell = new DataGridCellInfo(productDG.SelectedItem, productDG.Columns[0]);
                        productDG.Focus(); // فوکوس روی دیتاگرید

                        // انتقال فوکوس به سلکت شده جدید
                        productDG.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
        }

        private void productName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                addGrid.Visibility = Visibility.Collapsed;
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                productNumber.Focus();
                productNumber.Text = string.Empty;
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                productNumber.Focus();
                productNumber.Text = string.Empty;
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                productCode.Focus();
                productCode.SelectAll();
            }
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                if (productDG.Items.Count > 0)
                {
                    if (productDG.SelectedIndex >= 0)
                    {
                        lastSelectedIndex = productDG.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                    }

                    // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                    Dispatcher.InvokeAsync(() =>
                    {
                        // بررسی ایندکس قبلی و تنظیم آن
                        if (lastSelectedIndex >= 0 && lastSelectedIndex < productDG.Items.Count)
                        {
                            // دوباره سلکت کردن ایندکس قبلی
                            productDG.SelectedIndex = lastSelectedIndex;
                        }
                        else
                        {
                            // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                            productDG.SelectedIndex = 0;
                        }

                        // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                        productDG.CurrentCell = new DataGridCellInfo(productDG.SelectedItem, productDG.Columns[0]);
                        productDG.Focus(); // فوکوس روی دیتاگرید

                        // انتقال فوکوس به سلکت شده جدید
                        productDG.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
        }

        private void productNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                addGrid.Visibility = Visibility.Collapsed;
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                productPrice.Focus();
                productPrice.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                productPrice.Focus();
                productPrice.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                productName.Focus();
                productName.SelectAll();
            }
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                if (productDG.Items.Count > 0)
                {
                    if (productDG.SelectedIndex >= 0)
                    {
                        lastSelectedIndex = productDG.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                    }

                    // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                    Dispatcher.InvokeAsync(() =>
                    {
                        // بررسی ایندکس قبلی و تنظیم آن
                        if (lastSelectedIndex >= 0 && lastSelectedIndex < productDG.Items.Count)
                        {
                            // دوباره سلکت کردن ایندکس قبلی
                            productDG.SelectedIndex = lastSelectedIndex;
                        }
                        else
                        {
                            // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                            productDG.SelectedIndex = 0;
                        }

                        // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                        productDG.CurrentCell = new DataGridCellInfo(productDG.SelectedItem, productDG.Columns[0]);
                        productDG.Focus(); // فوکوس روی دیتاگرید

                        // انتقال فوکوس به سلکت شده جدید
                        productDG.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
        }

        private void productPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                addbtn_Click(addbtn, null);
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                productNumber.Focus();
                productNumber.SelectAll();
            }
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                cancelbtn_Click(cancelbtn, null);
            }
        }

        private void txtdiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtdiscount_LostFocus(e, null);
            }
        }

        private async void LoadDetail()
        {
            var detail = await _detailService.GetAll();
            dbdetail.ItemsSource = detail;
        }

        private void dropDetail_Click(object sender, RoutedEventArgs e)
        {
            buyp.Visibility = Visibility.Collapsed;
            sellp.Visibility = Visibility.Collapsed;
            addGrid.Visibility = Visibility.Collapsed;
            bDetail2.Visibility = Visibility.Collapsed;
            bDetail.Visibility = (bDetail.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            LoadDetail();
        }

        private void dbdetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (dbdetail.SelectedItem is Detail selectedPerson)
                {
                    txtdetail.Text = selectedPerson.InvoiceDetail;
                }
            }
        }

        private void dbdetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbdetail.SelectedItem is Detail selectedPerson)
            {
                txtdetail.Text = selectedPerson.InvoiceDetail;
            }
        }

        private async void addDetaildb_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtdtailAddorSearch.Text))
            {
                MessageBox.Show("لطفا متن مورد نظر خود را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                try
                {
                    string detail = txtdtailAddorSearch.Text.Trim();
                    Detail query = new Detail
                    {
                        InvoiceDetail = detail
                    };
                    var add = await _detailService.Create(query);
                    LoadDetail();
                    txtdtailAddorSearch.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void delDetaildb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic selectedItem = dbdetail.SelectedItem;
                int id = selectedItem.Id;

                bool success = await _detailService.Delete(id);
                if (success)
                {
                    MessageBox.Show($"متن ( {selectedItem.InvoiceDetail} ) با موفقیت خذف شد", "حذف", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDetail();
                }
                else
                {
                    MessageBox.Show("خطا در حذف متن مورد نظر.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HideButtons()
        {
            borderId.Visibility = Visibility.Collapsed;
            borderSeller.Visibility = Visibility.Collapsed;
            borderBuyer.Visibility = Visibility.Collapsed;
            borderPhone.Visibility = Visibility.Collapsed;
            borderPhoneCode.Visibility = Visibility.Collapsed;
            borderAddress.Visibility = Visibility.Collapsed;
            borderDay.Visibility = Visibility.Collapsed;
            borderMonth.Visibility = Visibility.Collapsed;
            borderYear.Visibility = Visibility.Collapsed;
            borderDayS.Visibility = Visibility.Collapsed;
            borderMonthS.Visibility = Visibility.Collapsed;
            borderYearS.Visibility = Visibility.Collapsed;
            dropSeller.Visibility = Visibility.Collapsed;
            dropBuyer.Visibility = Visibility.Collapsed;
            chk.Visibility = Visibility.Collapsed;
            if (chk.IsChecked == true)
            {
                darsadl.Visibility = Visibility.Collapsed;
                darsadb.Visibility = Visibility.Collapsed;
                darsad.Visibility = Visibility.Collapsed;
            }
            detailborder.Visibility = Visibility.Collapsed;
            dropDetail.Visibility = Visibility.Collapsed;
            detailborder2.Visibility = Visibility.Collapsed;
            dropDetail2.Visibility = Visibility.Collapsed;
            bDetail2.Visibility = Visibility.Collapsed;
            bDetail.Visibility = Visibility.Collapsed;
        }

        private void ShowButtons()
        {
            borderId.Visibility = Visibility.Visible;
            borderSeller.Visibility = Visibility.Visible;
            borderBuyer.Visibility = Visibility.Visible;
            borderPhone.Visibility = Visibility.Visible;
            borderPhoneCode.Visibility = Visibility.Visible;
            borderAddress.Visibility = Visibility.Visible;
            borderDay.Visibility = Visibility.Visible;
            borderMonth.Visibility = Visibility.Visible;
            borderYear.Visibility = Visibility.Visible;
            borderDayS.Visibility = Visibility.Visible;
            borderMonthS.Visibility = Visibility.Visible;
            borderYearS.Visibility = Visibility.Visible;
            dropSeller.Visibility = Visibility.Visible;
            dropBuyer.Visibility = Visibility.Visible;
            chk.Visibility = Visibility.Visible;
            if (chk.IsChecked == true)
            {
                darsadl.Visibility = Visibility.Visible;
                darsadb.Visibility = Visibility.Visible;
                darsad.Visibility = Visibility.Visible;
            }
            detailborder.Visibility = Visibility.Visible;
            dropDetail.Visibility = Visibility.Visible;
            detailborder2.Visibility = Visibility.Visible;
            dropDetail2.Visibility = Visibility.Visible;
        }

        private async void print_Click(object sender, RoutedEventArgs e)
        {
            myDataGrid.SelectedItem = null;
            myDataGrid.UnselectAll();

            // مخفی کردن دکمه‌ها و سایر المان‌هایی که نمی‌خواهید در پرینت باشند
            HideButtons();

            // صبر برای آپدیت UI
            await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);

            var grid = main; // نام Grid خود را جایگزین کنید

            // اندازه‌های صفحه A4 در پیکسل (96 DPI)
            double a4Width = 816;
            double a4Height = 1056;

            // رندر کردن Grid به تصویر
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                (int)grid.ActualWidth, (int)grid.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(grid);

            // تبدیل RenderTargetBitmap به Image برای نمایش در Print Preview
            Image printImage = new Image
            {
                Source = renderTargetBitmap,
                Width = a4Width,
                Height = a4Height,
                Stretch = Stretch.Uniform
            };

            // ایجاد FixedDocument برای نمایش در Print Preview
            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage
            {
                Width = a4Width,
                Height = a4Height
            };

            // اضافه کردن تصویر به صفحه پرینت
            fixedPage.Children.Add(printImage);
            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            // ایجاد Window جدید برای نمایش پیش‌نمایش چاپ
            Window previewWindow = new Window
            {
                Title = "پیش‌نمایش چاپ",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 1000,
                Height = 700,
                Content = new DocumentViewer { Document = fixedDoc }
            };

            // نمایش پنجره
            previewWindow.ShowDialog();

            // نمایش مجدد دکمه‌ها و عناصر مخفی شده
            ShowButtons();
        }

        private async void LoadDetail2()
        {
            var detail = await _detail2Service.GetAll();
            dbdetail2.ItemsSource = detail;
        }

        private void dbdetail2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (dbdetail2.SelectedItem is Detail2 selectedPerson)
                {
                    txtdetail2.Text = selectedPerson.InvoiceDetail2;
                }
            }
        }

        private void dbdetail2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dbdetail2.SelectedItem is Detail2 selectedPerson)
            {
                txtdetail2.Text = selectedPerson.InvoiceDetail2;
            }
        }

        private async void addDetaildb2_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtdtailAddorSearch2.Text))
            {
                MessageBox.Show("لطفا متن مورد نظر خود را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    string detail = txtdtailAddorSearch2.Text.Trim();
                    Detail2 query = new Detail2
                    {
                        InvoiceDetail2 = detail
                    };
                    var add = await _detail2Service.Create(query);
                    LoadDetail2();
                    txtdtailAddorSearch2.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void delDetaildb2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic selectedItem = dbdetail2.SelectedItem;
                int id = selectedItem.Id;

                bool success = await _detail2Service.Delete(id);
                if (success)
                {
                    MessageBox.Show($"متن ( {selectedItem.InvoiceDetail2} ) با موفقیت خذف شد", "حذف", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDetail2();
                }
                else
                {
                    MessageBox.Show("خطا در حذف متن مورد نظر.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dropDetail2_Click(object sender, RoutedEventArgs e)
        {
            buyp.Visibility = Visibility.Collapsed;
            sellp.Visibility = Visibility.Collapsed;
            addGrid.Visibility = Visibility.Collapsed;
            bDetail.Visibility = Visibility.Collapsed;
            bDetail2.Visibility = (bDetail2.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            LoadDetail2();
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _Id = int.Parse(txtinvoiceId.Text.Trim());
                string _nameS = txtNameS.Text.Trim();
                string _nameB = txtNameB.Text.Trim();
                string _phone = txtPhone.Text.Trim();
                string _phoneCode = txtPhoneCode.Text.Trim();
                string _address = txtAddress.Text.Trim();
                int _day = int.Parse(txtDay.Text.Trim());
                int _month = int.Parse(txtMonth.Text.Trim());
                int _year = int.Parse(txtYear.Text.Trim());
                int _dayS = int.Parse(txtDayS.Text.Trim());
                int _monthS = int.Parse(txtMonthS.Text.Trim());
                int _yearS = int.Parse(txtYearS.Text.Trim());
                decimal _total = decimal.Parse(txtfinalPrice.Text.Replace(",", "").Replace(" ریال", "").Trim());
                decimal _discount = decimal.Parse(txtdiscount.Text.Replace(",", "").Replace(" ریال", "").Trim());
                string _detail = txtdetail.Text.Trim();
                string _detail2 = txtdetail2.Text.Trim();
                

                if (string.IsNullOrEmpty(_Id.ToString()) || string.IsNullOrEmpty(_nameB) || string.IsNullOrEmpty(_nameS) ||
                    string.IsNullOrEmpty(_day.ToString()) || string.IsNullOrEmpty(_month.ToString()) || string.IsNullOrEmpty(_year.ToString()))
                {
                    MessageBox.Show("بخش‌های شماره فاکتور، نام فروشنده، نام خریدار و تاریخ را وارد کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<PreInvoice>(db);
                    int dateYear = db.financials.Select(c => c.LastYear).FirstOrDefault();

                    if (TitlePreInvoice.Equals("2"))
                    {
                        var update = await context.GetId(_Id);

                        if (update == null)
                        {
                            MessageBox.Show("فاکتور مورد نظر پیدا نشد", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        update.PreInvoiceSeller = _nameS;
                        update.PreInvoiceBuyer = _nameB;
                        update.PreInvoicePhone = _phone;
                        update.PreInvoicePhoneCode = _phoneCode;
                        update.PreInvoiceAddress = _address;
                        update.PreDateDay = _day;
                        update.PreDateMonth = _month;
                        update.PreDateYear = _year;
                        update.PreDateDayS = _dayS;
                        update.PreDateMonthS = _monthS;
                        update.PreDateYearS = _yearS;
                        update.InvoiceTotalCost = _total;
                        update.InvoiceDiscount = _discount;
                        update.Detail = _detail;
                        update.Detail2 = _detail2;

                        var oldItems = db.preInvoiceItems.Where(i => i.PreInvoiceId == _Id).ToList();
                        db.preInvoiceItems.RemoveRange(oldItems);
                        await db.SaveChangesAsync(); // تغییرات را در دیتابیس ذخیره کن

                        // حالا آیتم‌های جدید را اضافه کن
                        foreach (var item in invoiceItems)
                        {
                            item.PreInvoiceId = _Id; // مطمئن شو کلید خارجی مقدار دارد
                            update.preInvoiceItems.Add(item);
                        }

                        var confirm = await context.Update(_Id, update);
                        if (confirm != null)
                        {
                            PreInvoiceSave?.Invoke();
                        }
                        else
                        {
                            MessageBox.Show("این شماره قبلا انتخاب شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else
                    {

                        if (db.preInvoices.Any(i => i.ChangeId == _Id && i.PreDateYear == dateYear))
                        {
                            MessageBox.Show("این شماره فاکتور قبلاً ثبت شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (db.preInvoices.Any(c => c.ChangeId > _Id && c.PreDateYear == dateYear))
                        {
                            var lastInvoice = await db.preInvoices.OrderByDescending(i => i.ChangeId).Where(c => c.PreDateYear == dateYear).FirstOrDefaultAsync();
                            txtinvoiceId.Text = (lastInvoice.ChangeId + 1).ToString("D5");
                            MessageBox.Show("شماره فاکتور فعلی شما از آخرین شماره فاکتور ثبت شده کمتر است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        PreInvoice invoice = new PreInvoice()
                        {
                            ChangeId = _Id,
                            PreInvoiceIdS = _Id.ToString("D5"),
                            PreInvoiceSeller = _nameS,
                            PreInvoiceBuyer = _nameB,
                            PreInvoicePhone = _phone,
                            PreInvoicePhoneCode = _phoneCode,
                            PreInvoiceAddress = _address,
                            PreDateDay = _day,
                            PreDateMonth = _month,
                            PreDateYear = _year,
                            PreDateDayS = _dayS,
                            PreDateMonthS = _monthS,
                            PreDateYearS = _yearS,
                            InvoiceTotalCost = _total,
                            InvoiceDiscount = _discount,
                            Detail = _detail,
                            Detail2 = _detail2
                        };

                        foreach (var item in invoiceItems)
                        {
                            string priceStr = item.InvoiceItemPrice.ToString().Replace(",", "").Replace(" ریال", "").Trim();

                            if (decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal priceValue))
                            {
                                item.InvoiceItemPrice = priceValue;
                                invoice.preInvoiceItems.Add(item);
                            }
                            else
                            {
                                MessageBox.Show("خطا در تبدیل قیمت آیتم", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        var create = await context.Create(invoice);
                        if (create == null)
                        {
                            MessageBox.Show("قبلا این شماره فاکتور ثبت شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        PreInvoiceSave?.Invoke();
                    }
                    isSaved = true;
                    CloseControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SetId()
        {
            using (var db = new DataBaseContext())
            {
                int dateYear = db.financials.Select(c => c.LastYear).FirstOrDefault();
                // پیدا کردن آخرین شماره فاکتور
                var lastInvoice = await db.preInvoices.OrderByDescending(i => i.ChangeId).Where(i => i.PreDateYear == dateYear).FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(txtinvoiceId.Text))
                {
                    // اگر دیتابیس خالی بود، مقدار 1 تنظیم شود
                    txtinvoiceId.Text = (lastInvoice != null ? lastInvoice.ChangeId + 1 : 1).ToString("D5");
                }
            }
        }

        public void UpdateDateTimeInTextBoxes()
        {
            if (string.IsNullOrEmpty(txtDay.Text) && string.IsNullOrEmpty(txtMonth.Text) && string.IsNullOrEmpty(txtYear.Text))
            {
                PersianCalendar persianCalendar = new PersianCalendar();

                // دریافت تاریخ جاری
                DateTime currentDate = DateTime.Now;

                // تفکیک تاریخ به روز، ماه و سال شمسی
                int day = persianCalendar.GetDayOfMonth(currentDate); // روز با دو رقم
                int month = persianCalendar.GetMonth(currentDate); // ماه با دو رقم
                int year = persianCalendar.GetYear(currentDate); // سال با چهار رقم

                DateTime futureDate = currentDate.AddDays(1);

                int dayS = persianCalendar.GetDayOfMonth(futureDate); // روز با دو رقم
                int monthS = persianCalendar.GetMonth(futureDate); // ماه با دو رقم
                int yearS = persianCalendar.GetYear(futureDate); // سال با چهار رقم

                // نمایش روز، ماه و سال در تکس‌باکس‌ها
                txtDay.Text = day.ToString("00");      // نمایش روز
                txtMonth.Text = month.ToString("00");  // نمایش ماه
                txtYear.Text = year.ToString();    // نمایش سال
                txtDayS.Text = dayS.ToString("00");      // نمایش روز
                txtMonthS.Text = monthS.ToString("00");  // نمایش ماه
                txtYearS.Text = yearS.ToString();    // نمایش سال
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (TitlePreInvoice.Equals("1"))
            {
                SetId();
                UpdateDateTimeInTextBoxes();
                if (string.IsNullOrEmpty(txtfinalPrice.Text) || string.IsNullOrEmpty(txtfinalPay.Text) || string.IsNullOrEmpty(txtdiscount.Text))
                {
                    txtfinalPrice.Text = MoneyConverter.Money(0);
                    txtfinalPay.Text = MoneyConverter.Money(0);
                    txtdiscount.Text = MoneyConverter.Money(0);
                }
            }
            else if (TitlePreInvoice.Equals("2")) 
            {
                EditLoaded(sender, null);
            }
            else
            {
                EditLoaded(sender, null);
                txtNameB.IsReadOnly = true;
                txtNameS.IsReadOnly = true;
                txtDay.IsReadOnly = true;
                txtDayS.IsReadOnly = true;
                txtMonth.IsReadOnly = true;
                txtMonthS.IsReadOnly = true;
                txtYear.IsReadOnly = true;
                txtYearS.IsReadOnly = true;
                txtPhone.IsReadOnly = true;
                txtPhoneCode.IsReadOnly = true;
                txtAddress.IsReadOnly = true;
                txtdetail.IsReadOnly = true;
                txtdetail2.IsReadOnly = true;
                add.Visibility = Visibility.Collapsed;
                del.Visibility = Visibility.Collapsed;
                print.Visibility = Visibility.Collapsed;
                save.Visibility = Visibility.Collapsed;
                chk.Visibility = Visibility.Collapsed;
                dropDetail.Visibility = Visibility.Collapsed;
                dropDetail2.Visibility = Visibility.Collapsed;
                dropSeller.Visibility = Visibility.Collapsed;
                dropBuyer.Visibility = Visibility.Collapsed;
            }
            Keyboard.Focus(this);
            this.Focus();
        }

        private async Task RestoreProducts()
        {
            foreach (var item in backupItems)
            {
                var product = await _productService.GetId(item.ProductId);
                if (product != null)
                {
                    product.ProductNumber = item.InitialCount; // بازگردانی مقدار اولیه
                    await _productService.Update(product.ProductId, product);
                }
            }

            backupItems.Clear(); // پاک کردن لیست بکاپ بعد از بازگردانی
        }

        private async void back_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
            "آیا می‌خواهید بدون ذخیره خارج شوید؟",
            "خروج",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await RestoreProducts(); // بازگردانی مقادیر اولیه محصولات
                CloseControl();
                PreInvoiceSave?.Invoke();
            }
        }

        private async void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsVisible && !isSaved)
            {
                await RestoreProducts(); // بازگردانی مقادیر اولیه محصولات
            }
            isSaved = false;
        }

        private void sellerList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                sellerList.Text = string.Empty;
                sGrid.Focus();
                if (sGrid.Items.Count > 0)
                {
                    if (sGrid.SelectedIndex >= 0)
                    {
                        lastSelectedIndex = sGrid.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                    }

                    // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                    Dispatcher.InvokeAsync(() =>
                    {
                        // بررسی ایندکس قبلی و تنظیم آن
                        if (lastSelectedIndex >= 0 && lastSelectedIndex < sGrid.Items.Count)
                        {
                            // دوباره سلکت کردن ایندکس قبلی
                            sGrid.SelectedIndex = lastSelectedIndex;
                        }
                        else
                        {
                            // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                            sGrid.SelectedIndex = 0;
                        }

                        // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                        sGrid.CurrentCell = new DataGridCellInfo(sGrid.SelectedItem, sGrid.Columns[0]);
                        sGrid.Focus(); // فوکوس روی دیتاگرید

                        // انتقال فوکوس به سلکت شده جدید
                        sGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
            if (sGrid.Items.Count > 0)
            {
                if (e.Key == Key.Enter)
                {
                    sGrid.Focus();
                    if (sGrid.Items.Count > 0)
                    {
                        if (sGrid.SelectedIndex >= 0)
                        {
                            lastSelectedIndex = sGrid.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                        }

                        // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                        Dispatcher.InvokeAsync(() =>
                        {
                            // بررسی ایندکس قبلی و تنظیم آن
                            if (lastSelectedIndex >= 0 && lastSelectedIndex < sGrid.Items.Count)
                            {
                                // دوباره سلکت کردن ایندکس قبلی
                                sGrid.SelectedIndex = lastSelectedIndex;
                            }
                            else
                            {
                                // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                                sGrid.SelectedIndex = 0;
                            }

                            // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                            sGrid.CurrentCell = new DataGridCellInfo(sGrid.SelectedItem, sGrid.Columns[0]);
                            sGrid.Focus(); // فوکوس روی دیتاگرید

                            // انتقال فوکوس به سلکت شده جدید
                            sGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                    }
                }
            }
        }
            
        private void buyerList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                buyerList.Text = string.Empty;
                bGrid.Focus();
                if (bGrid.Items.Count > 0)
                {
                    if (bGrid.SelectedIndex >= 0)
                    {
                        lastSelectedIndex = bGrid.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                    }

                    // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                    Dispatcher.InvokeAsync(() =>
                    {
                        // بررسی ایندکس قبلی و تنظیم آن
                        if (lastSelectedIndex >= 0 && lastSelectedIndex < bGrid.Items.Count)
                        {
                            // دوباره سلکت کردن ایندکس قبلی
                            bGrid.SelectedIndex = lastSelectedIndex;
                        }
                        else
                        {
                            // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                            bGrid.SelectedIndex = 0;
                        }

                        // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                        bGrid.CurrentCell = new DataGridCellInfo(bGrid.SelectedItem, bGrid.Columns[0]);
                        bGrid.Focus(); // فوکوس روی دیتاگرید

                        // انتقال فوکوس به سلکت شده جدید
                        bGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }

            if (bGrid.Items.Count > 0)
            {
                if (e.Key == Key.Enter)
                {
                    bGrid.Focus();
                    if (bGrid.Items.Count > 0)
                    {
                        if (bGrid.SelectedIndex >= 0)
                        {
                            lastSelectedIndex = bGrid.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                        }

                        // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                        Dispatcher.InvokeAsync(() =>
                        {
                            // بررسی ایندکس قبلی و تنظیم آن
                            if (lastSelectedIndex >= 0 && lastSelectedIndex < bGrid.Items.Count)
                            {
                                // دوباره سلکت کردن ایندکس قبلی
                                bGrid.SelectedIndex = lastSelectedIndex;
                            }
                            else
                            {
                                // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                                bGrid.SelectedIndex = 0;
                            }

                            // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                            bGrid.CurrentCell = new DataGridCellInfo(bGrid.SelectedItem, bGrid.Columns[0]);
                            bGrid.Focus(); // فوکوس روی دیتاگرید

                            // انتقال فوکوس به سلکت شده جدید
                            bGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                    }
                }
            }
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.FocusedElement is TextBox textBox || Keyboard.FocusedElement is Border border)
                return;

            if (e.Key == Key.F1)
            {
                hint.Visibility = Visibility.Visible;
                hint.Focus();
            }
            if (e.Key == Key.F2)
            {
                add_Click(add, null);
            }
            if (e.Key == Key.F3)
            {
                del_Click(del, null);
            }
            if (e.Key == Key.F4)
            {
                edit_Click(edit, null);
            }
            if (e.Key == Key.F5)
            {
                save_Click(save, null);
            }
            if (e.Key == Key.Back)
            {
                back_Click(back, null);
            }
        }

        private async void edit_Click(object sender, RoutedEventArgs e)
        {
            buyp.Visibility = Visibility.Collapsed;
            sellp.Visibility = Visibility.Collapsed;
            bDetail.Visibility = Visibility.Collapsed;
            bDetail2.Visibility = Visibility.Collapsed;
            addGrid.Visibility = Visibility.Collapsed;

            if (myDataGrid.SelectedItem != null)
            {
                editB.Visibility = (editB.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;

                dynamic selected = myDataGrid.SelectedItem;
                int Id = selected.ProductId;
                int id = selected.InvoiceItemId;

                if (Id > 0)
                {
                    using (var db = new DataBaseContext())
                    {
                        var context = new GenericDataBaseServices<PreInvoiceItem>(db);
                        var query = invoiceItems.FirstOrDefault(x => x.ProductId == Id);
                        if (query != null)
                        {
                            productCode2.Text = query.ProductCode;
                            productName2.Text = query.ProductName;
                            productNumber2.Text = (query.InvoiceItemNumber).ToString();
                            productPrice2.Text = (query.InvoiceItemPrice).ToString();

                            saveNumber = query.InvoiceItemNumber;

                            var product = await _productService.GetId(Id);
                            if (product != null)
                            {
                                productNumberShow.Text = MoneyConverter.Money1(product.ProductNumber);
                            }
                            productNumber2.Focus();
                            productNumber2.SelectAll();
                        }
                        else
                        {
                            MessageBox.Show("محصول مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
                else
                {
                    using (var db = new DataBaseContext())
                    {
                        var context = new GenericDataBaseServices<PreInvoiceItem>(db);
                        var query = invoiceItems.FirstOrDefault(x => x.InvoiceItemId == id);
                        if (query != null)
                        {
                            productCode2.Text = query.ProductCode;
                            productName2.Text = query.ProductName;
                            productNumber2.Text = (query.InvoiceItemNumber).ToString();
                            productPrice2.Text = (query.InvoiceItemPrice).ToString();

                            saveNumber = query.InvoiceItemNumber;

                            productNumber2.Focus();
                            productNumber2.SelectAll();
                        }
                        else
                        {
                            MessageBox.Show("محصول مورد نظر یافت نشد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }
        }

        private async void ad_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem == null)
            {
                MessageBox.Show("لطفا ابتدا یک آیتم را برای ویرایش انتخاب کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(productNumber2.Text, out int _number) || !decimal.TryParse(productPrice2.Text, out decimal _price))
            {
                MessageBox.Show("لطفا مقدار عددی وارد کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // فقط از دیتاگرید استفاده می‌کنیم، نه دیتابیس
            var selected = (PreInvoiceItem)myDataGrid.SelectedItem;

            // بررسی موجودی
            string show = productNumberShow.Text.Replace(",", "").Trim();

            if (selected.ProductId > 0)
            {
                var product = await _productService.GetId(selected.ProductId);
                if (product != null)
                {
                    // اول موجودی قبلی رو برگردون
                    product.ProductNumber += saveNumber;

                    // بررسی محدودیت موجودی
                    if (_number <= product.ProductNumber)
                    {
                        // بعد از اون، مقدار جدید کم بشه
                        product.ProductNumber -= _number;
                        await _productService.Update(product.ProductId, product);

                        selected.InvoiceItemNumber = _number;
                        selected.InvoiceItemPrice = _price;
                        selected.InvoiceItemPriceS = MoneyConverter.Money(_price);
                        selected.InvoiceFinalPrice = _price * _number;
                        selected.InvoiceFinalPriceS = MoneyConverter.Money(_price * _number);
                    }
                    else
                    {
                        var result = MessageBox.Show($"موجودی فقط {product.ProductNumber} عدد است. آیا می‌خواهید کل موجودی را وارد کنید؟", "هشدار", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            _number = product.ProductNumber;
                            product.ProductNumber = 0;
                            await _productService.Update(product.ProductId, product);

                            selected.InvoiceItemNumber = _number;
                            selected.InvoiceItemPrice = _price;
                            selected.InvoiceItemPriceS = MoneyConverter.Money(_price);
                            selected.InvoiceFinalPrice = _price * _number;
                            selected.InvoiceFinalPriceS = MoneyConverter.Money(_price * _number);
                        }
                        else return;
                    }
                }
            }
            else
            {
                selected.InvoiceItemNumber = _number;
                selected.InvoiceItemPrice = _price;
                selected.InvoiceItemPriceS = MoneyConverter.Money(_price);
                selected.InvoiceFinalPrice = _price * _number;
                selected.InvoiceFinalPriceS = MoneyConverter.Money(_price * _number);
            }

            // رفرش دیتاگرید (در صورتی که تغییرات نمایش داده نشد)
            myDataGrid.Items.Refresh();

            editB.Visibility = Visibility.Collapsed;
            LoadFinalPrice();

            if (chk.IsChecked == true)
                LoadDarsad();
            else
                LoadDiscount();
        }

        private void ca_Click(object sender, RoutedEventArgs e)
        {  
            editB.Visibility = Visibility.Collapsed;
        }

        private void productNumber2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                productPrice2.Focus();
                productPrice2.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                productPrice2.Focus();
                productPrice2.SelectAll();
            }
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                ca_Click(ca, null);
            }
        }

        private void productPrice2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ad_Click(ad, null);
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                productNumber2.Focus();
                productNumber2.SelectAll();
            }
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                ca_Click(ca, null);
            }
        }

        private async void searchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (addGrid.Visibility == Visibility.Visible)
            {
                var searchText = searchProduct.Text.Trim();

                var context = await _productService.SearchProducts(searchText);
                productDG.ItemsSource = context;
            }
        }

        private void searchProduct_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                searchProduct.Text = string.Empty;
                productDG.Focus();
                if (productDG.Items.Count > 0)
                {
                    if (productDG.SelectedIndex >= 0)
                    {
                        lastSelectedIndex = productDG.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                    }

                    // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                    Dispatcher.InvokeAsync(() =>
                    {
                        // بررسی ایندکس قبلی و تنظیم آن
                        if (lastSelectedIndex >= 0 && lastSelectedIndex < productDG.Items.Count)
                        {
                            // دوباره سلکت کردن ایندکس قبلی
                            productDG.SelectedIndex = lastSelectedIndex;
                        }
                        else
                        {
                            // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                            productDG.SelectedIndex = 0;
                        }

                        // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                        productDG.CurrentCell = new DataGridCellInfo(productDG.SelectedItem, productDG.Columns[0]);
                        productDG.Focus(); // فوکوس روی دیتاگرید

                        // انتقال فوکوس به سلکت شده جدید
                        productDG.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
            }
            if (productDG.Items.Count > 0)
            {
                if (e.Key == Key.Enter)
                {
                    productDG.Focus();
                    if (productDG.Items.Count > 0)
                    {
                        if (productDG.SelectedIndex >= 0)
                        {
                            lastSelectedIndex = productDG.SelectedIndex;  // ذخیره ایندکس انتخابی فعلی
                        }

                        // از Dispatcher برای اطمینان از اجرای عملیات بعد از آماده شدن UI استفاده می‌کنیم
                        Dispatcher.InvokeAsync(() =>
                        {
                            // بررسی ایندکس قبلی و تنظیم آن
                            if (lastSelectedIndex >= 0 && lastSelectedIndex < productDG.Items.Count)
                            {
                                // دوباره سلکت کردن ایندکس قبلی
                                productDG.SelectedIndex = lastSelectedIndex;
                            }
                            else
                            {
                                // اگر ایندکس قبلی معتبر نیست، اولین ایتم را انتخاب می‌کنیم
                                productDG.SelectedIndex = 0;
                            }

                            // تنظیم فوکوس بر روی سلکت شده برای جلوگیری از مشکل فوکوس با کلیدهای جهت‌دار
                            productDG.CurrentCell = new DataGridCellInfo(productDG.SelectedItem, productDG.Columns[0]);
                            productDG.Focus(); // فوکوس روی دیتاگرید

                            // انتقال فوکوس به سلکت شده جدید
                            productDG.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                    }
                }
            }
        }

        private void hint_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                hint.Visibility = Visibility.Collapsed;
                grid.Focus();
            }
        }

        private void productPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(productPrice.Text))
            {
                decimal get = decimal.Parse(productPrice.Text.Trim());
                productShow.Text = MoneyConverter.Money1(get);
            }
            else
            {
                productShow.Text = MoneyConverter.Money1(0);
            }
        }

        private void productPrice2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(productPrice2.Text))
            {
                decimal get = decimal.Parse(productPrice2.Text.Trim());
                productPriceShow.Text = MoneyConverter.Money1(get);
            }
            else
            {
                productPriceShow.Text = MoneyConverter.Money1(0);
            }
        }
    }
}
