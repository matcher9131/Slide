using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Slide.Converter
{
    public class FavoriteLevelConverter : IValueConverter
    {
        // Level -> Color
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? level = (int?)value;
            return level switch
            {
                1 => "Yellow",
                2 => "HotPink",
                _ => "White"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
