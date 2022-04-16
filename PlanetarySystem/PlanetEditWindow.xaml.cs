using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlanetarySystem
{
    public partial class PlanetEditWindow : Window
    {
        private CelestialObject editedPlanet;
        private BitmapImage newImage;

        public PlanetEditWindow(CelestialObject planet)
        {
            InitializeComponent();

            PlanetName.Text = ((Planet)planet).Name;
            PlanetAtmo.Text = ((Planet)planet).Atmosphere;
            PlanetOrbitalPeriod.Text = ((Planet)planet).OrbitalPeriod;
            PlanetRotationPeriod.Text = ((Planet)planet).RotationPeriod;
            PlanetMoonCount.Text = ((Planet)planet).MoonCount.ToString();
            PlanetLife.Text = ((Planet)planet).Life;
            PlanetWidth.Text = planet.Width.ToString();
            PlanetHeight.Text = planet.Height.ToString();
            PlanetSpeed.Text = ((Planet)planet).Speed.ToString();

            string fullPath;
            if (planet.Image.ImageSource.ToString().Contains("file://"))
            {
                fullPath = planet.Image.ImageSource.ToString();
            }
            else if (planet.Image.ImageSource.ToString().Contains("/Images"))
            {
                fullPath = planet.Image.ImageSource.ToString();
            }
            else
            {
                fullPath = System.IO.Path.GetFullPath(planet.Image.ImageSource.ToString());
            }

            BitmapImage planetImage = ((MainWindow)Application.Current.MainWindow).CreateImage(fullPath);
            PlanetImage.Source = planetImage;

            editedPlanet = planet;
        }

        private void ImageEditButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "PNG Files |*.png";
            fileDialog.Title = "Select Picture";
            fileDialog.Multiselect = false;
            fileDialog.CheckFileExists = true;

            if (fileDialog.ShowDialog() == true)
            {
                newImage = ((MainWindow)Application.Current.MainWindow).CreateImage(fileDialog.FileName);
                PlanetImage.Source = newImage;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(PlanetMoonCount.Text, out _) || !int.TryParse(PlanetWidth.Text, out _) 
                || !int.TryParse(PlanetHeight.Text, out _) || !double.TryParse(PlanetSpeed.Text, out _))
            {
                MessageBox.Show("Input was not in the correct format.");
                return;
            }

            editedPlanet.Name = PlanetName.Text;
            ((Planet)editedPlanet).Atmosphere = PlanetAtmo.Text;
            ((Planet)editedPlanet).OrbitalPeriod = PlanetOrbitalPeriod.Text;
            ((Planet)editedPlanet).RotationPeriod = PlanetRotationPeriod.Text;
            ((Planet)editedPlanet).MoonCount = int.Parse(PlanetMoonCount.Text);
            ((Planet)editedPlanet).Life = PlanetLife.Text;
            editedPlanet.Width = int.Parse(PlanetWidth.Text);
            editedPlanet.Height = int.Parse(PlanetHeight.Text);
            ((Planet)editedPlanet).Speed = double.Parse(PlanetSpeed.Text);
            
            if(newImage != null)
            {
                editedPlanet.Image.ImageSource = newImage;
                editedPlanet.ImageUri = newImage.UriSource.ToString();
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
