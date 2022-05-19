using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;
using CelestialObjectsLibrary;

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

            BitmapImage planetImage = DataControl.CreateImage(planet.Image.ImageSource.ToString());
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
                newImage = DataControl.CreateImage(fileDialog.FileName);
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
