using Slide.Static;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Slide.Converter
{
    public class FavoriteLevelBrushConverter : IValueConverter
    {
        // Level -> Brush
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int level = (int?)value ?? 0;
            return new SolidColorBrush(FavoriteLevelColors.GetColor(level));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
