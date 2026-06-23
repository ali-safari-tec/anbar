using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using anbar.Data.Models;
using anbar.Data.Services;
using anbar.Tools;
using anbar.Windows;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for AddOrEditProduct.xaml
    /// </summary>
    public partial class AddOrEditProduct : UserControl
    {
        public Action ProductSave { get; set; }
        public Product ProductToEdit { get; set; }
        private int? ID { get; set; }

        public static readonly DependencyProperty TitleProductProperty = DependencyProperty.Register("TitleTextProduct", typeof(string), typeof(AddOrEditProduct), new PropertyMetadata(""));

        public string TitleProduct
        {
            get { return (string)GetValue(TitleProductProperty); }
            set { SetValue(TitleProductProperty, value); }
        }

        public AddOrEditProduct()
        {
            InitializeComponent();
            this.Loaded += AddOrEditProduct_Loaded;
        }

        private void AddOrEditProduct_Loaded(object sender, RoutedEventArgs e)
        {
            if (ProductToEdit != null)
            {
                IdProduct.Text = ProductToEdit.ProductIdByUser;
                nameProduct.Text = ProductToEdit.ProductName;
                priceProduct.Text = ProductToEdit.ProductPrice.ToString();
                countProduct.Text = ProductToEdit.ProductNumber.ToString();
                infoProduct.Text = ProductToEdit.ProductDetail;
                this.ID = ProductToEdit.ProductId;
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
                var main = FindParent<USProduct>(this);
                if (main != null)
                {
                    main.dgInvoice.IsEnabled = true;
                    main.Add.IsEnabled = true;
                    main.Edit.IsEnabled = true;
                    main.Delete.IsEnabled = true;
                    main.count.IsEnabled = true;
                    main.fee.IsEnabled = true;
                    main.detail.IsEnabled = true;
                }
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

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string _Id = IdProduct.Text.Trim();
                string _name = nameProduct.Text.Trim();
                string _info = infoProduct.Text.Trim();

                if (string.IsNullOrEmpty(_Id) || string.IsNullOrEmpty(_name))
                {
                    MessageBox.Show("لطفا بخش های کد محصول، نام محصول را وارد کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IdProduct.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(priceProduct.Text) || string.IsNullOrEmpty(countProduct.Text))
                {
                    priceProduct.Text = "0";
                    countProduct.Text = "0";
                }

                if (!decimal.TryParse(priceProduct.Text, out decimal _price) || !int.TryParse(countProduct.Text, out int _count))
                {
                    MessageBox.Show("لطفا در بخش های قیمت و تعداد مقدار عددی وارد کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                    priceProduct.Focus();
                    priceProduct.SelectAll();
                    return;
                }

                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<Product>(db);

                    if (TitleProduct.Equals("ویرایش مشخصات محصول"))
                    {
                        var selectedUpdate = await context.GetId(ID.Value);
                        if (selectedUpdate == null)
                        {
                            MessageBox.Show("خطا", "شخص مورد نظر پیدا نشد", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        selectedUpdate.ProductIdByUser = _Id;
                        selectedUpdate.ProductName = _name;
                        selectedUpdate.ProductPrice = _price;
                        selectedUpdate.ProductNumber = _count;
                        selectedUpdate.ProductDetail = _info;

                        var update = await context.Update(ID.Value, selectedUpdate);
                        if (update != null)
                        {
                            ProductSave?.Invoke();
                        }
                        else
                        {
                            MessageBox.Show("این کد قبلا اینتخاب شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else
                    {
                        Product product = new Product()
                        {
                            ProductIdByUser = _Id,
                            ProductName = _name,
                            ProductPrice = _price,
                            ProductNumber = _count,
                            ProductDetail = _info
                        };

                        var create = await context.Create(product);
                        if (create == null)
                        {
                            MessageBox.Show("قبلا این کد ثبت شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        ProductSave?.Invoke();
                    }
                    CloseControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CloseControl();
            ProductSave?.Invoke();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Cancel_Click(cancel, null);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IdProduct.Focus();
        }

        private void IdProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdProduct.Text))
            {
                a2.Visibility = Visibility.Collapsed;
            }
            else
            {
                a2.Visibility = Visibility.Visible;
            }
        }

        private void nameProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(nameProduct.Text))
            {
                a1.Visibility = Visibility.Collapsed;
            }
            else
            {
                a1.Visibility = Visibility.Visible;
            }
        }

        private void IdProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                nameProduct.Focus();
                nameProduct.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                nameProduct.Focus();
                nameProduct.SelectAll();
            }
        }

        private void nameProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                priceProduct.Focus();
                priceProduct.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                priceProduct.Focus();
                priceProduct.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                IdProduct.Focus();
                IdProduct.SelectAll();
            }
        }

        private void priceProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                countProduct.Focus();
                countProduct.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                countProduct.Focus();
                countProduct.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                nameProduct.Focus();
                nameProduct.SelectAll();
            }
        }

        private void countProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                infoProduct.Focus();
                infoProduct.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                infoProduct.Focus();
                infoProduct.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                priceProduct.Focus();
                priceProduct.SelectAll();
            }
        }

        private void infoProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save_Click(save, null);
            }
            if (e.Key == Key.Up)
            {
                countProduct.Focus();
                countProduct.SelectAll();
            }
        }

        private void priceProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(priceProduct.Text, out decimal price))
            {
                lbl1.Content = MoneyConverter.Money(price);
            }
        }

        private void countProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(countProduct.Text, out int count))
            {
                lbl2.Content = MoneyConverter.Money1(count);
            }
        }
    }
}
