using System;

namespace LSHS.Helpers
{
    public static class StringExtensions
    {
        private const string STR_TwitterFormat = "ddd, dd MMM yyyy H:mm:ss %zzzz";// Thu, 14 Apr 2011 02:22:52 +0000
        private const string STR_TwitterFormatAbbreviated = "ddd, dd MMM yyyy";// Thu, 14 Apr 2011 0:00:00

        public static DateTime ParseDateTime(this string date)
        {
            DateTime output = DateTime.MinValue;
            try
            {
                output = DateTime.ParseExact(date, STR_TwitterFormat, null);
            }
            catch (FormatException ex)
            {
                var tempDate = date.Replace(" 0:00:00 ", "");
                try
                {
                    output = DateTime.ParseExact(tempDate, STR_TwitterFormatAbbreviated, null);
                }
                catch
                {
                    throw;
                }
            }
            return output;
        }
    }
}