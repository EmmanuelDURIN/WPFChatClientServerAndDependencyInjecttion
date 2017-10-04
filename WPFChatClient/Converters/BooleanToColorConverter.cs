using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFChatClient.Converters
{
  public class BooleanToColorConverter : IValueConverter
  {
    public Brush TrueColor { get; set; } = new SolidColorBrush(Color.FromArgb(255, 128, 128, 0));
    public Brush FalseColor { get; set; } = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        bool bValue = (bool)value;
        return bValue ? TrueColor : FalseColor;
      }
      catch (Exception)
      {
        System.Diagnostics.Debug.WriteLine("Error in conversion");
      }
      return TrueColor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
