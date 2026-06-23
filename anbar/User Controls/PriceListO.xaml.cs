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
    /// Interaction logic for PriceListO.xaml
    /// </summary>
    public partial class PriceListO : UserControl
    {
        private FontSizeViewModel ViewModel { get; set; } = new FontSizeViewModel();

        public PriceListO()
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
                var context = new GenericDataBaseServices<ListO>(db);
                var existingRecord = await db.listOs.FirstOrDefaultAsync(x => x.ListOId == 1);
                if (existingRecord != null)
                {
                    var update = await context.GetId(1);

                    p14.Text = update.OP14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p16.Text = update.OP16.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p18.Text = update.OP18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p19.Text = update.OP19.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p20.Text = update.OP20.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p22.Text = update.OP22.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p24.Text = update.OP24.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p27.Text = update.OP27.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p28.Text = update.OP28.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p30.Text = update.OP30.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p32.Text = update.OP32.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    a10.Text = update.OG14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    b10.Text = update.OG16.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    c10.Text = update.OG18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    d10.Text = update.OG19.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    e10.Text = update.OG20.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    f10.Text = update.OG22.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    g10.Text = update.OG24.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    h10.Text = update.OG27.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    i10.Text = update.OG28.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    j10.Text = update.OG30.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    k10.Text = update.OG32.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    d.Text = update.Oday.ToString("00");
                    m.Text = update.Omonth.ToString("00");
                    y.Text = update.Oyear.ToString();
                }
                else
                {
                    PersianCalendar persianCalendar = new PersianCalendar();
                    DateTime now = DateTime.Now;

                    ListO listO = new ListO()
                    {
                        ListOId = 1,
                        OP14 = 0,
                        OP16 = 0,
                        OP18 = 0,
                        OP19 = 0,
                        OP20 = 0,
                        OP22 = 0,
                        OP24 = 0,
                        OP27 = 0,
                        OP28 = 0,
                        OP30 = 0,
                        OP32 = 0,
                        OG14 = 0,
                        OG16 = 0,
                        OG18 = 0,
                        OG19 = 0,
                        OG20 = 0,
                        OG22 = 0,
                        OG24 = 0,
                        OG27 = 0,
                        OG28 = 0,
                        OG30 = 0,
                        OG32 = 0,
                        Oday = persianCalendar.GetDayOfMonth(now),
                        Omonth = persianCalendar.GetMonth(now),
                        Oyear = persianCalendar.GetYear(now)
                    };
                    await context.Create(listO);

                    p14.Text = listO.OP14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p16.Text = listO.OP16.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p18.Text = listO.OP18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p19.Text = listO.OP19.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p20.Text = listO.OP20.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p22.Text = listO.OP22.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p24.Text = listO.OP24.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p27.Text = listO.OP27.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p28.Text = listO.OP28.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p30.Text = listO.OP30.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    p32.Text = listO.OP32.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    a10.Text = listO.OG14.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    b10.Text = listO.OG16.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    c10.Text = listO.OG18.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    d10.Text = listO.OG19.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    e10.Text = listO.OG20.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    f10.Text = listO.OG22.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    g10.Text = listO.OG24.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    h10.Text = listO.OG27.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    i10.Text = listO.OG28.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    j10.Text = listO.OG30.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    k10.Text = listO.OG32.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
                    d.Text = listO.Oday.ToString("00");
                    m.Text = listO.Omonth.ToString("00");
                    y.Text = listO.Oyear.ToString();
                }
            }
        }

        private void Toggle()
        {
            Dictionary<TextBox, double> textBoxValues = new Dictionary<TextBox, double>();

            // لیست تکس‌باکس‌هایی که باید فرمت روی آن‌ها اعمال شود
            List<TextBox> formattedTextBoxes = new List<TextBox>()
            {
                a10 , b10 , c10 , d10 , e10 , f10 , g10 , h10 , i10 , j10 , k10 ,
                p14 , p16 , p18 , p19 , p20 , p22 , p24 , p27 , p28 , p30 , p32
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
                p14, p16, p18, p19, p20, p22, p24, p27, p28, p30, p32,
                a10, b10, c10, d10, e10, f10, g10, h10, i10, j10, k10,
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
                var context = new GenericDataBaseServices<ListO>(db);
                var update = await context.GetId(1);

                update.OP14 = decimal.Parse(p14.Text);
                update.OP16 = decimal.Parse(p16.Text);
                update.OP18 = decimal.Parse(p18.Text);
                update.OP19 = decimal.Parse(p19.Text);
                update.OP20 = decimal.Parse(p20.Text);
                update.OP22 = decimal.Parse(p22.Text);
                update.OP24 = decimal.Parse(p24.Text);
                update.OP27 = decimal.Parse(p27.Text);
                update.OP28 = decimal.Parse(p28.Text);
                update.OP30 = decimal.Parse(p30.Text);
                update.OP32 = decimal.Parse(p32.Text);
                update.OG14 = decimal.Parse(a10.Text);
                update.OG16 = decimal.Parse(b10.Text);
                update.OG18 = decimal.Parse(c10.Text);
                update.OG19 = decimal.Parse(d10.Text);
                update.OG20 = decimal.Parse(e10.Text);
                update.OG22 = decimal.Parse(f10.Text);
                update.OG24 = decimal.Parse(g10.Text);
                update.OG27 = decimal.Parse(h10.Text);
                update.OG28 = decimal.Parse(i10.Text);
                update.OG30 = decimal.Parse(j10.Text);
                update.OG32 = decimal.Parse(k10.Text);
                update.Oday = int.Parse(d.Text);
                update.Omonth = int.Parse(m.Text);
                update.Oyear = int.Parse(y.Text);

                await context.Update(1, update);
            }
            Load();

            List<TextBox> textBoxes = new List<TextBox>()
            {
                p14, p16, p18, p19, p20, p22, p24, p27, p28, p30, p32,
                a10, b10, c10, d10, e10, f10, g10, h10, i10, j10, k10,
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

        private void a10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(a10.Text, out double initialPrice) || !double.TryParse(p14.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                a12, a14, a16, a18, a20, a22, a24, a26, a28, a30,
                a32, a34, a35, a36, a38, a40, a42, a44, a46, a48,
                a50, a52, a54, a56, a58, a60, a62, a64, a65, a66,
                a68, a70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == a35 || textBoxes[i] == a65)
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

        private void b10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(b10.Text, out double initialPrice) || !double.TryParse(p16.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                b12, b14, b16, b18, b20, b22, b24, b26, b28, b30,
                b32, b34, b35, b36, b38, b40, b42, b44, b46, b48,
                b50, b52, b54, b56, b58, b60, b62, b64, b65, b66,
                b68, b70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == b35 || textBoxes[i] == b65)
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

        private void c10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(c10.Text, out double initialPrice) || !double.TryParse(p18.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                c12, c14, c16, c18, c20, c22, c24, c26, c28, c30,
                c32, c34, c35, c36, c38, c40, c42, c44, c46, c48,
                c50, c52, c54, c56, c58, c60, c62, c64, c65, c66,
                c68, c70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == c35 || textBoxes[i] == c65)
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

        private void d10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(d10.Text, out double initialPrice) || !double.TryParse(p19.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                d12, d14, d16, d18, d20, d22, d24, d26, d28, d30,
                d32, d34, d35, d36, d38, d40, d42, d44, d46, d48,
                d50, d52, d54, d56, d58, d60, d62, d64, d65, d66,
                d68, d70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == d35 || textBoxes[i] == d65)
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

        private void e10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(e10.Text, out double initialPrice) || !double.TryParse(p20.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                e12, e14, e16, e18, e20, e22, e24, e26, e28, e30,
                e32, e34, e35, e36, e38, e40, e42, e44, e46, e48,
                e50, e52, e54, e56, e58, e60, e62, e64, e65, e66,
                e68, e70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == e35 || textBoxes[i] == e65)
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

        private void f10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(f10.Text, out double initialPrice) || !double.TryParse(p22.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                f12, f14, f16, f18, f20, f22, f24, f26, f28, f30,
                f32, f34, f35, f36, f38, f40, f42, f44, f46, f48,
                f50, f52, f54, f56, f58, f60, f62, f64, f65, f66,
                f68, f70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == f35 || textBoxes[i] == f65)
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

        private void g10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(g10.Text, out double initialPrice) || !double.TryParse(p24.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                g12, g14, g16, g18, g20, g22, g24, g26, g28, g30,
                g32, g34, g35, g36, g38, g40, g42, g44, g46, g48,
                g50, g52, g54, g56, g58, g60, g62, g64, g65, g66,
                g68, g70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == g35 || textBoxes[i] == g65)
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

        private void h10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(h10.Text, out double initialPrice) || !double.TryParse(p27.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                h12, h14, h16, h18, h20, h22, h24, h26, h28, h30,
                h32, h34, h35, h36, h38, h40, h42, h44, h46, h48,
                h50, h52, h54, h56, h58, h60, h62, h64, h65, h66,
                h68, h70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == h35 || textBoxes[i] == h65)
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

        private void i10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(i10.Text, out double initialPrice) || !double.TryParse(p28.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                i12, i14, i16, i18, i20, i22, i24, i26, i28, i30,
                i32, i34, i35, i36, i38, i40, i42, i44, i46, i48,
                i50, i52, i54, i56, i58, i60, i62, i64, i65, i66,
                i68, i70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == i35 || textBoxes[i] == i65)
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

        private void j10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(j10.Text, out double initialPrice) || !double.TryParse(p30.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                j12, j14, j16, j18, j20, j22, j24, j26, j28, j30,
                j32, j34, j35, j36, j38, j40, j42, j44, j46, j48,
                j50, j52, j54, j56, j58, j60, j62, j64, j65, j66,
                j68, j70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == j35 || textBoxes[i] == j65)
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

        private void k10_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(k10.Text, out double initialPrice) || !double.TryParse(p32.Text, out double jumpPrice))
                return;

            List<TextBox> textBoxes = new List<TextBox>
            {
                k12, k14, k16, k18, k20, k22, k24, k26, k28, k30,
                k32, k34, k35, k36, k38, k40, k42, k44, k46, k48,
                k50, k52, k54, k56, k58, k60, k62, k64, k65, k66,
                k68, k70
            };

            double lastValue = initialPrice;
            bool halfJumpAdded = false;

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (textBoxes[i] == k35 || textBoxes[i] == k65)
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
                var form = new PriceListO1();
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
