using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for PaintPrice.xaml
    /// </summary>
    public partial class PaintPrice : UserControl
    {
        public class PaintFee
        {
            public int Number { get; set; }
            public double Size { get; set; }
            public double Weight { get; set; }
            public double Price { get; set; }
        }

        public ObservableCollection<PaintFee> paintFee { get; set; } = new ObservableCollection<PaintFee>();

        public PaintPrice()
        {
            InitializeComponent();
            DataContext = this;
        }

        private double CalCulate(double diameter)
        {
            Dictionary<double, double> weight = new Dictionary<double, double>()
            {
                {10.2, 0.00637},
                {11.8, 0.00853},
                {12, 0.00911},
                {12.5, 0.00957},
                {13.2, 0.0107},
                {14, 0.0124},
                {14.5, 0.0129},
                {14.8, 0.0134},
                {15.8, 0.0153},
                {16, 0.0161},
                {17.2, 0.0181},
                {17.8, 0.0194},
                {18, 0.0203},
                {20, 0.025},
                {21.8, 0.0291},
                {22, 0.0302},
                {23.5, 0.0341},
                {24, 0.0359},
                {27, 0.0447},
                {28, 0.0487},
                {30, 0.0559},
                {32, 0.0635},
            };
            return weight.ContainsKey(diameter) ? weight[diameter] : 0.0;
        }

        private void drop_Click(object sender, RoutedEventArgs e)
        {
            if (panel.Visibility == Visibility.Visible)
            {
                panel.Visibility = Visibility.Collapsed;
            }
            else
            {
                panel.Visibility = Visibility.Visible;
                b10_2.Focus();
            }
        }

        private void b10_2_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "10.2";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b11_8_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "11.8";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b12_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "12";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b12_5_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "12.5";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b13_2_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "13.2";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b14_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "14";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b14_5_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "14.5";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b14_8_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "14.8";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b15_8_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "15.8";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b16_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "16";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b17_2_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "17.2";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b17_8_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "17.8";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b18_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "18";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b20_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "20";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b21_8_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "21.8";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b22_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "22";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b23_5_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "23.5";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b24_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "24";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b27_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "27";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b28_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "28";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b30_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "30";
            panel.Visibility = Visibility.Collapsed;
        }

        private void b32_Click(object sender, RoutedEventArgs e)
        {
            diameter.Text = "32";
            panel.Visibility = Visibility.Collapsed;
        }

        private void diameter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (diameter.Text != "10.2" && diameter.Text != "11.8" && diameter.Text != "12" && diameter.Text != "12.5" && diameter.Text != "13.2" && diameter.Text != "14" &&
                diameter.Text != "14.5" && diameter.Text != "14.8" && diameter.Text != "15.8" && diameter.Text != "16" && diameter.Text != "17.2" && diameter.Text != "17.8" &&
                diameter.Text != "18" && diameter.Text != "20" && diameter.Text != "21.8" && diameter.Text != "22" & diameter.Text != "23.5" && diameter.Text != "24" &&
                diameter.Text != "27" && diameter.Text != "28" && diameter.Text != "30" && diameter.Text != "32")
            {
                border.BorderThickness = new Thickness(3);
                border.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                border.BorderThickness = new Thickness(0);
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (diameter.Text != "10.2" && diameter.Text != "11.8" && diameter.Text != "12" && diameter.Text != "12.5" && diameter.Text != "13.2" && diameter.Text != "14" &&
                diameter.Text != "14.5" && diameter.Text != "14.8" && diameter.Text != "15.8" && diameter.Text != "16" && diameter.Text != "17.2" && diameter.Text != "17.8" &&
                diameter.Text != "18" && diameter.Text != "20" && diameter.Text != "21.8" && diameter.Text != "22" & diameter.Text != "23.5" && diameter.Text != "24" &&
                diameter.Text != "27" && diameter.Text != "28" && diameter.Text != "30" && diameter.Text != "32")
            {
                MessageBox.Show("لطفا یکی از مقادیر مجاز را انتخاب کنید", "اخطار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(diameter.Text) && string.IsNullOrEmpty(diameter.Text) && string.IsNullOrEmpty(diameter.Text))
            {
                MessageBox.Show("لطفا تمامی فیلد ها را پر کنید", "اخطار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {

                int number1 = int.Parse(number.Text);
                double size1 = double.Parse(size.Text);
                double diameter1 = double.Parse(diameter.Text);

                double weight = CalCulate(diameter1);
                double price = number1 * size1 * weight;

                // اضافه کردن به لیست
                paintFee.Add(new PaintFee
                {
                    Number = number1,
                    Size = size1,
                    Weight = weight,
                    Price = price
                });

                // پاک کردن مقادیر ورودی بعد از افزودن
                number.Clear();
                size.Clear();
            }
        }

        private void show_Click(object sender, RoutedEventArgs e)
        {
            double price1 = double.Parse(price.Text);
            double weight = paintFee.Sum(item => item.Price);
            double total = price1 * weight;
            result.Text = total.ToString("N0", new System.Globalization.CultureInfo("fa-IR")) + " ریال ";
        }

        private void price_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = price.Text.Replace(",", ""); // حذف کاماهای قبلی
            if (double.TryParse(text, out double value))
            {
                formattedText.Text = $"{value:N0} ریال";
            }
            else
            {
                formattedText.Text = "";
            }
        }

        private void OrderSelected()
        {
            if (data.Items.Count > 0)
            {
                data.SelectedIndex = data.Items.Count - 1;
            }
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = data.SelectedItem as PaintFee;
            if (selectedItem != null)
            {
                paintFee.Remove(selectedItem);
                OrderSelected();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            size.Focus();
        }

        private void diameter_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                price.Focus();
                price.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                number.Focus();
                number.SelectAll();
            }
            if (e.Key == Key.A)
            {
                e.Handled = true;
                drop_Click(drop, null);
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                add_Click(add, null);
                size.Focus();
            }
        }

        private void number_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                diameter.Focus();
                diameter.SelectAll();
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                diameter.Focus();
                diameter.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                size.Focus();
                size.SelectAll();
            }
        }

        private void size_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                number.Focus();
                number.SelectAll();
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                number.Focus();
                number.SelectAll();
            }
        }

        private void price_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                diameter.Focus();
                diameter.SelectAll();
            }
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                show_Click(show, null);
                size.Focus();
            }
        }
    }
}
