using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PlanetarySystem
{
    [Serializable]
    public class Star : CelestialObject
    {
        public Star(string name, BitmapImage image, double startx, double starty, int width, int height)
            : base(name, image, startx, starty, false, width, height)
        {
    
        }

        protected Star()
        {

        }
    }
}
