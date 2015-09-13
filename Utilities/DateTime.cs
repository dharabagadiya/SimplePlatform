
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Utilities
{
    public static class DateTimeUtilities
    {
        private static Calendar cal = CultureInfo.InvariantCulture.Calendar;
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            var day = cal.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) { time = time.AddDays(3); }
            return cal.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var weekNum = weekOfYear;
            if (firstWeek <= 1) { weekNum -= 1; }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static DateTime FirstDateOfWeekISO8601(DateTime dateTime)
        {
            return FirstDateOfWeekISO8601(dateTime.Year, GetIso8601WeekOfYear(dateTime));
        }
    }
}
