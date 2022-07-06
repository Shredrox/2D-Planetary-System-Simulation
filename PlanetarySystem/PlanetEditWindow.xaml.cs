using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;
using CelestialObjectsLibrary;
using System;

namespace PlanetarySystem
{
    public partial class PlanetEditWindow : Window
    {
        private CelestialObject _editedPlanet;
        private BitmapImage _newImage;

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

            _editedPlanet = planet;
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
                _newImage = DataControl.CreateImage(fileDialog.FileName);
                PlanetImage.Source = _newImage;
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
            
            _editedPlanet.Name = PlanetName.Text;
            ((Planet)_editedPlanet).Atmosphere = PlanetAtmo.Text;
            ((Planet)_editedPlanet).OrbitalPeriod = PlanetOrbitalPeriod.Text;
            ((Planet)_editedPlanet).RotationPeriod = PlanetRotationPeriod.Text;
            ((Planet)_editedPlanet).MoonCount = int.Parse(PlanetMoonCount.Text);
            ((Planet)_editedPlanet).Life = PlanetLife.Text;
            _editedPlanet.Width = int.Parse(PlanetWidth.Text);
            _editedPlanet.Height = int.Parse(PlanetHeight.Text);
            ((Planet)_editedPlanet).Speed = double.Parse(PlanetSpeed.Text);
            
            if(_newImage != null)
            {
                _editedPlanet.Image.ImageSource = _newImage;
                _editedPlanet.ImageUri = _newImage.UriSource.ToString();
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
