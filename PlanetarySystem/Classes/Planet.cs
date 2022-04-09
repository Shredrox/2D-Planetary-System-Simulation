using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PlanetarySystem
{
    public class Planet : CelestialObject
    {
        public string Atmosphere { get; set; }
        public string OrbitalPeriod { get; set; }
        public string RotationPeriod { get; set; }
        public int MoonCount { get; set; }
        public string Life { get; set; }
        public CelestialObject GravityCenter { get; set; }
        public int Radius { get; set; }
        public double Speed { get; set; }
        private double Angle = 0;

        public Planet(string name, string atmosphere, string orbitalPeriod, string rotationPeriod,
            int mooncount, string life, ImageSource image, 
            double startx, double starty,bool orbiting, int width, int height, 
            CelestialObject center, int radius, double speed) : base(name, image, startx, starty, orbiting, width, height)
        {
            Atmosphere = atmosphere;
            OrbitalPeriod = orbitalPeriod;
            RotationPeriod = rotationPeriod;
            MoonCount = mooncount;
            Life = life;
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
