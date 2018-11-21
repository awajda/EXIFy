using System;
using System.Globalization;
using Exify.Contracts;

namespace Exify.Helpers
{
    public class ExifDateTimeParser : IValueParser<string, DateTime?>
    {
        public DateTime? Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParseExact(value, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None,
                out var date))
                return date;

            return null;
        }
    }
}
