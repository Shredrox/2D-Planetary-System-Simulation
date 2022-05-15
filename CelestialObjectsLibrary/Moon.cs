using System;
using System.Windows.Media.Imaging;

namespace CelestialObjectsLibrary
{
    [Serializable]
    public class Moon : CelestialObject
    {
        public CelestialObject GravityCenter { get; set; }
        public int Radius { get; set; }
        public double Speed { get; set; }
        private double Angle = 0;

        public Moon(string name, BitmapImage image, double startx, double starty, bool orbiting, int width, int height,
            CelestialObject center, int radius, double speed) : base(name, image, startx, starty, orbiting, width, height)
        {
            Speed = speed;
            Radius = radius;
            GravityCenter = center;
        }

        protected Moon()
        {

        }

        protected override void CalculateChanges()
        {
            double rad = Angle * Math.PI / 180;
            X = GravityCenter.X + Radius * Math.Cos(rad);
            Y = GravityCenter.Y + Radius * Math.Sin(rad);

            Angle += Speed;
        }
    }
}
