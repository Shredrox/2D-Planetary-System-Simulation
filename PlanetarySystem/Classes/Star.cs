using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlanetarySystem
{
    public class Star : CelestialObject
    {
        public Star(string name, ImageSource image, double startx, double starty, int width, int height)
            : base(name, image, startx, starty, false, width, height)
        {
    
        }
    }
}
