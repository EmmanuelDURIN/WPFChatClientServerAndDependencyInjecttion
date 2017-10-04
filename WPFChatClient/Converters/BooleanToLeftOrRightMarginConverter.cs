using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFChatClient.Converters
{
  public class BooleanToLeftOrRightMarginConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        bool bValue = (bool)value;
        if (bValue) return new Thickness(30, 5, 5, 0);
      }
      catch (Exception)
      {
        System.Diagnostics.Debug.WriteLine("Error in conversion");
      }
      return  new Thickness(5, 5, 30, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
