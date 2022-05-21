using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;
using CelestialObjectsLibrary;
using System;

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

            PlanetImage.Source = DataControl.CreateImage(planet.Image.ImageSource.ToString());

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
                newImage = DataControl.CreateImage(fileDialog.FileName);
                PlanetImage.Source = newImage;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlanetName.Text == String.Empty || PlanetAtmo.Text == String.Empty
                || PlanetOrbitalPeriod.Text == String.Empty || PlanetRotationPeriod.Text == String.Empty
                || PlanetMoonCount.Text == String.Empty || PlanetLife.Text == String.Empty || PlanetSpeed.Text == String.Empty
                || PlanetWidth.Text == String.Empty || PlanetHeight.Text == String.Empty)
            {
                MessageBox.Show("Missing info for planet");
                return;
            }
            else if (!int.TryParse(PlanetMoonCount.Text, out _) || !int.TryParse(PlanetWidth.Text, out _)
                || !int.TryParse(PlanetHeight.Text, out _) || !double.TryParse(PlanetSpeed.Text, out _)
                || int.Parse(PlanetMoonCount.Text) < 0 || double.Parse(PlanetSpeed.Text) < -10)
            {
                MessageBox.Show("Input was not in the correct format.");
                return;
            }
            else if (int.Parse(PlanetWidth.Text) > 75 || int.Parse(PlanetHeight.Text) > 75
                || int.Parse(PlanetMoonCount.Text) > 500000 || double.Parse(PlanetSpeed.Text) > 10)
            {
                MessageBox.Show("Value too big. Please enter a smaller value.");
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

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
