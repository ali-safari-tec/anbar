using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using anbar.Data.Models;
using anbar.Tools;
using anbar.User_Controls;
using SQLite.CodeFirst;

namespace anbar.Data.Services
{
    public class DataBaseContext : DbContext
    {
        static DataBaseContext()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory.Trim();
            string dataFolder = Path.Combine(baseDirectory, "DB");

            if (dataFolder.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new ArgumentException("Invalid characters in database path.");
            }

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            AppDomain.CurrentDomain.SetData("DataDirectory", dataFolder);
        }

        public DataBaseContext() : base("name=db") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Person> people { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Cost> costs { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<BuyInvoice> buyInvoices { get; set; }
        public DbSet<PreInvoice> preInvoices { get; set; }
        public DbSet<InvoiceItem> invoiceItems { get; set; }
        public DbSet<BuyInvoiceItem> buyInvoiceItems { get; set; }
        public DbSet<PreInvoiceItem> preInvoiceItems { get; set; }
        public DbSet<FinancialYear> financials { get; set; }
        public DbSet<Detail> details { get; set; }
        public DbSet<Detail2> details2 { get; set; }
        public DbSet<ListM> listMs { get; set; }
        public DbSet<ListO> listOs { get; set; }
        public DbSet<ListM1> listM1s { get; set; }
        public DbSet<ListO1> listO1s { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var initializer = new SqliteCreateDatabaseIfNotExists<DataBaseContext>(modelBuilder);
            Database.SetInitializer(initializer);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasIndex(p => p.PersonIdByUser).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.ProductIdByUser).IsUnique();
        }

        public void SeedDatabase()
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime now = DateTime.Now;

            if (!financials.Any())
            {
                financials.Add(new FinancialYear
                {
                    Id = 1,
                    ShowYear = persianCalendar.GetYear(now),
                    LastYear = persianCalendar.GetYear(now)
                });
                SaveChanges();
            }
            if (!Users.Any())
            {
                Users.Add(new User
                {
                    Name = "1",
                    Password = HashPassword.PasswordHash("1"),
                    Day = persianCalendar.GetDayOfMonth(now),
                    Month = persianCalendar.GetMonth(now),
                    Year = persianCalendar.GetYear(now)
                });
                SaveChanges();
            }
        }
    }
}
