using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using anbar.Data.Dto;
using anbar.Data.Models;
using System.Windows;
using anbar.Tools;

namespace anbar.Data.Services
{
    public class GenericDataBaseServices<T> : IDataBaseServices<T>, IDisposable where T : class
    {
        private readonly DataBaseContext _db;
        private readonly DbSet<T> _dbSet;
        private bool _disposed = false;
        PersianCalendar persianCalendar = new PersianCalendar();
        DateTime now = DateTime.Now;

        public class NaturalSortComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == y) return 0;
                if (string.IsNullOrEmpty(x)) return -1;
                if (string.IsNullOrEmpty(y)) return 1;

                int ix = 0, iy = 0;

                while (ix < x.Length && iy < y.Length)
                {
                    if (char.IsDigit(x[ix]) && char.IsDigit(y[iy]))
                    {
                        string numX = string.Empty;
                        string numY = string.Empty;

                        while (ix < x.Length && char.IsDigit(x[ix]))
                            numX += x[ix++];

                        while (iy < y.Length && char.IsDigit(y[iy]))
                            numY += y[iy++];

                        int valX = int.Parse(numX);
                        int valY = int.Parse(numY);

                        int cmp = valX.CompareTo(valY);
                        if (cmp != 0)
                            return cmp;
                    }
                    else
                    {
                        int cmp = x[ix].CompareTo(y[iy]);
                        if (cmp != 0)
                            return cmp;

                        ix++;
                        iy++;
                    }
                }

