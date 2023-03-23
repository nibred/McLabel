using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace McLabel.Utils.Converters
{
    internal class DateTimeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Double.TryParse(values[0].ToString(), out var d))
                d = 0;
            if (!Double.TryParse(values[1].ToString(), out var h))
                h = 0;
            if (!Double.TryParse(values[2].ToString(), out var m))
                m = 0;
            try
            {
                DateTime newDateTime = DateTime.Now.AddDays(d).AddHours(h).AddMinutes(m);
                return newDateTime;
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
