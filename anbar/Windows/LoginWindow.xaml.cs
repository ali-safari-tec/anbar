using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using anbar.Data.Services;
using anbar.Tools;

namespace anbar.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            using (var db = new DataBaseContext())
            {
                db.Database.Initialize(true);
                db.SeedDatabase();
            }
        }

        private void Exit_Button(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txt.Text == "Enter Your Username")
            {
                txt.Text = string.Empty;
                txt.Foreground = new SolidColorBrush(Colors.White);
                txt.Opacity = 1;
                txt.FontSize = 22;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                txt.Text = "Enter Your Username";
                txt.Foreground = new SolidColorBrush(Colors.LightGray);
                txt.Opacity = 0.5;
                txt.FontSize = 12;
            }
        }

        private void pwb_GotFocus(object sender, RoutedEventArgs e)
        {
            txb.Visibility = Visibility.Collapsed;
        }

        private void pwb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwb.Password))
            {
                txb.Visibility = Visibility.Visible;
            }
        }

        private void pwb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwb.Password) && pwb.IsFocused == false)
            {
                txb.Visibility = Visibility.Visible;
            }
            if (string.IsNullOrEmpty(pwb.Password))
            {
                btninVisible.Visibility = Visibility.Collapsed;
                btnVisible.Visibility = Visibility.Collapsed;
            }
            if (!string.IsNullOrEmpty(pwb.Password))
            {
                btnVisible.Visibility = Visibility.Visible;
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            rec.Fill = new SolidColorBrush(Colors.LightGray);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            rec.Fill = new SolidColorBrush(Colors.Black);
        }

        private void btninVisible_Click(object sender, RoutedEventArgs e)
        {
            pwb.Password = txt2.Text;
            pwb.Visibility = Visibility.Visible;
            txt2.Visibility = Visibility.Collapsed;
            btnVisible.Visibility = Visibility.Visible;
            btninVisible.Visibility = Visibility.Collapsed;
        }

        private void btnVisible_Click(object sender, RoutedEventArgs e)
        {
            txt2.Text = pwb.Password;
            pwb.Visibility = Visibility.Collapsed;
            txt2.Visibility = Visibility.Visible;
            btnVisible.Visibility = Visibility.Collapsed;
            btninVisible.Visibility = Visibility.Visible;
            txt2.Focus();
            txt2.SelectAll();
        }

        private void txt2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt2.Text))
            {
                btninVisible.Visibility = Visibility.Collapsed;
                btnVisible.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(txt2.Text) && txt2.Visibility == Visibility.Visible)
            {
                txt2.Visibility = Visibility.Collapsed;
                pwb.Password = string.Empty;
                pwb.Visibility = Visibility.Visible;
                pwb.Focus();
            }
        }

        private void btninVisible_MouseEnter(object sender, MouseEventArgs e)
        {
            btninVisible.Opacity = 0.5;
        }

        private void btninVisible_MouseLeave(object sender, MouseEventArgs e)
        {
            btninVisible.Opacity = 1;
        }

        private void btnVisible_MouseEnter(object sender, MouseEventArgs e)
        {
            btnVisible.Opacity = 0.5;
        }

        private void btnVisible_MouseLeave(object sender, MouseEventArgs e)
        {
            btnVisible.Opacity = 1;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string username = txt.Text.Trim();
            string password = (pwb.Visibility == Visibility.Visible ? pwb.Password.Trim() : txt2.Text.Trim());

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("لطفاً نام کاربری و رمز عبور را وارد کنید.", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new DataBaseContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Name == username);
                if (user == null)
                {
                    MessageBox.Show("کاربری با این نام یافت نشد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txt.Text = string.Empty;
                    txt2.Text = string.Empty;
                    pwb.Password = string.Empty;
                    txt.Focus();
                    return;
                }

                bool isAuthenticated = HashPassword.VerifyPassword(password, user.Password);
                if (isAuthenticated)
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("نام کاربری یا رمز عبور اشتباه است!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    txt.Text = string.Empty;
                    txt2.Text = string.Empty;
                    pwb.Password = string.Empty;
                    txt.Focus();
                    return;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            else if (e.Key == Key.Enter)
            {
                if (pwb.IsFocused)
                {
                    button_Click(button, null);
                }
                else if (pwb.Visibility == Visibility.Visible)
                {
                    pwb.Focus();
                }
                else if (txt2.Visibility == Visibility.Visible && txt2.IsFocused)
                {
                    button_Click(button, null);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt.Focus();
        }
    }
}
