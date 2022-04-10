using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PlanetarySystem
{
    public partial class MainWindow : Window
    {
        private List<CelestialObject> systemObjects = new List<CelestialObject>();
        private List<Ellipse> orbits = new List<Ellipse>();
        private List<string> paths = new List<string>();
        private int systemIndex = 0;
        private int orbitIndex = 0;
        private string path = System.IO.Path.GetFullPath("../../Data/PlanetData.txt");

        //images
        private BitmapImage sunImage;
        private BitmapImage earthImage;
        private BitmapImage marsImage;
        private BitmapImage mercuryImage;
        private BitmapImage venusImage;
        private BitmapImage moonImage;
        private BitmapImage jupiterImage;
        private BitmapImage saturnImage;
        private BitmapImage uranusImage;
        private BitmapImage neptuneImage;
        private BitmapImage cometImage;
        private BitmapImage backgroundImage;

        //function to create images 
        public BitmapImage CreateImage(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        public MainWindow()
        { 
            InitializeComponent();

            sunImage = CreateImage("../../Images/sun.png");
            earthImage = CreateImage("../../Images/earth.png");
            marsImage = CreateImage("../../Images/mars.png");
            mercuryImage = CreateImage("../../Images/mercury.png");
            venusImage = CreateImage("../../Images/venus.png");
            moonImage = CreateImage("../../Images/moon.png");
            jupiterImage = CreateImage("../../Images/jupiter.png");
            saturnImage = CreateImage("../../Images/saturn.png");
            uranusImage = CreateImage("../../Images/uranus.png");
            neptuneImage = CreateImage("../../Images/neptune.png");
            cometImage = CreateImage("../../Images/comet.png");

            //main window background
            BitmapImage windowBackground = CreateImage("../../Images/windowBackground.png");
            ImageBrush imageBackground = new ImageBrush();
            imageBackground.ImageSource = windowBackground;
            this.Background = imageBackground;

            //planet info background
            BitmapImage windowBackground2 = CreateImage("../../Images/windowBackground2.jpg");
            ImageBrush imageBackground2 = new ImageBrush();
            imageBackground2.ImageSource = windowBackground2;
            PlanetInfoPanel.Background = imageBackground2;

            //canvas background
            backgroundImage = CreateImage("../../Images/spaceCanvasBackground.jpg");
            ImageBrush image = new ImageBrush();
            image.ImageSource = backgroundImage;
            MainCanvas.Background = image;

            var mt = new MatrixTransform();
            MainCanvas.RenderTransform = mt;
            MainCanvas.MouseWheel += (s, e) =>
            {
                if (mt.Value.OffsetX <= 0 && mt.Value.OffsetY <= 0 && e.Delta > 0) 
                {
                    var matrix = mt.Matrix;
                    var mousePosition = e.GetPosition(MainCanvas);
                    var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;
                    matrix.ScaleAt(scale, scale, mousePosition.X, mousePosition.Y);
                    mt.Matrix = matrix;
                    e.Handled = true;
                }
                else if(mt.Value.OffsetX < 0 && mt.Value.OffsetY < 0 && e.Delta < 0)
                {
                    var matrix = mt.Matrix;
                    var mousePosition = e.GetPosition(MainCanvas);
                    var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;
                    matrix.ScaleAt(scale, scale, mousePosition.X, mousePosition.Y);
                    if (matrix.OffsetX > 0 || matrix.OffsetY > 0)
                    {
                        matrix.SetIdentity();
                        mt.Matrix = matrix;
                        e.Handled = true;
                        return;
                    }
                    mt.Matrix = matrix;
                    e.Handled = true;
                }
                else if (mt.Value.OffsetX == 0 && mt.Value.OffsetY == 0 && e.Delta < 0)
                {
                    e.Handled = true;
                    return;
                }
            };

            LoadDataFromFile();
        }
        
        //loads data from file
        private void LoadDataFromFile()
        {
            if (File.Exists(path))
            {
                string[] fileLines = File.ReadAllLines(path);
                List<CelestialObject> planets = new List<CelestialObject>();
                Star sun = new Star("Sun", sunImage, 450, 425, 86, 86);

                for (int i = 0; i < fileLines.Length; i++)
                {
                    if (fileLines[i] == String.Empty)
                    {
                        return;
                    }
                    var lineContent = fileLines[i].Split('\t');

                    for (int k = 1; k < lineContent.Length; k++)
                    {
                        var lineElements = lineContent[k].Split('|');

                        BitmapImage newImage = CreateImage(lineElements[6]);

                        Planet newPlanet = new Planet(lineElements[0], lineElements[1], lineElements[2], lineElements[3], int.Parse(lineElements[4]),
                                                 lineElements[5], newImage, 100, 100, true, int.Parse(lineElements[7]),
                                                 int.Parse(lineElements[8]), sun, int.Parse(lineElements[9]), double.Parse(lineElements[10]));

                        planets.Add(newPlanet);

                        if (newPlanet.MoonCount <= 3 && newPlanet.MoonCount > 0)
                        {
                            for (int m = 1; m < newPlanet.MoonCount + 1; m++)
                            {
                                planets.Add(new Moon($"Moon {m}", moonImage, 10, 10, true, 5, 5, newPlanet, newPlanet.Width / 2 + 10 + m * 4, m));
                            }
                        }
                        else if (newPlanet.MoonCount > 3)
                        {
                            for (int m = 1; m < 4; m++)
                            {
                                planets.Add(new Moon($"Moon {m}", moonImage, 10, 10, true, 5, 5, newPlanet, newPlanet.Width / 2 + 10 + m * 4, m));
                            }
                        }
                    }

                    planets.Add(sun);
                    SolarSystem solarSystem = new SolarSystem { SystemName = lineContent[0], PlanetCount = planets.Count, SystemPlanets = planets };
                    SystemList.Items.Add(solarSystem);
                    SystemList.SelectedValuePath = solarSystem.SystemName;
                    planets = new List<CelestialObject>();
                }
            }
        }

        //saves data to file
        private void SaveDataToFile()
        {
            if (SystemList.Items.Count > 0)
            {
                string spliter = "\t";

                using (StreamWriter file = File.CreateText(path))
                {
                    foreach (SolarSystem system in SystemList.Items)
                    {
                        system.SystemPlanets.RemoveAll(x => x.GetType() == typeof(Star));
                        system.SystemPlanets.RemoveAll(x => x.GetType() == typeof(Moon));

                        for (int i = 0; i < system.SystemPlanets.Count; i++)
                        {
                            paths.Add(system.SystemPlanets[i].Image.ImageSource.ToString());
                        }
                        ImageCopy(paths);

                        file.Write(system.SystemName + "\t");
                        for (int i = 0; i < system.SystemPlanets.Count; i++)
                        {
                            if (system.SystemPlanets[i].GetType() == typeof(Planet))
                            {
                                if (i == system.SystemPlanets.Count - 1)
                                {
                                    spliter = string.Empty;
                                }
                                string name = system.SystemPlanets[i].Name;
                                string atmosphere = ((Planet)system.SystemPlanets[i]).Atmosphere;
                                string orbitalPeriod = ((Planet)system.SystemPlanets[i]).OrbitalPeriod;
                                string rotationPeriod = ((Planet)system.SystemPlanets[i]).RotationPeriod;
                                int moonCount = ((Planet)system.SystemPlanets[i]).MoonCount;
                                string life = ((Planet)system.SystemPlanets[i]).Life;
                                string image = paths[i];
                                int width = system.SystemPlanets[i].Width;
                                int height = system.SystemPlanets[i].Height;
                                int radius = ((Planet)system.SystemPlanets[i]).Radius;
                                double speed = ((Planet)system.SystemPlanets[i]).Speed;
                                file.Write(name + "|" + atmosphere + "|" + orbitalPeriod + "|" + rotationPeriod + "|" +
                                           moonCount + "|" + life + "|" + image + "|" + width + "|" + height + "|" + radius +
                                           "|" + speed + "|" + spliter);
                            }
                        }
                        spliter = "\t";
                        file.WriteLine();
                        paths.Clear();
                    }
                    file.Flush();
                    file.Dispose();
                }
            }
        }

        //copies newly added images to project folder for reuse
        private void ImageCopy(List<String> paths)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i].Contains("file://"))
                {
                    string[] pathSplit = paths[i].Split(new string[] { "///" }, StringSplitOptions.None);
                    string path = pathSplit[1];
                    string name = System.IO.Path.GetFileName(path);
                    string combinedPath = System.IO.Path.Combine("../../UserImages/", name);
                    File.Copy(path, combinedPath, true);
                    paths[i] = combinedPath;
                }
            }
        }

        //draws planet orbits
        private void OrbitDraw(double scalar)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.RenderTransform = new TranslateTransform(MainCanvas.ActualWidth / 2 - scalar, MainCanvas.ActualHeight / 2 - scalar);
            ellipse.Width = scalar * 2;
            ellipse.Height = scalar * 2;
            ellipse.Stroke = Brushes.White;
            ellipse.StrokeThickness = 2;
            ellipse.MouseLeftButtonDown += EllipseClick;
            ellipse.MouseMove += EllipseSelected;

            orbits.Add(ellipse);
            MainCanvas.Children.Add(ellipse);
        }

        //sets the orbit count depending on how much planets there are
        private void SystemOrbitCreation(int count)
        {
            switch (count)
            {
                case 1:
                    OrbitDraw(50);
                    break;
                case 2:
                    OrbitDraw(50);
                    OrbitDraw(100);
                    break;
                case 3:
                    OrbitDraw(50);
                    OrbitDraw(100);
                    OrbitDraw(150);
                    break;
                case 4:
                    OrbitDraw(50);
                    OrbitDraw(100);
                    OrbitDraw(150);
                    OrbitDraw(200);
                    break;
                case 5:
                    OrbitDraw(50);
                    OrbitDraw(100);
                    OrbitDraw(150);
                    OrbitDraw(200);
                    OrbitDraw(280);
                    break;
                case 6:
                    OrbitDraw(50);
                    OrbitDraw(100);
                    OrbitDraw(150);
                    OrbitDraw(200);
                    OrbitDraw(280);
                    OrbitDraw(330);
                    break;
                case 7:
                    OrbitDraw(50);
                    OrbitDraw(100);
                    OrbitDraw(150);
                    OrbitDraw(200);
                    OrbitDraw(280);
                    OrbitDraw(330);
                    OrbitDraw(370);
                    break;
                case 8:
                    OrbitDraw(50);
                    OrbitDraw(100);
                    OrbitDraw(150);
                    OrbitDraw(200);
                    OrbitDraw(280);
                    OrbitDraw(330);
                    OrbitDraw(370);
                    OrbitDraw(400);
                    break;
            }
        }

        //an event for orbit selection
        private void EllipseSelected(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < orbits.Count; i++)
            {
                if (orbits[i].IsMouseOver == true)
                {
                    orbits[i].StrokeThickness = 5;
                }
                else
                {
                    orbits[i].StrokeThickness = 2;
                }
            }
        }

        //an event for orbit click
        private void EllipseClick(object sender, MouseButtonEventArgs e)
        {
            if (PlanetInfo.Visibility == Visibility.Hidden)
            {
                EditPlanetButton.Visibility = Visibility.Visible;
                DeletePlanetButton.Visibility = Visibility.Visible;
                PlanetInfo.Visibility = Visibility.Visible;
                PlanetAtmosphere.Visibility = Visibility.Visible;
                PlanetOrbitalPeriod.Visibility = Visibility.Visible;
                PlanetRotationPeriod.Visibility = Visibility.Visible;
                PlanetMoonCount.Visibility = Visibility.Visible;
                PlanetLife.Visibility = Visibility.Visible;
                SelectedPlanetImage.Visibility = Visibility.Visible;
                PlanetNameCard.Visibility = Visibility.Visible;
                PlanetAtmoCard.Visibility = Visibility.Visible;
                PlanetOrbitalPeriodCard.Visibility = Visibility.Visible;
                PlanetRotationPerdiodCard.Visibility = Visibility.Visible;
                PlanetMoonCountCard.Visibility = Visibility.Visible;
                PlanetLifeCard.Visibility = Visibility.Visible;
            }

            var onlyPlanets = systemObjects.Where(s => s.GetType() == typeof(Planet));

            for (int i = 0; i < orbits.Count; i++)
            {
                if (orbits[i].IsMouseOver == true)
                {
                    orbitIndex = i;
                    PlanetInfoTextChange(onlyPlanets.ElementAt(i));
                }
            }

            if (SystemName.Text == "Sol System")
            {
                EditPlanetButton.Visibility = Visibility.Hidden;
                DeletePlanetButton.Visibility = Visibility.Hidden;
                LinkTextBlock.Visibility = Visibility.Visible;
                LinkName.Text = "More Info about " + systemObjects[orbitIndex].Name;
            }
        }

        //animates the planets by calling the update method of each object
        private void AnimationUpdate(object sender, EventArgs e)
        {
            foreach (CelestialObject obj in systemObjects)
            {
                obj.Update();
            }
        }

        //changes the textblocks to the selected planet info
        private void PlanetInfoTextChange(CelestialObject planet)
        {
            PlanetInfo.Text = planet.Name;
            PlanetAtmosphere.Text = "Atmosphere: " + ((Planet)planet).Atmosphere;
            PlanetOrbitalPeriod.Text = "Orbital Period: " + ((Planet)planet).OrbitalPeriod;
            PlanetRotationPeriod.Text = "Rotation Period: " + ((Planet)planet).RotationPeriod;
            PlanetMoonCount.Text = "Moon Count: " + ((Planet)planet).MoonCount.ToString();
            PlanetLife.Text = "Life: " + ((Planet)planet).Life;

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

            BitmapImage planetImage = CreateImage(fullPath);

            SelectedPlanetImage.Source = planetImage;
        }

        //changes the visivility of window elements
        private void VisibilityChange()
        {
            PlanetInfo.Visibility = Visibility.Hidden;
            PlanetAtmosphere.Visibility = Visibility.Hidden;
            PlanetOrbitalPeriod.Visibility = Visibility.Hidden;
            PlanetRotationPeriod.Visibility = Visibility.Hidden;
            PlanetMoonCount.Visibility = Visibility.Hidden;
            PlanetLife.Visibility = Visibility.Hidden;
            SelectedPlanetImage.Visibility = Visibility.Hidden;
            EditPlanetButton.Visibility = Visibility.Hidden;
            DeletePlanetButton.Visibility = Visibility.Hidden;
            DeleteSystemButton.Visibility = Visibility.Hidden;
            EditSystemButton.Visibility = Visibility.Hidden;
            PlanetNameCard.Visibility = Visibility.Hidden;
            PlanetAtmoCard.Visibility = Visibility.Hidden;
            PlanetOrbitalPeriodCard.Visibility = Visibility.Hidden;
            PlanetRotationPerdiodCard.Visibility = Visibility.Hidden;
            PlanetMoonCountCard.Visibility = Visibility.Hidden;
            PlanetLifeCard.Visibility = Visibility.Hidden;
        }

        //loads a system from the combobox list of systems
        private void LoadSystem_Click(object sender, RoutedEventArgs e)
        {
            if (SystemList.Items.Count == 0)
            {
                return;
            }

            CompositionTarget.Rendering -= AnimationUpdate;

            VisibilityChange();
            DeleteSystemButton.Visibility = Visibility.Visible;
            EditSystemButton.Visibility= Visibility.Visible;
            LinkTextBlock.Visibility = Visibility.Hidden;
            StopAnimationButton.Visibility = Visibility.Visible;
            IsOrbitOn = true;
            StopAnimationButton.Content = "Pause Animation";

            if (systemObjects.Count > 0 || orbits.Count > 0) 
            {
                systemObjects.Clear();
                MainCanvas.Children.Clear();
                orbits.Clear();
            }

            Random randomNumber = new Random();
            Comet comet = new Comet("Comet", MainCanvas, cometImage, randomNumber.Next((int)MainCanvas.ActualWidth),
                randomNumber.Next((int)MainCanvas.ActualHeight), true, 20, 20);

            SystemOrbitCreation(((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.OfType<Planet>().Count());

            foreach (CelestialObject obj in ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets)
            {
                systemObjects.Add(obj);
                MainCanvas.Children.Add(obj.Shape);
            }

            systemObjects.Add(comet);
            MainCanvas.Children.Add(comet.Shape);

            SystemName.Text = ((SolarSystem)SystemList.Items[systemIndex]).SystemName;

            CompositionTarget.Rendering += AnimationUpdate;
        }

        //loads the Sol System
        private void SolSystemButton_Click(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= AnimationUpdate;
            
            SystemName.Text = "Sol System";
            VisibilityChange();
            StopAnimationButton.Visibility = Visibility.Visible;
            IsOrbitOn = true;
            StopAnimationButton.Content = "Pause Animation";

            if (systemObjects.Count > 0 || orbits.Count > 0) 
            {
                systemObjects.Clear();
                MainCanvas.Children.Clear();
                orbits.Clear();
            }

            SystemOrbitCreation(8);

            Random randomNumber = new Random();
            Comet comet = new Comet("Comet", MainCanvas, cometImage, randomNumber.Next((int)MainCanvas.ActualWidth),
                randomNumber.Next((int)MainCanvas.ActualHeight), true, 20, 20);

            Star sun = new Star("Sun", sunImage, MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2, 86, 86);
            Planet mercury = new Planet("Mercury", "Not Breathable", "88 Days", "58 Days",
                                        0, "Uninhabited", mercuryImage, 10, 10, true, 10, 10, sun, 50, 4);
            Planet venus = new Planet("Venus", "Not Breathable", "225 Days", "116 Days",
                                        0, "Uninhabited", venusImage, 10, 10, true, 10, 10, sun, 100, 3);
            Planet earth = new Planet("Earth", "Breathable", "365 Days", "1 Day / 24 Hours",
                                        1, "Inhabited", earthImage, 10, 10, true, 20, 20, sun, 150, 2);
            Moon moon = new Moon("Luna", moonImage, 10, 10, true, 5, 5, earth, 23, 4);
            Planet mars = new Planet("Mars", "Not Breathable", "687 Days", "1 Day / 24 Hours",
                                        2, "Uninhabited", marsImage, 10, 10, true, 20, 20, sun, 200, 1.5);
            Planet jupiter = new Planet("Jupiter", "Not Breathable", "12 Years", "10 Hours",
                                        79, "Uninhabited", jupiterImage, 10, 10, true, 30, 30, sun, 280, 1);
            Planet saturn = new Planet("Saturn", "Not Breathable", "29 Years", "11 Hours",
                                        82, "Uninhabited", saturnImage, 10, 10, true, 40, 30, sun, 330, 0.75);
            Planet uranus = new Planet("Uranus", "Not Breathable", "84 Years", "17 Hours",
                                        27, "Uninhabited", uranusImage, 10, 10, true, 30, 30, sun, 370, 0.5);
            Planet neptune = new Planet("Neptune", "Not Breathable", "165 Years", "16 Hours",
                                        14, "Uninhabited", neptuneImage, 10, 10, true, 40, 30, sun, 400, 0.25);

            systemObjects.Add(mercury);
            systemObjects.Add(venus);
            systemObjects.Add(earth);
            systemObjects.Add(mars);
            systemObjects.Add(jupiter);
            systemObjects.Add(saturn);
            systemObjects.Add(uranus);
            systemObjects.Add(neptune);
            systemObjects.Add(sun);
            systemObjects.Add(moon);
            systemObjects.Add(comet);

            for (int i = 0; i < systemObjects.Count; i++)
            {
                MainCanvas.Children.Add(systemObjects[i].Shape);
            }

            CompositionTarget.Rendering += AnimationUpdate;
        }

        //opens a new window for system creation
        private void CreateSystemButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCreationWindow systemCreationWindow = new SystemCreationWindow();
            systemCreationWindow.Show();
        }

        //opens a new window for planet editing
        private void EditPlanetButton_Click(object sender, RoutedEventArgs e)
        {
            var onlyPlanets = systemObjects.Where(s => s.GetType() == typeof(Planet)).ToList();
            var selectedPlanet = onlyPlanets[orbitIndex];
            int selectedPlanetIndex = systemObjects.FindIndex(p => p.Equals(selectedPlanet));

            PlanetEditWindow planetEdit = new PlanetEditWindow(selectedPlanet);
            planetEdit.Show();
            
            planetEdit.Closed += delegate
            {
                systemObjects[selectedPlanetIndex] = selectedPlanet;

                for (int i = 0; i < ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.Count; i++)
                {
                    if (systemObjects[i].GetType() == typeof(Moon) && ((Moon)systemObjects[i]).GravityCenter == selectedPlanet)
                    {
                        ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.RemoveAt(i);
                        systemObjects.RemoveAt(i);
                        i--;
                    }
                }

                if (((Planet)selectedPlanet).MoonCount <= 3 && ((Planet)selectedPlanet).MoonCount > 0)
                {
                    for (int m = 1; m < ((Planet)selectedPlanet).MoonCount + 1; m++)
                    {
                        ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.Add(new Moon($"Moon {m}", moonImage, 10, 10, true, 5, 5, selectedPlanet, selectedPlanet.Width / 2 + 10 + m * 4, m));
                        systemObjects.Add(new Moon($"Moon {m}", moonImage, 10, 10, true, 5, 5, selectedPlanet, selectedPlanet.Width / 2 + 10 + m * 4, m));
                    }
                }
                else if (((Planet)selectedPlanet).MoonCount > 3)
                {
                    for (int m = 1; m < 4; m++)
                    {
                        ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.Add(new Moon($"Moon {m}", moonImage, 10, 10, true, 5, 5, selectedPlanet, selectedPlanet.Width / 2 + 10 + m * 4, m));
                        systemObjects.Add(new Moon($"Moon {m}", moonImage, 10, 10, true, 5, 5, selectedPlanet, selectedPlanet.Width / 2 + 10 + m * 4, m));
                    }
                }

                PlanetInfoTextChange(selectedPlanet);
                LoadSystem_Click(sender, e);
            };
        }

        //deletes the selected planet
        private void DeletePlanetButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this planet?",
                    "Deleting Planet", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var onlyPlanets = systemObjects.Where(s => s.GetType() == typeof(Planet)).ToList();
                var planetToRemove = onlyPlanets[orbitIndex];
                int planetToRemoveIndex = systemObjects.FindIndex(p => p.Equals(planetToRemove));
                systemObjects.Remove(planetToRemove);
                ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.Remove(planetToRemove);

                for (int i = 0; i < systemObjects.Count; i++)
                {
                    if (systemObjects[i].GetType() == typeof(Moon) && ((Moon)systemObjects[i]).GravityCenter == planetToRemove)
                    {
                        ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.RemoveAt(i);
                        systemObjects.RemoveAt(i);
                        i--;
                    }
                }

                MainCanvas.Children.Clear();
                orbits.Clear();
                SystemOrbitCreation(systemObjects.OfType<Planet>().Count());

                int orbitCounter = 1;
                for (int p = 0; p < systemObjects.Count; p++)
                {
                    if (systemObjects[p].GetType() == typeof(Planet))
                    {
                        switch (orbitCounter)
                        {
                            case 1: ((Planet)systemObjects[p]).Radius = 50; break;
                            case 2: ((Planet)systemObjects[p]).Radius = 100; break;
                            case 3: ((Planet)systemObjects[p]).Radius = 150; break;
                            case 4: ((Planet)systemObjects[p]).Radius = 200; break;
                            case 5: ((Planet)systemObjects[p]).Radius = 280; break;
                            case 6: ((Planet)systemObjects[p]).Radius = 330; break;
                            case 7: ((Planet)systemObjects[p]).Radius = 370; break;
                            case 8: ((Planet)systemObjects[p]).Radius = 400; break;
                        }
                        orbitCounter++;
                    }
                }

                for (int i = 0; i < systemObjects.Count; i++)
                {
                    MainCanvas.Children.Add(systemObjects[i].Shape);
                }
                VisibilityChange();
                DeleteSystemButton.Visibility = Visibility.Visible;
                EditSystemButton.Visibility = Visibility.Visible;
            }
        }

        //deletes the selected system
        private void DeleteSystemButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemList.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this system?",
                    "Deleting System", MessageBoxButton.YesNo) == MessageBoxResult.Yes) 
                {
                    systemIndex = SystemList.SelectedIndex;
                    SystemList.Items.RemoveAt(systemIndex);
                    MainCanvas.Children.Clear();
                    SystemList.Items.Refresh();
                    SystemList.Text = "Systems";
                    VisibilityChange();
                }
            }
        }

        //button to edit selected system
        private void EditSystemButton_Click(object sender, RoutedEventArgs e)
        {
            if (((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.OfType<Planet>().Count() == 8)
            {
                MessageBox.Show("This system has the max number of planets.");
            }
            else
            {
                AddPlanetsWindow addPlanetsWindow = new AddPlanetsWindow((SolarSystem)SystemList.Items[systemIndex]);
                addPlanetsWindow.Show();

                addPlanetsWindow.Closed += delegate
                {
                    SystemList.SelectedValuePath = ((SolarSystem)SystemList.Items[systemIndex]).SystemName;
                    SystemList.Items.Refresh();
                    LoadSystem_Click(sender, e);
                };
            }
        }

        //changes combobox text to selected system name
        private void SystemList_DropDownOpened(object sender, EventArgs e)
        {
            if (SystemList.SelectedIndex != -1)
            {
                SystemList.Text = ((SolarSystem)SystemList.Items[SystemList.SelectedIndex]).SystemName;
            }
        }

        //changes trhe selected system
        private void SystemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemList.SelectedItem != null)
            {
                systemIndex = SystemList.SelectedIndex;
                SystemName.Text = ((SolarSystem)SystemList.SelectedItem).SystemName;
            }
        }

        //opens wikipedia with e link
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (SystemName.Text == "Sol System")
            {
                string link = "https://en.wikipedia.org/wiki/";
                link += systemObjects[orbitIndex].Name + "_(planet)";
                Process.Start(new ProcessStartInfo(link));
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        //dragging the window
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //closes the program
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToFile();
            MainCanvas.Children.Clear();
            systemObjects.Clear();
            this.Close();
        }

        private bool IsOrbitOn = true;
        private void StopAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOrbitOn)
            {
                CompositionTarget.Rendering -= AnimationUpdate;
                IsOrbitOn = false;
                StopAnimationButton.Content = "Resume Animation";
            }
            else
            {
                CompositionTarget.Rendering += AnimationUpdate;
                IsOrbitOn = true;
                StopAnimationButton.Content = "Pause Animation";
            }
        }

        private void ChangeBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundChangeWindow backgroundChangeWindow = new BackgroundChangeWindow();
            backgroundChangeWindow.Show();
        }
    }
}