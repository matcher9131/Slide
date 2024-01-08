using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Slide.Static
{
    public static class FavoriteLevelColors
    {
        private const double DarkRatio = 0.25;

        private readonly static Color[] colors = new Color[]
        {
            Colors.White,
            Colors.Yellow,
            Colors.DeepPink
        };

        public static Color GetColor(int favoriteLevel, bool isActive = true)
        {
            var color = favoriteLevel >= 0 && favoriteLevel < colors.Length ? colors[favoriteLevel] : Colors.White;
            return isActive ? color : Color.FromRgb((byte)(color.R * DarkRatio), (byte)(color.G * DarkRatio), (byte)(color.B * DarkRatio));
        }
    }
}