                return x.Length.CompareTo(y.Length);
            }
        }

        public GenericDataBaseServices(DataBaseContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public async Task<T> Create(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _db.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }
                else
                {
                    _dbSet.Remove(entity);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {

                    return false;
                }

                if (entity is Invoice invoice)
                {
                    var invoiceItems = await _db.Set<InvoiceItem>()
                                               .Where(ii => ii.InvoiceId == id)
                                               .ToListAsync(); // فوراً داده‌ها را از دیتابیس می‌آورد

                    foreach (var item in invoiceItems)
                    {
                        var product = await _db.products.FirstOrDefaultAsync(p => p.ProductIdByUser == item.ProductCode);
                        if (product != null)
                        {
                            product.ProductNumber += item.InvoiceItemNumber; // افزایش تعداد محصول
                            _db.Entry(product).State = EntityState.Modified; // علامت‌گذاری به‌عنوان تغییر یافته
                        }
                    }
                    await _db.SaveChangesAsync();

                    _db.Set<InvoiceItem>().RemoveRange(invoiceItems);
                    await _db.SaveChangesAsync(); // ابتدا آیتم‌های فاکتور حذف شوند.
                }

                _dbSet.Remove(entity);
                await _db.SaveChangesAsync(); // سپس خود فاکتور حذف شود.
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> DeleteBuyInvoice(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {

                    return false;
                }

                if (entity is BuyInvoice invoice)
                {
                    var invoiceItems = await _db.Set<BuyInvoiceItem>()
                                               .Where(ii => ii.BuyInvoiceId == id)
                                               .ToListAsync(); // فوراً داده‌ها را از دیتابیس می‌آورد

                    foreach (var item in invoiceItems)
                    {
                        var product = await _db.products.FirstOrDefaultAsync(p => p.ProductIdByUser == item.ProductCode);
                        if (product != null)
                        {
                            product.ProductNumber += item.InvoiceItemNumber; // افزایش تعداد محصول
                            _db.Entry(product).State = EntityState.Modified; // علامت‌گذاری به‌عنوان تغییر یافته
                        }
                    }
                    await _db.SaveChangesAsync();

                    _db.Set<BuyInvoiceItem>().RemoveRange(invoiceItems);
                    await _db.SaveChangesAsync(); // ابتدا آیتم‌های فاکتور حذف شوند.
                }

                _dbSet.Remove(entity);
                await _db.SaveChangesAsync(); // سپس خود فاکتور حذف شود.
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> DeletePreInvoice(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {

                    return false;
                }

                if (entity is PreInvoice preInvoice)
                {
                    var invoiceItems = await _db.Set<PreInvoiceItem>()
                                               .Where(ii => ii.PreInvoiceId == id)
                                               .ToListAsync(); // فوراً داده‌ها را از دیتابیس می‌آورد

                    foreach (var item in invoiceItems)
                    {
                        var product = await _db.products.FirstOrDefaultAsync(p => p.ProductIdByUser == item.ProductCode);
                        if (product != null)
                        {
                            product.ProductNumber += item.InvoiceItemNumber; // افزایش تعداد محصول
                            _db.Entry(product).State = EntityState.Modified; // علامت‌گذاری به‌عنوان تغییر یافته
                        }
                    }
                    await _db.SaveChangesAsync();

                    _db.Set<PreInvoiceItem>().RemoveRange(invoiceItems);
                    await _db.SaveChangesAsync(); // ابتدا آیتم‌های فاکتور حذف شوند.
                }

                _dbSet.Remove(entity);
                await _db.SaveChangesAsync(); // سپس خود فاکتور حذف شود.
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<T> GetId(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return null;
                }
                else
                {
                    await _db.SaveChangesAsync();
                    return entity;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                var entity = await _dbSet.ToListAsync();
                await _db.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<T> Update(int id, T entity)
        {
            try
            {
                var updateEntity = await _dbSet.FindAsync(id);
                if (updateEntity == null)
                {
                    return null;
                }
                else
                {
                    _db.Entry(updateEntity).CurrentValues.SetValues(entity);
                    await _db.SaveChangesAsync();
                    return updateEntity;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<PersonDto>> GetSelectedPeople()
        {
            try
            {
                var query = await _db.people.Select(p => new PersonDto
                {
                    PersonIdByUser = p.PersonIdByUser,
                    PersonName = p.PersonName,
                    PersonPhone = p.PersonPhone,
                    PersonPhoneCode = p.PersonPhoneCode,
                    PersonAddress = p.PersonAddress,
                    PersonMobile = p.PersonMobile
                }).ToListAsync();

                var sender = query
                    .OrderBy(c => c.PersonIdByUser, new NaturalSortComparer())
                    .ToList();

                return sender;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<Person>> SearchPeople(string searchText)
        {
            try
            {
                if (string.IsNullOrEmpty(searchText) || searchText == "جستجو...")
                {
                    // در صورتی که متنی وارد نشده، تمام رکوردها را برمی‌گرداند

                    var query = await _db.people.ToListAsync();

                    var sender = query
                        .OrderBy(c => c.PersonIdByUser, new NaturalSortComparer())
                        .ToList();

                    return sender;
                }
                else
                {

                    // در صورتی که متنی وارد شده، جستجو انجام می‌شود
                    var query = await _db.people
                                     .Where(c => c.PersonName.Contains(searchText) || c.PersonIdByUser.Contains(searchText))
                                     .ToListAsync();

                    var sender = query
                        .OrderBy(c => c.PersonIdByUser, new NaturalSortComparer())
                        .ToList();

                    return sender;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetSelectedProducts()
        {
            try
            {
                var query = await _db.products.Select(p => new ProductDto
                {
                    ProductIdByUser = p.ProductIdByUser,
                    ProductName = p.ProductName,
                    ProductNumber = p.ProductNumber,
                    ProductPrice = p.ProductPrice
                }).ToListAsync();

                var sender = query
                    .OrderBy(c => c.ProductIdByUser, new NaturalSortComparer())
                    .ToList();

                return sender;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<Product>> SearchProducts(string searchText)
        {
            try
            {
                if (string.IsNullOrEmpty(searchText) || searchText == "جستجو...")
                {

                    // در صورتی که متنی وارد نشده، تمام رکوردها را برمی‌گرداند
                    var query = await _db.products.ToListAsync();

                    var sender = query
                    .OrderBy(c => c.ProductIdByUser, new NaturalSortComparer())
                    .ToList();

                    return sender;
                }
                else
                {

                    // در صورتی که متنی وارد شده، جستجو انجام می‌شود
                    var query = await _db.products
                                     .Where(c => c.ProductName.ToLower().Contains(searchText) || c.ProductIdByUser.ToLower().Contains(searchText))
                                     .ToListAsync();

                    var sender = query
                    .OrderBy(c => c.ProductIdByUser, new NaturalSortComparer())
                    .ToList();

                    return sender;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<InvoiceDto>> GetSelectedInvoicee(int type, int year)
        {
            try
            {
                var query = await _db.invoices.Where(c => c.Type == type && c.DateYear == year).Select(p => new InvoiceDto
                {
                    InvoiceId = p.InvoiceId,
                    ChangeId = p.ChangeId,
                    InvoiceIdS = p.InvoiceIdS,
                    InvoiceBuyer = p.InvoiceBuyer,
                    InvoiceDiscount = p.InvoiceDiscount,
                    InvoiceTotalCost = p.InvoiceTotalCost,
                    InvoicePay = p.InvoicePay,
                    DateDay = p.DateDay,
                    DateMonth = p.DateMonth,
                    DateYear = p.DateYear
                }).ToListAsync();
                return query;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<CostDto>> GetSelectedCost(int type, int year)
        {
            try
            {
                var query = await _db.costs.Where(c => c.Type == type && c.year == year).Select(p => new CostDto
                {
                    Id = p.Id,
                    Seller = p.Seller,
                    Caption = p.Caption,
                    Fee = p.Fee,
                    Discount = p.Discount,
                    Pay = p.Pay,
                    day = p.day,
                    month = p.month,
                    year = p.year
                }).ToListAsync();
                return query;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<BuyInvoiceDto>> GetSelectedBuyInvoicee(int type, int year)
        {
            try
            {
                var query = await _db.buyInvoices.Where(c => c.Type == type && c.DateYear == year).Select(p => new BuyInvoiceDto
                {
                    InvoiceId = p.BuyInvoiceId,
                    InvoiceIdS = p.BuyInvoiceIdS,
                    InvoiceSeller = p.BuyInvoiceSeller,
                    InvoiceDiscount = p.BuyInvoiceDiscount,
                    InvoiceTotalCost = p.BuyInvoiceTotalCost,
                    InvoicePay = p.BuyInvoicePay,
                    DateDay = p.DateDay,
                    DateMonth = p.DateMonth,
                    DateYear = p.DateYear
                }).ToListAsync();
                return query;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<IEnumerable<PreInvoiceDto>> GetSelectedPreInvoicee(int year)
        {
            try
            {
                var query = await _db.preInvoices.Where(c => c.PreDateYear == year).Select(p => new PreInvoiceDto
                {
                    PreInvoiceId = p.PreInvoiceId,
                    ChangeId = p.ChangeId,
                    PreInvoiceIdS = p.PreInvoiceIdS,
                    PreInvoiceBuyer = p.PreInvoiceBuyer,
                    InvoiceDiscount = p.InvoiceDiscount,
                    InvoiceTotalCost = p.InvoiceTotalCost,
                    PreDateDay = p.PreDateDay,
                    PreDateMonth = p.PreDateMonth,
                    PreDateYear = p.PreDateYear,
                    PreDateDayS = p.PreDateDayS,
                    PreDateMonthS = p.PreDateMonthS,
                    PreDateYearS = p.PreDateYearS
                }).ToListAsync();
                return query;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<bool> ConvertProformaInvoiceAsync(int proformaInvoiceId)
        {
            // یافتن پیش فاکتور بر اساس شناسه
            var proforma = await _db.preInvoices
                      .Include(p => p.preInvoiceItems) // شامل کردن آیتم‌ها
                      .FirstOrDefaultAsync(p => p.PreInvoiceId == proformaInvoiceId);

            if (proforma == null)
            {
                // پیش فاکتور یافت نشد
                return false;
            }
            int year1 = _db.financials.Select(c => c.ShowYear).FirstOrDefault();
            // ایجاد یک فاکتور جدید با انتقال اطلاعات از پیش فاکتور
            var invoice = new Invoice
            {
                ChangeId = _db.invoices.Any(c => c.DateYear == year1) ? _db.invoices.Max(c => c.ChangeId) + 1 : 1,
                InvoiceIdS = (_db.invoices.Any() ? _db.invoices.Max(c => c.ChangeId) + 1 : 1).ToString("D5"),
                InvoiceBuyer = proforma.PreInvoiceBuyer,
                InvoiceSeller = proforma.PreInvoiceSeller,
                InvoicePhone = proforma.PreInvoicePhone,
                InvoicePhoneCode = proforma.PreInvoicePhoneCode,
                InvoiceAddress = proforma.PreInvoiceAddress,
                DateDay = persianCalendar.GetDayOfMonth(now),
                DateMonth = persianCalendar.GetMonth(now),
                DateYear = persianCalendar.GetYear(now),
                InvoiceTotalCost = proforma.InvoiceTotalCost,
                InvoiceDiscount = proforma.InvoiceDiscount,
                Detail = proforma.Detail,
                Detail2 = proforma.Detail2,
                Type = 1
            };

            // افزودن فاکتور جدید به دیتابیس
            _db.invoices.Add(invoice);
            await _db.SaveChangesAsync();

            var invoiceItems = proforma.preInvoiceItems.Select(item => new InvoiceItem
            {
                InvoiceId = invoice.InvoiceId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                InvoiceItemPrice = item.InvoiceItemPrice,
                InvoiceFinalPrice = item.InvoiceFinalPrice,
                InvoiceItemPriceS = item.InvoiceItemPriceS,
                InvoiceFinalPriceS = item.InvoiceFinalPriceS,
                InvoiceItemNumber = item.InvoiceItemNumber
            }).ToList();

            _db.invoiceItems.AddRange(invoiceItems);

            // حذف پیش فاکتور و آیتم‌های آن
            _db.preInvoiceItems.RemoveRange(proforma.preInvoiceItems);
            _db.preInvoices.Remove(proforma);

            // ذخیره تغییرات نهایی
            await _db.SaveChangesAsync();
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // آزادسازی منابع مدیریت‌شده
                _db?.Dispose();
            }

            // آزادسازی منابع غیرمدیریت‌شده در صورت لزوم

            _disposed = true;
        }

        ~GenericDataBaseServices()
        {
            Dispose(false);
        }
    }
}
