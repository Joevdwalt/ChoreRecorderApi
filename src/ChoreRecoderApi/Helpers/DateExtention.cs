using System;

namespace DateExtentionMethods
{
    public static class DateExtention
    {
        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
        public static DateTime EndOfDay(this DateTime date)
        {
            DateTime returnDate = new DateTime(date.Year, date.Month, date.Day);
            return returnDate.AddDays(1).AddMinutes(-1);
        }

        public static DateTime Tomorrow(this DateTime date)
        {
            return date.AddDays(1);
        }
    }
}