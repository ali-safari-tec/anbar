namespace anbar.Tools
{
    public static class MoneyConverter
    {
        public static string Money(int number)
        {
            return number.ToString("N0", new System.Globalization.CultureInfo("fa-IR")) + " ریال";
        }

        public static string Money(decimal number)
        {
            return number.ToString("N0", new System.Globalization.CultureInfo("fa-IR")) + " ریال";
        }

        public static string Money1(int number)
        {
            return number.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
        }

        public static string Money1(decimal number)
        {
            return number.ToString("N0", new System.Globalization.CultureInfo("fa-IR"));
        }
    }
}
