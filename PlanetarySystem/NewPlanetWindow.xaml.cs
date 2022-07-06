using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CelestialObjectsLibrary;

namespace PlanetarySystem
{
    public partial class NewPlanetWindow : Window
    {
        private readonly BitmapImage _defaultImage = DataControl.CreateImage("defaultPlanet.png");
        private BitmapImage _newImage;

        private int _planetPosition;
        private SolarSystem _editedSystem;
        private Star _sun = new Star("Sun", DataControl.CreateImage("sun.png"),
                                    ((MainWindow)Application.Current.MainWindow).MainCanvas.ActualWidth / 2,
                                    ((MainWindow)Application.Current.MainWindow).MainCanvas.ActualHeight / 2, 80, 80);
        Planet newPlanet;

        public NewPlanetWindow(SolarSystem solarSystem, int planetIndex)
        {
            InitializeComponent();

            PlanetImage.Source = _defaultImage;

            _editedSystem = solarSystem;
            _planetPosition = planetIndex; 
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlanetName.Text == String.Empty || PlanetAtmo.Text == String.Empty
                || PlanetOrbitalPeriod.Text == String.Empty || PlanetRotationPeriod.Text == String.Empty
                || PlanetMoonCount.Text == String.Empty || PlanetLife.Text == String.Empty || PlanetSpeed.Text == String.Empty 
                || PlanetWidth.Text == String.Empty || PlanetHeight.Text == String.Empty)
            {
                MessageBox.Show("Missing info for new planet");
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
            
            int radius = 0;
            switch (_planetPosition + 1)
            {
                case 1: radius = 50; break;
                case 2: radius = 100; break;
                case 3: radius = 150; break;
                case 4: radius = 200; break;
                case 5: radius = 280; break;
                case 6: radius = 330; break;
                case 7: radius = 370; break;
                case 8: radius = 400; break;
            }

            newPlanet = new Planet(PlanetName.Text, PlanetAtmo.Text, PlanetOrbitalPeriod.Text, PlanetRotationPeriod.Text,
                int.Parse(PlanetMoonCount.Text), PlanetLife.Text, _defaultImage, 10, 10, true, 
                int.Parse(PlanetWidth.Text), int.Parse(PlanetHeight.Text), _sun, radius, double.Parse(PlanetSpeed.Text));

            if (_newImage != null)
            {
                newPlanet.Image.ImageSource = _newImage;
            }

            _editedSystem.SystemPlanets.Insert(_editedSystem.SystemPlanets.Count - 1, newPlanet);

            if (int.Parse(PlanetMoonCount.Text) <= 3 && int.Parse(PlanetMoonCount.Text) > 0)
            {
                for (int m = 1; m < int.Parse(PlanetMoonCount.Text) + 1; m++)
                {
                    _editedSystem.SystemPlanets.Insert(_editedSystem.SystemPlanets.Count - 1, new Moon($"Moon {m}", 
                        DataControl.CreateImage("moon.png"),
                        10, 10, true, 5, 5, newPlanet, newPlanet.Width / 2 + 10 + m * 4, m));
                }
            }
            else if (int.Parse(PlanetMoonCount.Text) > 3)
            {
                for (int m = 1; m < 4; m++)
                {
                    _editedSystem.SystemPlanets.Insert(_editedSystem.SystemPlanets.Count - 1, new Moon($"Moon {m}",
                        DataControl.CreateImage("moon.png"),
                        10, 10, true, 5, 5, newPlanet, newPlanet.Width / 2 + 10 + m * 4, m));
                }
            }

            DialogResult = true;
        }

        private void ImageAddButton_Click(object sender, RoutedEventArgs e)
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

        public ImageSource NewImage()
        {
            if(_newImage == null)
            {
                _newImage = _defaultImage;
                return _newImage;
            }
            return _newImage;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
