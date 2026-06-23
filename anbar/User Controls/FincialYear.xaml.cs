using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using anbar.Data.Models;
using anbar.Data.Services;
using anbar.Tools;
using anbar.Windows;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for FincialYear.xaml
    /// </summary>
    public partial class FincialYear : UserControl
    {
        public Action Save { get; set; }
        public FincialYear()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            using (var db = new DataBaseContext())
            {
                int dateYear = db.financials.Select(c => c.ShowYear).FirstOrDefault();
                txtYear.Text = dateYear.ToString();
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

        private void Close()
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
                    mainControl.MenuGrid.Background = new SolidColorBrush(Colors.Transparent);
                }
                parent.Children.Remove(this);
                parent.Visibility = Visibility.Collapsed;
                parent.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private async void search_Click(object sender, RoutedEventArgs e)
        {
            PersianCalendar persian = new PersianCalendar();
            DateTime now = DateTime.Now;

            var maxYear = persian.GetYear(now);
            var maxMonth = persian.GetMonth(now);

            if (!int.TryParse(txtYear.Text,out int search) || search < 1)
            {
                MessageBox.Show("لطفا داخل بخش مربوطه از عدد استفاده کنید", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<User>(db);
                var user = await context.GetId(1);

                if (user == null)
                {
                    MessageBox.Show("کاربری یافت نشد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int checkYear = user.Year;
                int checkMonth = user.Month;

                if (search < checkYear)
                {
                    MessageBox.Show("در این سال سال مالی وجود ندارد", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (search > maxYear + 1 || (search == maxYear + 1 && maxMonth < 12))
                {
                    MessageBox.Show("در این سال سال مالی وجود ندارد", "هشدار", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    var services = new GenericDataBaseServices<FinancialYear>(db);
                    var up = await services.GetId(1);

                    if (up == null)
                    {
                        MessageBox.Show("سال مالی یافت نشد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    up.ShowYear = search;

                    await services.Update(1, up);
                    Save?.Invoke();
                }
            }
            Close();
        }

        private async void add_Click(object sender, RoutedEventArgs e)
        {
            PersianCalendar persian = new PersianCalendar();
            DateTime now = DateTime.Now;

            var maxYear = persian.GetYear(now);
            var maxMonth = persian.GetMonth(now);

            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<FinancialYear>(db);
                var update = await context.GetId(1);
                if (update == null)
                {
                    MessageBox.Show("سال مالی یافت نشد!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int change = update.LastYear;
                if (change + 1 == maxYear || (change == maxYear && maxMonth == 12))
                {
                    update.LastYear = change + 1;
                    update.ShowYear = change + 1;

                    await context.Update(1, update);
                    Save?.Invoke();
                    MessageBox.Show($"سال مالی شما به سال {change + 1} منتقل شد", "خطا", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("نمیتوان سال مالی را عوض کرد", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            Close();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
