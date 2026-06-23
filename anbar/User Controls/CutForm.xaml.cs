using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using anbar.Tools;
using static anbar.Tools.Calculater;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for CutForm.xaml
    /// </summary>
    public partial class CutForm : UserControl
    {
        public ObservableCollection<Order> orders { get; set; } = new ObservableCollection<Order>();
        public ObservableCollection<HalfBar> halfBars { get; set; } = new ObservableCollection<HalfBar>();

        public CutForm()
        {
            InitializeComponent();
            LeftGrid.ItemsSource = halfBars;
            OrderGrid.ItemsSource = orders;
            LeftSelected();
            OrderSelected();
        }

        private void Button_Delete_Left(object sender, RoutedEventArgs e)
        {
            var selectedItem = LeftGrid.SelectedItem as HalfBar;
            if (selectedItem != null)
            {
                halfBars.Remove(selectedItem);
                LeftSelected();
            }
        }

        private void Button_Add_Left(object sender, RoutedEventArgs e)
        {
            halfBars.Add(new HalfBar { LengthH = 0, QuantityH = 0 });
            LeftSelected();
        }

        private void Button_Delete_Order(object sender, RoutedEventArgs e)
        {
            var selectedItem = OrderGrid.SelectedItem as Order;
            if (selectedItem != null)
            {
                orders.Remove(selectedItem);
                OrderSelected();
            }
        }

        private void Button_Add_Order(object sender, RoutedEventArgs e)
        {
            orders.Add(new Order { Length = 0, Quantity = 0 });
            OrderSelected();
        }

        private void Button_Result(object sender, RoutedEventArgs e)
        {
            try
            {
                double numberOfBars = Convert.ToDouble(BarNumber.Text);
                double barLength = Convert.ToDouble(BarLength.Text);
                double minLength = Convert.ToDouble(MinLength.Text);
                double maxLength = Convert.ToDouble(MaxLength.Text);

                // جمع‌آوری داده‌های وارد شده در Gridها
                List<HalfBar> currentHalfBars = new List<HalfBar>();
                foreach (var item in LeftGrid.Items)
                {
                    if (item is HalfBar halfBar)
                    {
                        currentHalfBars.Add(halfBar);
                    }
                }

                List<Order> currentOrders = new List<Order>();
                foreach (var item in OrderGrid.Items)
                {
                    if (item is Order order)
                    {
                        currentOrders.Add(order);
                    }
                }

                // ایجاد کپی عمیق از halfBars و orders
                List<HalfBar> initialHalfBars = currentHalfBars
                    .Select(h => new HalfBar { LengthH = h.LengthH, QuantityH = h.QuantityH })
                    .ToList();
                List<Order> initialOrders = currentOrders
                    .Select(o => new Order { Length = o.Length, Quantity = o.Quantity })
                    .ToList();

                // استفاده از کلاس Calculater برای بهینه‌سازی
                Calculater.CuttingOptimizer optimizer = new Calculater.CuttingOptimizer();
                List<Calculater.BarPlan> plans = optimizer.OptimizeCutting(numberOfBars, barLength, initialOrders, initialHalfBars, minLength, maxLength);

                // نمایش نتایج در TextBlock
                StringBuilder resultText = new StringBuilder();
                foreach (var plan in plans)
                {
                    resultText.AppendLine($"شاخه {plan.BarIndex} :     مجموع طول محاسبه شده : {plan.Sum}     ته شاخه باقی مانده : {plan.Leftover}");
                    foreach (var cut in plan.Cuts)
                    {
                        resultText.AppendLine($"  سایز برش : {cut.Key}    x    تعداد برش : {cut.Value}");
                    }
                    resultText.AppendLine();
                }

                Result.Text = resultText.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LeftSelected()
        {
            if (LeftGrid.Items.Count > 0)
            {
                LeftGrid.SelectedIndex = LeftGrid.Items.Count - 1;
            }
        }

        private void OrderSelected()
        {
            if (OrderGrid.Items.Count > 0)
            {
                OrderGrid.SelectedIndex = OrderGrid.Items.Count - 1;
            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            BarNumber.Focus();
        }

        private void BarNumber_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                BarLength.Focus();
                BarLength.SelectAll();
            }
        }

        private void BarLength_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                MinLength.Focus();
                MinLength.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                BarNumber.Focus();
                BarNumber.SelectAll();
            }
        }

        private void MinLength_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                e.Handled = true;
                MaxLength.Focus();
                MaxLength.SelectAll();
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                BarLength.Focus();
                BarLength.SelectAll();
            }
        }

        private void MaxLength_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                MinLength.Focus();
                MinLength.SelectAll();
            }
        }

        private void Border_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Button_Result(Button, null);
                BarNumber.Focus();
                BarNumber.SelectAll();
            }
        }
    }
}
