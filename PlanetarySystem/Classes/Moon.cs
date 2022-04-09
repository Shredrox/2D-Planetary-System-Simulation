using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlanetarySystem
{
    public class Moon : CelestialObject
    {
        public CelestialObject GravityCenter { get; set; }
        public int Radius { get; set; }
        public double Speed { get; set; }
        private double Angle = 0;

        public Moon(string name, ImageSource image, double startx, double starty, bool orbiting, int width, int height,
            CelestialObject center, int radius, double speed) : base(name, image, startx, starty, orbiting, width, height)
        {
            Speed = speed;
            Radius = radius;
            GravityCenter = center;
        }

        protected override void CalculateChanges()
        {
            double Xcoor = GravityCenter.X;
            double Ycoor = GravityCenter.Y;

            double rad = Angle * Math.PI / 180;
            X = Xcoor + Radius * Math.Cos(rad);
            Y = Ycoor + Radius * Math.Sin(rad);

            Angle += Speed;
        }
    }
}
