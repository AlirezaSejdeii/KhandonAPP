using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Utilities
{
    public static class MyDateTime
    {
        public static DateTime Now()
        {
            return DateTime.UtcNow;
        }
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            return $"{persianCalendar.GetYear(value)}/{persianCalendar.GetMonth(value)}/{persianCalendar.GetDayOfMonth(value)}";
        }
    }
}
