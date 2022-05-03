using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PlanetarySystem
{
    public partial class SystemCreationWindow : Window
    {
        public List<CelestialObject> newSystemObjects = new List<CelestialObject>();
        private BitmapImage defaultImage;
        private BitmapImage newImage;
        private int counter = 0;
        private Star sun = new Star("Sun", ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/sun.png"),
                                    ((MainWindow)Application.Current.MainWindow).MainCanvas.ActualWidth / 2,
                                    ((MainWindow)Application.Current.MainWindow).MainCanvas.ActualHeight / 2, 86, 86);

        public SystemCreationWindow()
        {
            InitializeComponent();

            defaultImage = ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/defaultPlanet.png");

            AddImageButton.Visibility = Visibility.Hidden;
        }

        private void CreatePlanetButton_Click(object sender, RoutedEventArgs e)
        {
            if(newSystemObjects.OfType<Planet>().Count() == 8)
            {
                MessageBox.Show("Maximum number of planets reached.");
            }

            if (Name.Text == String.Empty || Atmosphere.Text == String.Empty
                || OrbitalPeriod.Text == String.Empty || RotationPeriod.Text == String.Empty
                || MoonCount.Text == String.Empty || Life.Text == String.Empty || Speed.Text == String.Empty ||
                Width.Text == String.Empty || Height.Text == String.Empty)
            {
                MessageBox.Show("Missing info for new planet.");
            }
            else
            {
                counter++;
            }

            if (Name.Text != String.Empty && Atmosphere.Text != String.Empty
                && OrbitalPeriod.Text != String.Empty && RotationPeriod.Text != String.Empty
                && MoonCount.Text != String.Empty && Life.Text != String.Empty && Speed.Text != String.Empty && 
                Width.Text != String.Empty && Height.Text != String.Empty && 
                newSystemObjects.OfType<Planet>().Count() < 8)  
            {
                if(!int.TryParse(MoonCount.Text, out _) || !int.TryParse(Width.Text, out _) || !int.TryParse(Height.Text, out _) || !double.TryParse(Speed.Text, out _))
                {
                    MessageBox.Show("Input was not in the correct format.");
                    counter = 0;
                    return;
                }

                int radius = 0;
                switch (counter)
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

                Planet newPlanet;

                if (ImageOption.IsChecked == true)
                {
                    newPlanet = new Planet(Name.Text, Atmosphere.Text, OrbitalPeriod.Text, RotationPeriod.Text, int.Parse(MoonCount.Text), Life.Text,
                    newImage, 10, 10, true, int.Parse(Width.Text), int.Parse(Height.Text), sun, radius, double.Parse(Speed.Text));
                }
                else
                {
                    newPlanet = new Planet(Name.Text, Atmosphere.Text, OrbitalPeriod.Text, RotationPeriod.Text, int.Parse(MoonCount.Text), Life.Text,
                    defaultImage, 10, 10, true, int.Parse(Width.Text), int.Parse(Height.Text), sun, radius, double.Parse(Speed.Text));
                }

                newSystemObjects.Add(newPlanet);

                if (int.Parse(MoonCount.Text) <= 3 && int.Parse(MoonCount.Text) > 0)
                {
                    for (int m = 1; m < int.Parse(MoonCount.Text) + 1; m++)
                    {
                        newSystemObjects.Add(new Moon($"Moon {m}",
                            ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/moon.png"), 
                            10, 10, true, 5, 5, newPlanet, newPlanet.Width/2 + 10 + m * 4, m));
                    }
                }
                else if (int.Parse(MoonCount.Text) > 3) 
                {
                    for (int m = 1; m < 4; m++)
                    {
                        newSystemObjects.Add(new Moon($"Moon {m}",
                            ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/moon.png"), 
                            10, 10, true, 5, 5, newPlanet, newPlanet.Width / 2 + 10 + m * 4, m));
                    }
                }

                PlanetList.Items.Add($"Planet {counter}: " + newPlanet.Name + "     Image: " + ImageOption.IsChecked);
            }
        }

        private void CreateSystemButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemNameText.Text != String.Empty)
            {
                newSystemObjects.Add((sun));

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                SolarSystem newSystem = new SolarSystem { SystemName = SystemNameText.Text, SystemPlanets = newSystemObjects, PlanetCount = newSystemObjects.Count };

                mainWindow.SystemList.Items.Add(newSystem);
                mainWindow.SystemList.SelectedValuePath = newSystem.SystemName;

                this.Close();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
                    newImage = ((MainWindow)Application.Current.MainWindow).CreateImage(fileDialog.FileName);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ImageOption_Checked(object sender, RoutedEventArgs e)
        {
            AddImageButton.Visibility = Visibility.Visible;
        }

        private void ImageOption_Unchecked(object sender, RoutedEventArgs e)
        {
            AddImageButton.Visibility = Visibility.Hidden;
        }
    }
}
