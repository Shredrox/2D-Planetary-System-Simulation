using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace CelestialObjectsLibrary
{
    [Serializable]
    [XmlInclude(typeof(Planet))]
    [XmlInclude(typeof(Moon))]
    [XmlInclude(typeof(Star))]
    public class CelestialObject 
    {
        public string Name { get; set; }
        [XmlIgnore]
        public Ellipse Shape { get; set; }
        [XmlIgnore]
        public ImageBrush Image { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsOrbiting { get; set; }
        public string ImageUri { get; set; }

        public CelestialObject(string name, BitmapImage image, double startx, double starty, bool orbiting, int width, int height)
        {
            Name = name;
            Image = new ImageBrush();
            Image.ImageSource = image;
            Shape = new Ellipse();
            Shape.Fill = Image;
            X = startx;
            Y = starty;
            IsOrbiting = orbiting;
            Width = width;
            Height = height;
            ImageUri = Image.ImageSource.ToString();

            DrawOjbect();
        }
        
        protected CelestialObject()
        {
            Shape = new Ellipse();
            Image = new ImageBrush();
        }

        protected void DrawOjbect()
        {
            Shape.SetValue(Canvas.LeftProperty, X);
            Shape.SetValue(Canvas.TopProperty, Y);
            Shape.Width = Width;
            Shape.Height = Height;

            Shape.Margin = new Thickness(-Shape.Height/2);
        }
        
        protected virtual void CalculateChanges()
        {
            
        }

        public void Update()
        {
            if (IsOrbiting)
            {
                CalculateChanges();
                DrawOjbect();
            }
        }
    }
}
