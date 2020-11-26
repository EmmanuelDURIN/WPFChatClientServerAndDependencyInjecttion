using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFChatClient
{
    public class MoneyToColorConverter : IValueConverter
    {
        // Appelée à chaque fois que le binding lit la valeur
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dValue = (double)value;
            if (dValue < 0)
                return Brushes.Red;
            else
                return Brushes.Blue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
