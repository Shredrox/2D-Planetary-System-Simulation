using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CelestialObjectsLibrary
{
    public class Comet : CelestialObject
    {
        public Canvas Canvas { get; set; }

        public Comet(string name, Canvas scene, BitmapImage image, double startx, double starty, bool orbiting, int width, int height)
            : base(name, image, startx, starty, orbiting, width, height)
        {
            Canvas = scene;
        }

        protected override void CalculateChanges()
        {
            X += 10;
            Y += 10;

            if (X > Canvas.ActualWidth)
            {
                X = 0;
            }

            if(Y > Canvas.ActualHeight)
            {
                Y = 0;
            }
        }
    }
}
