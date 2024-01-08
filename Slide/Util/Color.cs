using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slide.Util
{
    public static class Colors
    {
        public static Color GetDarkerColor(Color color) => Color.FromArgb(color.A, (byte)(color.R * 3 / 4), (byte)(color.G * 3 / 4), (byte)(color.B * 3 / 4));
    }
}
