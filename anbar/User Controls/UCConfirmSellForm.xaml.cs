using System;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UCConfirmSellForm.xaml
    /// </summary>
    public partial class UCConfirmSellForm : UserControl
    {
        public static readonly DependencyProperty TitleInvoiceProperty = DependencyProperty.Register("TitleInvoice", typeof(string), typeof(UCConfirmSellForm), new PropertyMetadata(""));
        public ObservableCollection<InvoiceItem> invoiceItems { get; set; }
        public Action InvoiceSave { get; set; }
        public Invoice InvoiceToEdit { get; set; }

        public string TitleInvoice
        {
            get { return (string)GetValue(TitleInvoiceProperty); }
            set { SetValue(TitleInvoiceProperty, value); }
        }

        public UCConfirmSellForm()
        {
            InitializeComponent();
            invoiceItems = new ObservableCollection<InvoiceItem>();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (InvoiceToEdit != null)
            {

                txtinvoiceId.Text = InvoiceToEdit.ChangeId.ToString("D5");
                txtNameS.Text = InvoiceToEdit.InvoiceSeller;
                txtNameB.Text = InvoiceToEdit.InvoiceBuyer;
                txtAddress.Text = InvoiceToEdit.InvoiceAddress;
                txtPhone.Text = InvoiceToEdit.InvoicePhone;
                txtPhoneCode.Text = InvoiceToEdit.InvoicePhoneCode;
                txtDay.Text = InvoiceToEdit.DateDay.ToString("00");
                txtMonth.Text = InvoiceToEdit.DateMonth.ToString("00");
                txtYear.Text = InvoiceToEdit.DateYear.ToString();
                decimal total = InvoiceToEdit.InvoiceTotalCost;
                txtfinalPrice.Text = MoneyConverter.Money(total);
                decimal discount = InvoiceToEdit.InvoiceDiscount;
                txtdiscount.Text = MoneyConverter.Money(discount);
                txtfinalPay.Text = MoneyConverter.Money(total - discount);
                txtdetail.Text = InvoiceToEdit.Detail;
                txtdetail2.Text = InvoiceToEdit.Detail2;

                using (var db = new DataBaseContext())
                {
                    var invoiceItemsFromDb = db.invoiceItems
                                               .Where(i => i.InvoiceId == InvoiceToEdit.InvoiceId) // فقط آیتم‌های این فاکتور
                                               .ToList();

                    invoiceItems = new ObservableCollection<InvoiceItem>(invoiceItemsFromDb);
                }

                // مقداردهی DataGrid برای نمایش آیتم‌ها
                myDataGrid.ItemsSource = invoiceItems;
            }
            Keyboard.Focus(this);
            this.Focus();
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
            InvoiceSave?.Invoke();

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
