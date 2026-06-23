using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using anbar.Data.Models;
using anbar.Data.Services;
using anbar.Tools;

namespace anbar.User_Controls
{
    /// <summary>
    /// Interaction logic for PriceListO1.xaml
    /// </summary>
    public partial class PriceListO1 : UserControl
    {
        private FontSizeViewModel ViewModel {  get; set; } = new FontSizeViewModel();

        public PriceListO1()
        {
            InitializeComponent();
            this.DataContext = new FontSizeViewModel();
            DataContext = ViewModel;
            Load();
        }
        private async void Load()
        {
            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<ListO1>(db);
                var existingRecord = await db.listO1s.FirstOrDefaultAsync(x => x.ListO1Id == 1);
                if (existingRecord != null)
                {
                    var update = await context.GetId(1);

                    p11.Text = update.O1P11.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p12.Text = update.O1P12.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p14.Text = update.O1P14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    pm14.Text = update.O1PM14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    pm16.Text = update.O1PM16.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    pm18.Text = update.O1PM18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    po18.Text = update.O1PO18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    po19.Text = update.O1PO19.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    a8.Text = update.O1G11.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    b8.Text = update.O1G12.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    c8.Text = update.O1G14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    d8.Text = update.O1GM14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    e8.Text = update.O1GM16.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    f8.Text = update.O1GM18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    g8.Text = update.O1GO18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    h8.Text = update.O1GO19.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    d.Text = update.O1day.ToString("00");
                    m.Text = update.O1month.ToString("00");
                    y.Text = update.O1year.ToString();
                }
                else
                {
                    PersianCalendar persianCalendar = new PersianCalendar();
                    DateTime now = DateTime.Now;

                    ListO1 listO1 = new ListO1()
                    {
                        ListO1Id = 1,
                        O1P11 = 0,
                        O1P12 = 0,
                        O1P14 = 0,
                        O1PM14 = 0,
                        O1PM16 = 0,
                        O1PM18 = 0,
                        O1PO18 = 0,
                        O1PO19 = 0,
                        O1G11 = 0,
                        O1G12 = 0,
                        O1G14 = 0,
                        O1GM14 = 0,
                        O1GM16 = 0,
                        O1GM18 = 0,
                        O1GO18 = 0,
                        O1GO19 = 0,
                        O1day = persianCalendar.GetDayOfMonth(now),
                        O1month = persianCalendar.GetMonth(now),
                        O1year = persianCalendar.GetYear(now)
                    };

                    await context.Create(listO1);

                    p11.Text = listO1.O1P11.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p12.Text = listO1.O1P12.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p14.Text = listO1.O1P14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    pm14.Text = listO1.O1PM14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    pm16.Text = listO1.O1PM16.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    pm18.Text = listO1.O1PM18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    po18.Text = listO1.O1PO18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    po19.Text = listO1.O1PO19.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    a8.Text = listO1.O1G11.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    b8.Text = listO1.O1G12.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    c8.Text = listO1.O1G14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    d8.Text = listO1.O1GM14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    e8.Text = listO1.O1GM16.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    f8.Text = listO1.O1GM18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    g8.Text = listO1.O1GO18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    h8.Text = listO1.O1GO19.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    d.Text = listO1.O1day.ToString("00");
                    m.Text = listO1.O1month.ToString("00");
                    y.Text = listO1.O1year.ToString();
                }
            }
        }

