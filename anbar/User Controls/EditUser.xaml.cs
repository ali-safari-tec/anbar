using System;
using System.Linq;
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
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : UserControl
    {
        public EditUser()
        {
            InitializeComponent();
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

        private void ExitUC()
        {
            var parent = this.Parent as Grid;
            if (parent != null)
            {
                var mainControl = FindParent<MainWindow>(this);
                if (mainControl != null)
                {
                    mainControl.btnHome.IsEnabled = true;
                    mainControl.btnMenu.IsEnabled = true;
                    mainControl.Introduction.IsEnabled = true;
                    mainControl.sellPart.IsEnabled = true;
                    mainControl.buyPart.IsEnabled = true;
                    mainControl.toolsPart.IsEnabled = true;
                    mainControl.MainGrid.Background = new SolidColorBrush(Colors.Transparent);
                    mainControl.FincialGrid.Background = new SolidColorBrush(Colors.Transparent);
                }
                parent.Children.Remove(this);
                parent.Visibility = Visibility.Collapsed;
                parent.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            ExitUC();
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            string _oname = oldUser.Text.Trim();
            string _opass = oldPass.Text.Trim();
            string _nname = newUser.Text.Trim();
            string _npass = newPass.Text.Trim();

            if (string.IsNullOrEmpty(_nname) || string.IsNullOrEmpty(_npass) || string.IsNullOrEmpty(_oname) || string.IsNullOrEmpty(_opass))
            {
                MessageBox.Show("لطفا تمامی مقادیر را کامل کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new DataBaseContext())
                {
                    var context = new GenericDataBaseServices<User>(db);

                    var user = db.Users.FirstOrDefault(u => u.Name == _oname);
                    if (user == null)
                    {
                        MessageBox.Show("نام کاربری یا رمز عبور اشتباه است", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    bool isAuthenticated = HashPassword.VerifyPassword(_opass, user.Password);
                    if (isAuthenticated)
                    {
                        var userUpdate = await context.GetId(1);

                        if (userUpdate == null)
                        {
                            MessageBox.Show("خطا", "شخص مورد نظر پیدا نشد", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        userUpdate.Name = _nname;
                        userUpdate.Password = HashPassword.PasswordHash(_npass);

                        var update = await context.Update(1, userUpdate);
                        if (user == null)
                        {
                            MessageBox.Show("مشکل در تغییر نام کاربری و کلمه عبور", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        MessageBox.Show("اطلاعات به درستی بروز رسانی شد", "موفقیت", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("نام کاربری یا رمز عبور اشتباه است!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ExitUC();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            oldUser.Focus();
        }

        private void oldUser_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                e.Handled = true;
                oldPass.Focus();
                oldPass.SelectAll();
            }

            if (e.Key == Key.Down)
            {
                e.Handled = true;
                oldPass.Focus();
                oldPass.SelectAll();
            }

            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                cancel_Click(cancel, null);
            }
        }

        private void oldPass_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                newUser.Focus();
                newUser.SelectAll();
            }

            if (e.Key == Key.Down)
            {
                e.Handled = true;
                newUser.Focus();
                newUser.SelectAll();
            }

            if (e.Key == Key.Up)
            {
                e.Handled = true;
                oldUser.Focus();
                oldUser.SelectAll();
            }

            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                cancel_Click(cancel, null);
            }
        }

        private void newUser_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                newPass.Focus();
                newPass.SelectAll();
            }

            if (e.Key == Key.Down)
            {
                e.Handled = true;
                newPass.Focus();
                newPass.SelectAll();
            }

            if (e.Key == Key.Up)
            {
                e.Handled = true;
                oldPass.Focus();
                oldPass.SelectAll();
            }

            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                cancel_Click(cancel, null);
            }
        }

        private void newPass_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                save_Click(save, null);
            }

            if (e.Key == Key.Up)
            {
                e.Handled = true;
                newUser.Focus();
                newUser.SelectAll();
            }

            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                cancel_Click(cancel, null);
            }
        }
    }
}
