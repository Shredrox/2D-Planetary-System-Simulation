using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CelestialObjectsLibrary;

namespace PlanetarySystem
{
    public partial class SystemCreationWindow : Window
    {
        public List<CelestialObject> newSystemObjects = new List<CelestialObject>();
        private Star _sun = new Star("Sun", DataControl.CreateImage("sun.png"),
                                    ((MainWindow)Application.Current.MainWindow).MainCanvas.ActualWidth / 2,
                                    ((MainWindow)Application.Current.MainWindow).MainCanvas.ActualHeight / 2, 80, 80);

        private readonly BitmapImage _defaultImage = DataControl.CreateImage("defaultPlanet.png");
        private BitmapImage _newImage;

        private int _counter = 0;
        
        public SystemCreationWindow()
        {
            InitializeComponent();
        }

        private void CreatePlanetButton_Click(object sender, RoutedEventArgs e)
        {
            if (newSystemObjects.OfType<Planet>().Count() == 8)
            {
                MessageBox.Show("Maximum number of planets reached.");
                return;
            }
            else if (Name.Text == String.Empty || Atmosphere.Text == String.Empty
                || OrbitalPeriod.Text == String.Empty || RotationPeriod.Text == String.Empty
                || MoonCount.Text == String.Empty || Life.Text == String.Empty || Speed.Text == String.Empty ||
                Width.Text == String.Empty || Height.Text == String.Empty)
            {
                MessageBox.Show("Missing info for new planet.");
                return;
            }
            else if (!int.TryParse(MoonCount.Text, out _) || !int.TryParse(Width.Text, out _)
                    || !int.TryParse(Height.Text, out _) || !double.TryParse(Speed.Text, out _)
                    || int.Parse(MoonCount.Text) < 0 || double.Parse(Speed.Text) < -10)
            {
                MessageBox.Show("Input was not in the correct format.");
                return;
            }
            else if (int.Parse(Width.Text) > 75 || int.Parse(Height.Text) > 75
                || int.Parse(MoonCount.Text) > 500000 || double.Parse(Speed.Text) > 10)
            {
                MessageBox.Show("Value too big. Please enter a smaller value.");
                return;
            } 
            
            _counter++;

            int radius = 0;
            switch (_counter)
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

            Planet newPlanet = new Planet(Name.Text, Atmosphere.Text, OrbitalPeriod.Text, RotationPeriod.Text, int.Parse(MoonCount.Text), Life.Text,
                _defaultImage, 10, 10, true, int.Parse(Width.Text), int.Parse(Height.Text), _sun, radius, double.Parse(Speed.Text));

            if (ImageOption.IsChecked == true)
            {
                newPlanet.Image.ImageSource = _newImage;
            }

            newSystemObjects.Add(newPlanet);

            if (int.Parse(MoonCount.Text) <= 3 && int.Parse(MoonCount.Text) > 0)
            {
                for (int m = 1; m < int.Parse(MoonCount.Text) + 1; m++)
                {
                    newSystemObjects.Add(new Moon($"Moon {m}",
                        DataControl.CreateImage("moon.png"),
                        10, 10, true, 5, 5, newPlanet, newPlanet.Width / 2 + 10 + m * 4, m));
                }
            }
            else if (int.Parse(MoonCount.Text) > 3)
            {
                for (int m = 1; m < 4; m++)
                {
                    newSystemObjects.Add(new Moon($"Moon {m}",
                        DataControl.CreateImage("moon.png"),
                        10, 10, true, 5, 5, newPlanet, newPlanet.Width / 2 + 10 + m * 4, m));
                }
            }

            PlanetList.Items.Add($"Planet {_counter}: " + newPlanet.Name + "\n     Image: " + ImageOption.IsChecked);
            ImageOption.IsChecked = false;
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            if(ImageOption.IsChecked == true)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "PNG Files |*.png";
                fileDialog.Title = "Select Picture";
                fileDialog.Multiselect = false;
                fileDialog.CheckFileExists = true;

                if (fileDialog.ShowDialog() == true)
                {
                    _newImage = DataControl.CreateImage(fileDialog.FileName);
                }
            }
        }

        private void CreateSystemButton_Click(object sender, RoutedEventArgs e)
        {
            if(newSystemObjects.Count == 0)
            {
                MessageBox.Show("System has no objects");
                return;
            }
            else if(SystemNameText.Text == String.Empty)
            {
                MessageBox.Show("Please enter system name.");
                return;
            }

            newSystemObjects.Add(_sun);

            SolarSystem newSystem = new SolarSystem 
            { 
                SystemName = SystemNameText.Text, 
                SystemPlanets = newSystemObjects, 
                PlanetCount = newSystemObjects.Count, 
                Description = SystemDescription.Text 
            };

            ((MainWindow)Application.Current.MainWindow).SystemList.Items.Add(newSystem);
            ((MainWindow)Application.Current.MainWindow).SystemList.SelectedValuePath = newSystem.SystemName;

            MessageBox.Show("A new system was added to the systems list.");

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ImageOption_Checked(object sender, RoutedEventArgs e)
        {
            AddImageButton.Visibility = Visibility.Visible;
        }

        private void ImageOption_Unchecked(object sender, RoutedEventArgs e)
        {
            AddImageButton.Visibility = Visibility.Hidden;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