        private void Toggle()
        {
            Dictionary<TextBox, double> textBoxValues = new Dictionary<TextBox, double>();

            // لیست تکس‌باکس‌هایی که باید فرمت روی آن‌ها اعمال شود
            List<TextBox> formattedTextBoxes = new List<TextBox>()
            {
                a8 , b8 , c8 , d8 , e8 , f8 , g8 , h8 ,
                p11 , p12 , p14 , pm14 , pm16 , pm18 , po18 , po19
            };

            // مقداردهی اولیه دیکشنری (فقط یکبار در ابتدای اجرا)
            void InitializeTextBoxValues()
            {
                foreach (var textBox in formattedTextBoxes)
                {
                    if (double.TryParse(textBox.Text, out double value))
                    {
                        textBoxValues[textBox] = value;
                    }
                }
            }

            // به‌روزرسانی مقادیر تکس‌باکس‌ها در دیکشنری (قبل از اعمال فرمت)
            void UpdateTextBoxValues()
            {
                foreach (var textBox in formattedTextBoxes)
                {
                    if (double.TryParse(textBox.Text, out double value))
                    {
                        textBoxValues[textBox] = value;
                    }
                }
            }

            // تابع نمایش با فرمت
            void ApplyFormattedNumbers()
            {
                UpdateTextBoxValues(); // مقدارهای جدید را ذخیره کن
                foreach (var textBox in formattedTextBoxes)
                {
                    if (textBoxValues.ContainsKey(textBox))
                    {
                        textBox.Text = textBoxValues[textBox].ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    }
                }
            }

            // تابع نمایش بدون فرمت
            void ApplyPlainNumbers()
            {
                UpdateTextBoxValues(); // مقدارهای جدید را ذخیره کن
                foreach (var textBox in formattedTextBoxes)
                {
                    if (textBoxValues.ContainsKey(textBox))
                    {
                        textBox.Text = textBoxValues[textBox].ToString();
                    }
                }
            }

            // مقداردهی اولیه دیکشنری هنگام لود شدن فرم
            InitializeTextBoxValues();

            // دکمه اول برای نمایش با فرمت
            save.Click += (s, e) => ApplyFormattedNumbers();

            // دکمه دوم برای نمایش بدون فرمت
            edit.Click += (s, e) => ApplyPlainNumbers();

        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> textBoxes = new List<TextBox>()
            {
                p11, p12, p14, pm14, pm16, pm18, po18, po19,
                a8, b8, c8, d8, e8, f8, g8, h8,
                d , m , y
            };

            // استفاده از حلقه برای اعمال تنظیمات به تکس‌باکس‌ها
            foreach (var textBox in textBoxes)
            {
                textBox.IsReadOnly = false;
                textBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD0CCED"));
            }

            Toggle();
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {

            using (var db = new DataBaseContext())
            {
                var context = new GenericDataBaseServices<ListO1>(db);
                var update = await context.GetId(1);

                update.O1P11 = decimal.Parse(p11.Text);
                update.O1P12 = decimal.Parse(p12.Text);
                update.O1P14 = decimal.Parse(p14.Text);
                update.O1PM14 = decimal.Parse(pm14.Text);
                update.O1PM16 = decimal.Parse(pm16.Text);
                update.O1PM18 = decimal.Parse(pm18.Text);
                update.O1PO18 = decimal.Parse(po18.Text);
                update.O1PO19 = decimal.Parse(po19.Text);
                update.O1G11 = decimal.Parse(a8.Text);
                update.O1G12 = decimal.Parse(b8.Text);
                update.O1G14 = decimal.Parse(c8.Text);
                update.O1GM14 = decimal.Parse(d8.Text);
                update.O1GM16 = decimal.Parse(e8.Text);
                update.O1GM18 = decimal.Parse(f8.Text);
                update.O1GO18 = decimal.Parse(g8.Text);
                update.O1GO19 = decimal.Parse(h8.Text);
                update.O1day = int.Parse(d.Text);
                update.O1month = int.Parse(m.Text);
                update.O1year = int.Parse(y.Text);

                await context.Update(1, update);
            }
            Load();

            List<TextBox> textBoxes = new List<TextBox>()
            {
                p11, p12, p14, pm14, pm16, pm18, po18, po19,
                a8, b8, c8, d8, e8, f8, g8, h8,
                d , m , y
            };

            // استفاده از حلقه برای اعمال تنظیمات به تکس‌باکس‌ها
            foreach (var textBox in textBoxes)
            {
                textBox.IsReadOnly = true;
                textBox.Background = new SolidColorBrush(Colors.Transparent);
            }

            Toggle();
        }

        private void a8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(a8.Text, out double initialPrice) || !double.TryParse(p11.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                a10, a12, a14, a16, a18, a20, a22, a24, a25, a26, a28,
                a30, a32, a34, a35, a36, a38, a40, a42, a43, a44, a45,
                a46, a48, a50, a52, a54, a55, a56, a58, a60, a65, a70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == a65 || textBoxes[i] == a70)
                {
                    lastValue += (jumpPrice * 1) + (jumpPrice / 2);
                }
                if (textBoxes[i] == a25 || textBoxes[i] == a35 || textBoxes[i] == a43 || textBoxes[i] == a45 || textBoxes[i] == a55)
                {
                    lastValue += (jumpPrice / 2);
                    halfJumpAdded = true;
                }
                else
                {
                    if (halfJumpAdded)
                    {
                        lastValue += (jumpPrice / 2);
                        halfJumpAdded = false;
                    }
                    else
                    {
                        lastValue += jumpPrice;
                    }
                }

                textBoxes[i].Text = lastValue.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
            }
        }

        private void b8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(b8.Text, out double initialPrice) || !double.TryParse(p12.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                b10, b12, b14, b16, b18, b20, b22, b24, b25, b26, b28,
                b30, b32, b34, b35, b36, b38, b40, b42, b43, b44, b45,
                b46, b48, b50, b52, b54, b55, b56, b58, b60, b65, b70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == b65 || textBoxes[i] == b70)
                {
                    lastValue += (jumpPrice * 1) + (jumpPrice / 2);
                }
                if (textBoxes[i] == b25 || textBoxes[i] == b35 || textBoxes[i] == b43 || textBoxes[i] == b45 || textBoxes[i] == b55)
                {
                    lastValue += (jumpPrice / 2);
                    halfJumpAdded = true;
                }
                else
                {
                    if (halfJumpAdded)
                    {
                        lastValue += (jumpPrice / 2);
                        halfJumpAdded = false;
                    }
                    else
                    {
                        lastValue += jumpPrice;
                    }
                }

                textBoxes[i].Text = lastValue.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
            }
        }

        private void c8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(c8.Text, out double initialPrice) || !double.TryParse(p14.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                c10, c12, c14, c16, c18, c20, c22, c24, c25, c26, c28,
                c30, c32, c34, c35, c36, c38, c40, c42, c43, c44, c45,
                c46, c48, c50, c52, c54, c55, c56, c58, c60, c65, c70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == c65 || textBoxes[i] == c70)
                {
                    lastValue += (jumpPrice * 1) + (jumpPrice / 2);
                }
                if (textBoxes[i] == c25 || textBoxes[i] == c35 || textBoxes[i] == c43 || textBoxes[i] == c45 || textBoxes[i] == c55)
                {
                    lastValue += (jumpPrice / 2);
                    halfJumpAdded = true;
                }
                else
                {
                    if (halfJumpAdded)
                    {
                        lastValue += (jumpPrice / 2);
                        halfJumpAdded = false;
                    }
                    else
                    {
                        lastValue += jumpPrice;
                    }
                }

                textBoxes[i].Text = lastValue.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
            }
        }

        private void d8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(d8.Text, out double initialPrice) || !double.TryParse(pm14.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                d10, d12, d14, d16, d18, d20, d22, d24, d25, d26, d28,
                d30, d32, d34, d35, d36, d38, d40, d42, d43, d44, d45,
                d46, d48, d50, d52, d54, d55, d56, d58, d60, d65, d70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == d65 || textBoxes[i] == d70)
                {
                    lastValue += (jumpPrice * 1) + (jumpPrice / 2);
                }
                if (textBoxes[i] == d25 || textBoxes[i] == d35 || textBoxes[i] == d43 || textBoxes[i] == d45 || textBoxes[i] == d55)
                {
                    lastValue += (jumpPrice / 2);
                    halfJumpAdded = true;
                }
                else
                {
                    if (halfJumpAdded)
                    {
                        lastValue += (jumpPrice / 2);
                        halfJumpAdded = false;
                    }
                    else
                    {
                        lastValue += jumpPrice;
                    }
                }

                textBoxes[i].Text = lastValue.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
            }
        }

        private void e8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(e8.Text, out double initialPrice) || !double.TryParse(pm16.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                e10, e12, e14, e16, e18, e20, e22, e24, e25, e26, e28,
                e30, e32, e34, e35, e36, e38, e40, e42, e43, e44, e45,
                e46, e48, e50, e52, e54, e55, e56, e58, e60, e65, e70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == e65 || textBoxes[i] == e70)
                {
                    lastValue += (jumpPrice * 1) + (jumpPrice / 2);
                }
                if (textBoxes[i] == e25 || textBoxes[i] == e35 || textBoxes[i] == e43 || textBoxes[i] == e45 || textBoxes[i] == e55)
                {
                    lastValue += (jumpPrice / 2);
                    halfJumpAdded = true;
                }
                else
                {
                    if (halfJumpAdded)
                    {
                        lastValue += (jumpPrice / 2);
                        halfJumpAdded = false;
                    }
                    else
                    {
                        lastValue += jumpPrice;
                    }
                }

                textBoxes[i].Text = lastValue.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
            }
        }

        private void f8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(f8.Text, out double initialPrice) || !double.TryParse(pm18.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                f10, f12, f14, f16, f18, f20, f22, f24, f25, f26, f28,
                f30, f32, f34, f35, f36, f38, f40, f42, f43, f44, f45,
                f46, f48, f50, f52, f54, f55, f56, f58, f60, f65, f70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == f65 || textBoxes[i] == f70)
                {
                    lastValue += (jumpPrice * 1) + (jumpPrice / 2);
                }
                if (textBoxes[i] == f25 || textBoxes[i] == f35 || textBoxes[i] == f43 || textBoxes[i] == f45 || textBoxes[i] == f55)
                {
                    lastValue += (jumpPrice / 2);
                    halfJumpAdded = true;
                }
                else
                {
                    if (halfJumpAdded)
                    {
                        lastValue += (jumpPrice / 2);
                        halfJumpAdded = false;
                    }
                    else
                    {
                        lastValue += jumpPrice;
                    }
                }

                textBoxes[i].Text = lastValue.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
            }
        }

        private void g8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(g8.Text, out double initialPrice) || !double.TryParse(po18.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                g10, g12, g14, g16, g18, g20, g22, g24, g25, g26, g28,
                g30, g32, g34, g35, g36, g38, g40, g42, g43, g44, g45,
                g46, g48, g50, g52, g54, g55, g56, g58, g60, g65, g70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == g65 || textBoxes[i] == g70)
                {
                    lastValue += (jumpPrice * 1) + (jumpPrice / 2);
                }
                if (textBoxes[i] == g25 || textBoxes[i] == g35 || textBoxes[i] == g43 || textBoxes[i] == g45 || textBoxes[i] == g55)
                {
                    lastValue += (jumpPrice / 2);
                    halfJumpAdded = true;
                }
                else
                {
                    if (halfJumpAdded)
                    {
                        lastValue += (jumpPrice / 2);
                        halfJumpAdded = false;
                    }
                    else
                    {
                        lastValue += jumpPrice;
                    }
                }

                textBoxes[i].Text = lastValue.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
            }
        }

        private void h8_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(h8.Text, out double initialPrice) || !double.TryParse(po19.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                h10, h12, h14, h16, h18, h20, h22, h24, h25, h26, h28,
                h30, h32, h34, h35, h36, h38, h40, h42, h43, h44, h45,
                h46, h48, h50, h52, h54, h55, h56, h58, h60, h65, h70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == h65 || textBoxes[i] == h70)
                {
                    lastValue += (jumpPrice * 1) + (jumpPrice / 2);
                }
                if (textBoxes[i] == h25 || textBoxes[i] == h35 || textBoxes[i] == h43 || textBoxes[i] == h45 || textBoxes[i] == h55)
                {
                    lastValue += (jumpPrice / 2);
                    halfJumpAdded = true;
                }
                else
                {
                    if (halfJumpAdded)
                    {
                        lastValue += (jumpPrice / 2);
                        halfJumpAdded = false;
                    }
                    else
                    {
                        lastValue += jumpPrice;
                    }
                }

                textBoxes[i].Text = lastValue.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
            }
        }

        private void new_Click(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent as Grid;
            if (parent != null)
            {
                parent.Children.Clear();
                var form = new PriceListO();
                parent.Children.Add(form);
            }
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            if (a10.IsReadOnly == true)
            {
                var grid = main; // نام Grid خود را جایگزین کنید

                // مخفی کردن دکمه‌ها و سایر المان‌هایی که نمی‌خواهید در پرینت باشند
                HideButtons();

                // اندازه‌های صفحه A4 در پیکسل (96 DPI)
                double a4Width = 816;
                double a4Height = 1056;

                // رندر کردن Grid به تصویر
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                    (int)grid.ActualWidth, (int)grid.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(grid);

                // تبدیل RenderTargetBitmap به Image برای نمایش در Print Preview
                Image printImage = new Image
                {
                    Source = renderTargetBitmap,
                    Width = a4Width,
                    Height = a4Height,
                    Stretch = Stretch.Uniform
                };

                // ایجاد FixedDocument برای نمایش در Print Preview
                FixedDocument fixedDoc = new FixedDocument();
                PageContent pageContent = new PageContent();
                FixedPage fixedPage = new FixedPage
                {
                    Width = a4Width,
                    Height = a4Height
                };

                // اضافه کردن تصویر به صفحه پرینت
                fixedPage.Children.Add(printImage);
                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDoc.Pages.Add(pageContent);

                // ایجاد Window جدید برای نمایش پیش‌نمایش چاپ
                Window previewWindow = new Window
                {
                    Title = "پیش‌نمایش چاپ",
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Width = 1000,
                    Height = 700,
                    Content = new DocumentViewer { Document = fixedDoc }
                };

                // نمایش پنجره
                previewWindow.ShowDialog();

                // نمایش مجدد دکمه‌ها و عناصر مخفی شده
                ShowButtons();
            }
            else
            {
                save_Click(save, null);
            }
        }

        private void HideButtons()
        {
            edit.Visibility = Visibility.Hidden;
            save.Visibility = Visibility.Hidden;
            new1.Visibility = Visibility.Hidden;
            print.Visibility = Visibility.Hidden;
            dec.Visibility = Visibility.Hidden;
            inc.Visibility = Visibility.Hidden;
            h1.Visibility = Visibility.Hidden;
            h2.Visibility = Visibility.Hidden;
        }

        private void ShowButtons()
        {
            edit.Visibility = Visibility.Visible;
            save.Visibility = Visibility.Visible;
            new1.Visibility = Visibility.Visible;
            print.Visibility = Visibility.Visible;
            dec.Visibility = Visibility.Visible;
            inc.Visibility = Visibility.Visible;
            h1.Visibility = Visibility.Visible;
            h2.Visibility = Visibility.Visible;
        }

        private void IncreaseFontSize(object sender, RoutedEventArgs e)
        {
            ViewModel.FontSize += 2;
        }

        private void DecreaseFontSize(object sender, RoutedEventArgs e)
        {
            ViewModel.FontSize -= 2;
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Add))
            {
                IncreaseFontSize(inc, null);
            }
            if (Keyboard.IsKeyDown(Key.Subtract))
            {
                DecreaseFontSize(dec, null);
            }
        }
    }
}
