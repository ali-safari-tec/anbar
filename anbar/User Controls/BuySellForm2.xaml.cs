using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for BuySellForm2.xaml
    /// </summary>
    public partial class BuySellForm2 : UserControl
    {
        public ObservableCollection<BuyInvoiceItem> buyInvoiceItem { get; set; }

        public static readonly DependencyProperty TitleBuyProperty = DependencyProperty.Register("TitleBuy2", typeof(string), typeof(BuySellForm), new PropertyMetadata(""));
        public Action BuyInvoiceSave2 { get; set; }
        public BuyInvoice BuyInvoiceToEdit2 { get; set; }

        public string TitleBuy2
        {
            get { return (string)GetValue(TitleBuyProperty); }
            set { SetValue(TitleBuyProperty, value); }
        }

        public BuySellForm2()
        {
            InitializeComponent();
            buyInvoiceItem = new ObservableCollection<BuyInvoiceItem>();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (BuyInvoiceToEdit2 != null)
            {

                txtinvoiceId.Text = BuyInvoiceToEdit2.ChangeId.ToString("D5");
                txtNameS.Text = BuyInvoiceToEdit2.BuyInvoiceSeller;
                txtAddress.Text = BuyInvoiceToEdit2.BuyInvoiceAddress;
                txtPhone.Text = BuyInvoiceToEdit2.BuyInvoicePhone;
                txtPhoneCode.Text = BuyInvoiceToEdit2.BuyInvoicePhoneCode;
                txtDay.Text = BuyInvoiceToEdit2.DateDay.ToString("00");
                txtMonth.Text = BuyInvoiceToEdit2.DateMonth.ToString("00");
                txtYear.Text = BuyInvoiceToEdit2.DateYear.ToString();
                decimal total = BuyInvoiceToEdit2.BuyInvoiceTotalCost;
                txtfinalPrice.Text = MoneyConverter.Money(total);
                decimal discount = BuyInvoiceToEdit2.BuyInvoiceDiscount;
                txtdiscount.Text = MoneyConverter.Money(discount);
                txtfinalPay.Text = MoneyConverter.Money(total - discount);
                txtdetail.Text = BuyInvoiceToEdit2.Detail;

                using (var db = new DataBaseContext())
                {
                    var invoiceItemsFromDb = db.buyInvoiceItems
                                               .Where(i => i.BuyInvoiceId == BuyInvoiceToEdit2.BuyInvoiceId) // فقط آیتم‌های این فاکتور
                                               .ToList();

                    buyInvoiceItem = new ObservableCollection<BuyInvoiceItem>(invoiceItemsFromDb);
                }

                // مقداردهی DataGrid برای نمایش آیتم‌ها
                myDataGrid.ItemsSource = buyInvoiceItem;
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

        private void back_Click(object sender, RoutedEventArgs e)
        {
            CloseControl();
            BuyInvoiceSave2?.Invoke();
        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                back_Click(back, null);
            }
        }
    }
}
