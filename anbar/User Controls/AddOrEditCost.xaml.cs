using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for AddOrEditCost.xaml
    /// </summary>
    public partial class AddOrEditCost : UserControl
    {
        public ObservableCollection<Person> people { get; set; }
        private readonly GenericDataBaseServices<Person> _personService;
        public int lastSelectedIndex = -1;
        public Action CostSave { get; set; }
        private int? ID { get; set; }
        public Cost CostToEdit { get; set; }

        public static readonly DependencyProperty TitlePersonProperty = DependencyProperty.Register("TitleCost", typeof(string), typeof(AddOrEditPerson), new PropertyMetadata(""));

        public string TitleCost
        {
            get { return (string)GetValue(TitlePersonProperty); }
            set { SetValue(TitlePersonProperty, value); }
        }

        public AddOrEditCost()
        {
            InitializeComponent();
            _personService = new GenericDataBaseServices<Person>(new DataBaseContext());
            people = new ObservableCollection<Person>();
            DataContext = this;
            this.Loaded += AddOrEditPerson_Loaded;
        }

        private void AddOrEditPerson_Loaded(object sender, RoutedEventArgs e)
        {
            if (TitleCost.Equals("1"))
            {
                PersianCalendar persianCalendar = new PersianCalendar();

                // دریافت تاریخ جاری
                DateTime currentDate = DateTime.Now;

                // تفکیک تاریخ به روز، ماه و سال شمسی
                string day = persianCalendar.GetDayOfMonth(currentDate).ToString("00"); // روز با دو رقم
                string month = persianCalendar.GetMonth(currentDate).ToString("00"); // ماه با دو رقم
                string year = persianCalendar.GetYear(currentDate).ToString(); // سال با چهار رقم

                // نمایش روز، ماه و سال در تکس‌باکس‌ها
                txtDay.Text = day;      // نمایش روز
                txtMonth.Text = month;  // نمایش ماه
                txtYear.Text = year;    // نمایش سال
            }
            if (CostToEdit != null && TitleCost.Equals("2"))
            {
                txtSeller.Text = CostToEdit.Seller;
                txtCaption.Text = CostToEdit.Caption;
                txtPhone.Text = CostToEdit.SellerPhone;
                txtPhoneCode.Text = CostToEdit.SellerPhoneCode;
                txtMobile.Text = CostToEdit.SellerMobile;
                txtAddress.Text = CostToEdit.SellerAddress;
                txtDay.Text = CostToEdit.day.ToString("00");
                txtMonth.Text = CostToEdit.month.ToString("00");
                txtYear.Text = CostToEdit.year.ToString("0000");
                decimal fee = CostToEdit.Fee;
                txtFee.Text = MoneyConverter.Money1(fee);
                decimal discount = CostToEdit.Discount;
                txtDiscount.Text = MoneyConverter.Money1(discount);
                txtTotal.Text = MoneyConverter.Money1(fee - discount);
                txtDetail.Text = CostToEdit.Detail;
                this.ID = CostToEdit.Id;
            }
            if (CostToEdit != null && TitleCost.Equals("3"))
            {
                txtSeller.Text = CostToEdit.Seller;
                txtCaption.Text = CostToEdit.Caption;
                txtPhone.Text = CostToEdit.SellerPhone;
                txtPhoneCode.Text = CostToEdit.SellerPhoneCode;
                txtMobile.Text = CostToEdit.SellerMobile;
                txtAddress.Text = CostToEdit.SellerAddress;
                txtDay.Text = CostToEdit.day.ToString("00");
                txtMonth.Text = CostToEdit.month.ToString("00");
                txtYear.Text = CostToEdit.year.ToString("0000");
                decimal fee = CostToEdit.Fee;
                txtFee.Text = MoneyConverter.Money1(fee);
                decimal discount = CostToEdit.Discount;
                txtDiscount.Text = MoneyConverter.Money1(discount);
                txtTotal.Text = MoneyConverter.Money1(fee - discount);
                txtDetail.Text = CostToEdit.Detail;
                this.ID = CostToEdit.Id;

                txtSeller.IsReadOnly = true;
                txtCaption.IsReadOnly = true;
                txtPhone.IsReadOnly = true;
                txtPhoneCode.IsReadOnly = true;
                txtMobile.IsReadOnly = true;
                txtAddress.IsReadOnly = true;
                txtDay.IsReadOnly = true;
                txtMonth.IsReadOnly = true;
                txtYear.IsReadOnly = true;
                txtFee.IsReadOnly = true;
                txtDiscount.IsReadOnly = true;
                txtTotal.IsReadOnly = true;
                txtDetail.IsReadOnly = true;
                dropSeller.Visibility = Visibility.Collapsed;
                save.Visibility = Visibility.Collapsed;
                cancel.Visibility = Visibility.Collapsed;
                exit.Visibility = Visibility.Visible;
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
                var mainControl = FindParent<USCost>(this);
                if (mainControl != null)
                {
                    mainControl.add.IsEnabled = true;
                    mainControl.edit.IsEnabled = true;
                    mainControl.delete.IsEnabled = true;
                    mainControl.dgInvoice.IsEnabled = true;
                    mainControl.txtedit.IsEnabled = true;
                    mainControl.txtTotalFee.IsEnabled = true;
                    mainControl.txtDiscount.IsEnabled = true;
                    mainControl.txtTotalPay.IsEnabled = true;
                    mainControl.txtPay.IsEnabled = true;
                    mainControl.txtLeft.IsEnabled = true;
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CloseControl();
            CostSave?.Invoke();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {

            string _seller = txtSeller.Text.Trim();
            string _caption = txtCaption.Text.Trim();
            string _phone = txtPhone.Text.Trim();
            string _phoneCode = txtPhoneCode.Text.Trim();
            string _mobile = txtMobile.Text.Trim();
            string _address = txtAddress.Text.Trim();
            string _info = txtDetail.Text.Trim();
            string fee = txtFee.Text.Replace(",", "").Replace(" ریال", "").Trim();
            string discount = txtDiscount.Text.Replace(",", "").Replace(" ریال", "").Trim();

            if (!int.TryParse(txtDay.Text, out int _day) || !int.TryParse(txtMonth.Text, out int _month) || !int.TryParse(txtYear.Text, out int _year) ||
                !decimal.TryParse(discount, out decimal _discount) || !decimal.TryParse(fee, out decimal _fee))
            {
                MessageBox.Show("لطفا مقدار عددی وارد کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_seller) || string.IsNullOrEmpty(_caption))
            {
                MessageBox.Show("لطفا فیلد های عنوان و نام فروشنده را کامل کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new DataBaseContext())
            {
                var repository = new GenericDataBaseServices<Cost>(db);

                if (TitleCost.Equals("2"))
                {
                    var userUpdate = await repository.GetId(ID.Value);

                    if (userUpdate == null)
                    {
                        MessageBox.Show("خطا", "شخص مورد نظر پیدا نشد", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    userUpdate.Seller = _seller;
                    userUpdate.Caption = _caption;
                    userUpdate.SellerPhone = _phone;
                    userUpdate.SellerPhoneCode = _phoneCode;
                    userUpdate.SellerMobile = _mobile;
                    userUpdate.SellerAddress = _address;
                    userUpdate.day = _day;
                    userUpdate.month = _month;
                    userUpdate.year = _year;
                    userUpdate.Fee = _fee;
                    userUpdate.Discount = _discount;
                    userUpdate.Detail = _info;

                    var update = await repository.Update(ID.Value, userUpdate);
                    if (update != null)
                    {
                        CostSave?.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("این کد قبلا اینتخاب شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    Cost person = new Cost
                    {
                        Seller = _seller,
                        Caption = _caption,
                        SellerPhone = _phone,
                        SellerPhoneCode = _phoneCode,
                        SellerMobile = _mobile,
                        SellerAddress = _address,
                        day = _day,
                        month = _month,
                        year = _year,
                        Fee = _fee,
                        Discount = _discount,
                        Detail = _info,
                        Type = 1
                    };

                    var create = await repository.Create(person);
                    if (create == null)
                    {
                        MessageBox.Show("قبلا این کد ثبت شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    CostSave?.Invoke();
                }
                CloseControl();
            }
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
            txtSeller.Focus();
        }

        private void txtFee_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = txtFee.Text.Replace(",", ""); // حذف کاماهای قبلی
            if (decimal.TryParse(text, out decimal value))
            {
                lblFee.Text = MoneyConverter.Money(value);
            }
            else
            {
                lblFee.Text = "";
            }
        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = txtDiscount.Text.Replace(",", ""); // حذف کاماهای قبلی
            if (decimal.TryParse(text, out decimal value))
            {
                lblDiscount.Text = MoneyConverter.Money(value);
            }
            else
            {
                lblDiscount.Text = "";
            }
            decimal fee = decimal.Parse(txtFee.Text.Trim());
            if (fee >= value)
            {
                txtTotal.Text = MoneyConverter.Money1(fee - value);
            }
            else
            {
                MessageBox.Show("مقدار تخفیف از قیمت نمی تواند بیشتر باشد", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDiscount.Text = string.Empty;
                return;
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
                        PersonAddress = dto.PersonAddress,
                        PersonMobile = dto.PersonMobile
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

        private void sGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sGrid.SelectedItem is Person selectedPerson)
            {
                txtSeller.Text = selectedPerson.PersonName;
                txtPhone.Text = selectedPerson.PersonPhone;
                txtPhoneCode.Text = selectedPerson.PersonPhoneCode;
                txtMobile.Text = selectedPerson.PersonMobile;
                txtAddress.Text = selectedPerson.PersonAddress;
            }
        }

        private void sGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sGrid.SelectedItem is Person selectedPerson)
                {
                    txtSeller.Text = selectedPerson.PersonName;
                    txtPhone.Text = selectedPerson.PersonPhone;
                    txtPhoneCode.Text = selectedPerson.PersonPhoneCode;
                    txtMobile.Text = selectedPerson.PersonMobile;
                    txtAddress.Text = selectedPerson.PersonAddress;
                    e.Handled = true;
                    sellp.Visibility = Visibility.Collapsed;
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

        private void dropSeller_Click(object sender, RoutedEventArgs e)
        {
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

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            CloseControl();
        }

        private void txtSeller_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtCaption.Focus();
                txtCaption.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtCaption.Focus();
                txtCaption.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtDay.Focus();
                txtDay.SelectAll();
            }
        }

        private void txtDay_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtSeller.Focus();
                txtSeller.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtSeller.Focus();
                txtSeller.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtMonth.Focus();
                txtMonth.SelectAll();
            }
        }

        private void txtMonth_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtDay.Focus();
                txtDay.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtDay.Focus();
                txtDay.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtYear.Focus();
                txtYear.SelectAll();
            }
        }

        private void txtYear_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtMonth.Focus();
                txtMonth.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtMonth.Focus();
                txtMonth.SelectAll();
            }
        }

        private void txtCaption_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtPhoneCode.Focus();
                txtPhoneCode.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtPhoneCode.Focus();
                txtPhoneCode.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtSeller.Focus();
                txtSeller.SelectAll();
            }
        }

        private void txtPhone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtMobile.Focus();
                txtMobile.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtMobile.Focus();
                txtMobile.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtPhoneCode.Focus();
                txtPhoneCode.SelectAll();
            }
        }

        private void txtPhoneCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtPhone.Focus();
                txtPhone.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtPhone.Focus();
                txtPhone.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtCaption.Focus();
                txtCaption.SelectAll();
            }
        }

        private void txtMobile_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtAddress.Focus();
                txtAddress.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtAddress.Focus();
                txtAddress.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtPhone.Focus();
                txtPhone.SelectAll();
            }
        }

        private void txtAddress_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtDetail.Focus();
                txtDetail.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtDetail.Focus();
                txtDetail.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtMobile.Focus();
                txtMobile.SelectAll();
            }
        }

        private void txtDetail_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtFee.Focus();
                txtFee.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtFee.Focus();
                txtFee.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtAddress.Focus();
                txtAddress.SelectAll();
            }
        }

        private void txtFee_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                txtDiscount.Focus();
                txtDiscount.SelectAll();
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                txtDiscount.Focus();
                txtDiscount.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtDetail.Focus();
                txtDetail.SelectAll();
            }
        }

        private void txtDiscount_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Save_Click(save, null);
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                txtFee.Focus();
                txtFee.SelectAll();
            }
        }
    }
}
