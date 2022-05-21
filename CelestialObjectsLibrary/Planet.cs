using System;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace CelestialObjectsLibrary
{
    [Serializable]
    [XmlInclude(typeof(Star))]
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
            int mooncount, string life, BitmapImage image, 
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

        protected Planet()
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
