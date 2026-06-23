using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using anbar.Data.Models;
using anbar.Data.Services;
using anbar.Windows;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for AddOrEditPerson.xaml
    /// </summary>
    public partial class AddOrEditPerson : UserControl
    {
        public Action PersonSave { get; set; }
        private int? ID { get; set; }
        public Person PersonToEdit { get; set; }

        public static readonly DependencyProperty TitlePersonProperty = DependencyProperty.Register("TitlePerson", typeof(string), typeof(AddOrEditPerson), new PropertyMetadata(""));

        public string TitlePerson
        {
            get { return (string)GetValue(TitlePersonProperty); }
            set { SetValue(TitlePersonProperty, value); }
        }

        public AddOrEditPerson()
        {
            InitializeComponent();
            this.Loaded += AddOrEditPerson_Loaded;
        }

        private void AddOrEditPerson_Loaded(object sender, RoutedEventArgs e)
        {
            if (PersonToEdit != null)
            {
                IdPerson.Text = PersonToEdit.PersonIdByUser;
                namePerson.Text = PersonToEdit.PersonName;
                modelPerson.Text = PersonToEdit.PersonModel;
                cityPerson.Text = PersonToEdit.PersonCity;
                provincePerson.Text = PersonToEdit.PersonProvince;
                phonePerson.Text = PersonToEdit.PersonPhone;
                phoneCodePerson.Text = PersonToEdit.PersonPhoneCode;
                mobilePerson.Text = PersonToEdit.PersonMobile;
                addressPerson.Text = PersonToEdit.PersonAddress;
                infoPerson.Text = PersonToEdit.PersonDetail;
                this.ID = PersonToEdit.PersonId;
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
                var main = FindParent<USPerson>(this);
                if (main != null)
                {
                    main.dgInvoice.IsEnabled = true;
                    main.add.IsEnabled = true;
                    main.edit.IsEnabled = true;
                    main.delete.IsEnabled = true;
                    main.type.IsEnabled = true;
                    main.city.IsEnabled = true;
                    main.province.IsEnabled = true;
                    main.address.IsEnabled = true;
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CloseControl();
            PersonSave?.Invoke();
        }

        bool IsNumeric(string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsDigit);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {

            string _Id = IdPerson.Text.Trim();
            string _name = namePerson.Text.Trim();
            string _model = modelPerson.Text.Trim();
            string _city = cityPerson.Text.Trim();
            string _province = provincePerson.Text.Trim();
            string _phone = phonePerson.Text.Trim();
            string _phoneCode = phoneCodePerson.Text.Trim();
            string _mobile = mobilePerson.Text.Trim();
            string _address = addressPerson.Text.Trim();
            string _info = infoPerson.Text.Trim();

            if (string.IsNullOrEmpty(_Id) || string.IsNullOrEmpty(_name))
            {
                MessageBox.Show("بخش‌های کد و نام را وارد کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                IdPerson.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(phonePerson.Text))
            {
                if (!IsNumeric(_phone))
                {
                    MessageBox.Show("بخش‌ تلفن را به صورت عددی وارد کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(phoneCodePerson.Text))
            {
                if (!IsNumeric(_phoneCode))
                {
                    MessageBox.Show("بخش‌ کد تلفن را به صورت عددی وارد کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(mobilePerson.Text))
            {
                if (!IsNumeric(_mobile))
                {
                    MessageBox.Show("بخش‌ موبایل را به صورت عددی وارد کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            using (var db = new DataBaseContext())
            {
                var repository = new GenericDataBaseServices<Person>(db);

                if (TitlePerson.Equals("ویرایش مشخصات شخص"))
                {
                    var userUpdate = await repository.GetId(ID.Value);

                    if (userUpdate == null)
                    {
                        MessageBox.Show("خطا", "شخص مورد نظر پیدا نشد", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    userUpdate.PersonIdByUser = _Id;
                    userUpdate.PersonName = _name;
                    userUpdate.PersonModel = _model;
                    userUpdate.PersonCity = _city;
                    userUpdate.PersonProvince = _province;
                    userUpdate.PersonPhone = _phone;
                    userUpdate.PersonPhoneCode = _phoneCode;
                    userUpdate.PersonMobile = _mobile;
                    userUpdate.PersonAddress = _address;
                    userUpdate.PersonDetail = _info;

                    var update = await repository.Update(ID.Value, userUpdate);
                    if (update != null)
                    {
                        PersonSave?.Invoke();
                    }
                    else
                    {
                        MessageBox.Show("این کد قبلا اینتخاب شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    Person person = new Person
                    {
                        PersonIdByUser = _Id,
                        PersonName = _name,
                        PersonModel = _model,
                        PersonCity = _city,
                        PersonProvince = _province,
                        PersonPhone = _phone,
                        PersonPhoneCode = _phoneCode,
                        PersonMobile = _mobile,
                        PersonAddress = _address,
                        PersonDetail = _info
                    };

                    var create = await repository.Create(person);
                    if (create == null)
                    {
                        MessageBox.Show("قبلا این کد ثبت شده است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    PersonSave?.Invoke();
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
            IdPerson.Focus();
        }

        private void IdPerson_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdPerson.Text))
            {
                a1.Visibility = Visibility.Collapsed;
            }
            else
            {
                a1.Visibility = Visibility.Visible;
            }
        }

        private void namePerson_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(namePerson.Text))
            {
                a2.Visibility = Visibility.Collapsed;
            }
            else
            {
                a2.Visibility = Visibility.Visible;
            }
        }

        private void IdPerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                namePerson.Focus();
                namePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                namePerson.Focus();
                namePerson.SelectAll();
                e.Handled = true;
            }
        }

        private void namePerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                modelPerson.Focus();
                modelPerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                modelPerson.Focus();
                modelPerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                IdPerson.Focus();
                IdPerson.SelectAll();
                e.Handled = true;
            }
        }

        private void modelPerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cityPerson.Focus();
                cityPerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                cityPerson.Focus();
                cityPerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                namePerson.Focus();
                namePerson.SelectAll();
                e.Handled = true;
            }
        }

        private void cityPerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                provincePerson.Focus();
                provincePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                provincePerson.Focus();
                provincePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                modelPerson.Focus();
                modelPerson.SelectAll();
                e.Handled = true;
            }
        }

        private void provincePerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                phonePerson.Focus();
                phonePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                phonePerson.Focus();
                phonePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                cityPerson.Focus();
                cityPerson.SelectAll();
                e.Handled = true;
            }
        }

        private void phonePerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                phoneCodePerson.Focus();
                phoneCodePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                phoneCodePerson.Focus();
                phoneCodePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                provincePerson.Focus();
                provincePerson.SelectAll();
                e.Handled = true;
            }
        }

        private void phoneCodePerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mobilePerson.Focus();
                mobilePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                mobilePerson.Focus();
                mobilePerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                phonePerson.Focus();
                phonePerson.SelectAll();
                e.Handled = true;
            }
        }

        private void mobilePerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addressPerson.Focus();
                addressPerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                addressPerson.Focus();
                addressPerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                phoneCodePerson.Focus();
                phoneCodePerson.SelectAll();
                e.Handled = true;
            }
        }

        private void addressPerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                infoPerson.Focus();
                infoPerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                infoPerson.Focus();
                infoPerson.SelectAll();
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                mobilePerson.Focus();
                mobilePerson.SelectAll();
                e.Handled = true;
            }
        }

        private void infoPerson_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save_Click(save, null);
            }
            if (e.Key == Key.Up)
            {
                addressPerson.Focus();
                addressPerson.SelectAll();
                e.Handled = true;
            }
        }
    }
}
