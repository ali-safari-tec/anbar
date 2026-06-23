using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Threading;

namespace anbar.Tools
{
    public class ClockViewModel : INotifyPropertyChanged
    {
        private string _persianDate;
        private readonly PersianCalendar persianCalendar = new PersianCalendar();
        private readonly DispatcherTimer timer;

        public string PersianDate
        {
            get { return _persianDate; }
            set
            {
                _persianDate = value;
                OnPropertyChanged(nameof(PersianDate));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public ClockViewModel()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (s, e) => UpdateTime();
            timer.Start();

            UpdateTime();
        }

        private void UpdateTime()
        {
            DateTime now = DateTime.Now;
            string[] persianDay = { "یکشنبه", "دوشنبه", "‌سه‌شنبه", "چهارشنبه", "پنج‌شنبه‌", "جمعه", "شنبه" };
            string dayOfWeek = persianDay[(int)now.DayOfWeek];

            PersianDate = $"{persianCalendar.GetYear(now)}/{persianCalendar.GetMonth(now):00}/{persianCalendar.GetDayOfMonth(now):00}" + "    " + $"{persianCalendar.GetHour(now):00}:{persianCalendar.GetMinute(now):00}" + "    " + $"{dayOfWeek}";
        }
    }
}
