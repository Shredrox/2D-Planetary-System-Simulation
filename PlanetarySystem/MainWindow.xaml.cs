using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CelestialObjectsLibrary;

namespace PlanetarySystem
{
    public partial class MainWindow : Window
    {
        private List<Ellipse> orbits = new List<Ellipse>();
        private int systemIndex = 0;
        private int orbitIndex = 0;
        private bool solSystemLoaded = false;

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

        public MainWindow()
        { 
            InitializeComponent();

            sunImage = DataControl.CreateImage("../../Images/sun.png");
            earthImage = DataControl.CreateImage("../../Images/earth.png");
            marsImage = DataControl.CreateImage("../../Images/mars.png");
            mercuryImage = DataControl.CreateImage("../../Images/mercury.png");
            venusImage = DataControl.CreateImage("../../Images/venus.png");
            moonImage = DataControl.CreateImage("../../Images/moon.png");
            jupiterImage = DataControl.CreateImage("../../Images/jupiter.png");
            saturnImage = DataControl.CreateImage("../../Images/saturn.png");
            uranusImage = DataControl.CreateImage("../../Images/uranus.png");
            neptuneImage = DataControl.CreateImage("../../Images/neptune.png");
            cometImage = DataControl.CreateImage("../../Images/comet.png");

            //main window background
            BitmapImage windowBackground = DataControl.CreateImage("../../Images/windowBackground.png");
            ImageBrush imageBackground = new ImageBrush();
            imageBackground.ImageSource = windowBackground;
            this.Background = imageBackground;

            //planet info background
            BitmapImage windowBackground2 = DataControl.CreateImage("../../Images/windowBackground2.jpg");
            ImageBrush imageBackground2 = new ImageBrush();
            imageBackground2.ImageSource = windowBackground2;
            PlanetInfoPanel.Background = imageBackground2;

            //systeim info background
            BitmapImage windowBackground3 = DataControl.CreateImage("../../Images/windowBackground2.jpg");
            ImageBrush imageBackground3 = new ImageBrush();
            imageBackground3.ImageSource = windowBackground3;
            SystemInfoPanel.Background = imageBackground3;

            //canvas background
            backgroundImage = DataControl.CreateImage("../../Images/spaceCanvasBackground.jpg");
            ImageBrush image = new ImageBrush();
            image.ImageSource = backgroundImage;
            MainCanvas.Background = image;

            //zoom in and zoom out
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
            };

            DataControl.LoadFromFile(SystemList);
        }

        //draws planet orbits
        private void SystemOrbitCreation(int count)
        {
            int radius = 0;
            for (int i = 1; i <= count; i++)
            {
                switch (i)
                {
                    case 5: radius += 30; break;
                    case 7: radius -= 10; break;
                    case 8: radius -= 20; break;
                }
                radius += 50;

                Ellipse ellipse = new Ellipse();
                ellipse.RenderTransform = new TranslateTransform(MainCanvas.ActualWidth / 2 - radius, MainCanvas.ActualHeight / 2 - radius);
                ellipse.Width = radius * 2;
                ellipse.Height = radius * 2;
                ellipse.Stroke = Brushes.White;
                ellipse.StrokeThickness = 2;
                ellipse.MouseLeftButtonDown += EllipseClick;
                ellipse.MouseMove += EllipseSelected;

                orbits.Add(ellipse);
                MainCanvas.Children.Add(ellipse);
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

            var selectedOrbitIndex = orbits
                .FindIndex(o => o.Equals(
                    orbits
                    .Where(x => x.IsMouseOver == true)
                    .FirstOrDefault()));

            PlanetInfoTextChange(DataControl.SelectPlanet(selectedOrbitIndex));
            orbitIndex = selectedOrbitIndex;

            if (solSystemLoaded)
            {
                EditPlanetButton.Visibility = Visibility.Hidden;
                DeletePlanetButton.Visibility = Visibility.Hidden;
                LinkTextBlock.Visibility = Visibility.Visible;
                LinkName.Text = "More Info about " + DataControl.SelectPlanet(orbitIndex).Name;
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

            BitmapImage planetImage = DataControl.CreateImage(System.IO.Path.GetFullPath(planet.Image.ImageSource.ToString()));

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

        //stopping/playing the animation
        private bool IsOrbitOn = true;
        private void StopAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOrbitOn)
            {
                CompositionTarget.Rendering -= DataControl.AnimationUpdate;
                IsOrbitOn = false;
                StopAnimationButton.Content = "Resume Animation";
            }
            else
            {
                CompositionTarget.Rendering += DataControl.AnimationUpdate;
                IsOrbitOn = true;
                StopAnimationButton.Content = "Pause Animation";
            }
        }

        //opens the window for background change
        private void ChangeBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundChangeWindow backgroundChangeWindow = new BackgroundChangeWindow();
            backgroundChangeWindow.Show();
        }

        //loads a system from the combobox list of systems
        private void LoadSystem_Click(object sender, RoutedEventArgs e)
        {
            if (SystemList.Items.Count == 0)
            {
                return;
            }

            CompositionTarget.Rendering -= DataControl.AnimationUpdate;

            VisibilityChange();
            DeleteSystemButton.Visibility = Visibility.Visible;
            EditSystemButton.Visibility= Visibility.Visible;
            LinkTextBlock.Visibility = Visibility.Hidden;
            StopAnimationButton.Visibility = Visibility.Visible;
            StopAnimationButton.Content = "Pause Animation";

            IsOrbitOn = true;
            solSystemLoaded = false;

            if (DataControl.SystemObjectsCount() > 0 || orbits.Count > 0) 
            {
                DataControl.ClearSystemObjects();
                MainCanvas.Children.Clear();
                orbits.Clear();
            }

            Random randomNumber = new Random();
            Comet comet = new Comet("Comet", MainCanvas, cometImage, randomNumber.Next((int)MainCanvas.ActualWidth),
                randomNumber.Next((int)MainCanvas.ActualHeight), true, 20, 20);

            SystemOrbitCreation(((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.OfType<Planet>().Count());

            foreach (CelestialObject obj in ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets)
            {
                DataControl.AddObject(obj);
            }
            DataControl.AddObject(comet);

            DataControl.FillCanvas(MainCanvas);

            SystemName.Text = ((SolarSystem)SystemList.Items[systemIndex]).SystemName;

            CompositionTarget.Rendering += DataControl.AnimationUpdate;
        }

        //loads the Sol System
        private void SolSystemButton_Click(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= DataControl.AnimationUpdate;
            
            SystemName.Text = "Sol System";
            
            VisibilityChange();
            StopAnimationButton.Visibility = Visibility.Visible;
            StopAnimationButton.Content = "Pause Animation";

            solSystemLoaded = true;
            IsOrbitOn = true;

            if (DataControl.SystemObjectsCount() > 0 || orbits.Count > 0) 
            {
                DataControl.ClearSystemObjects();
                MainCanvas.Children.Clear();
                orbits.Clear();
            }

            SystemOrbitCreation(8);

            Random randomNumber = new Random();
            Comet comet = new Comet("Comet", MainCanvas, cometImage, randomNumber.Next((int)MainCanvas.ActualWidth),
                randomNumber.Next((int)MainCanvas.ActualHeight), true, 20, 20);

            Star sun = new Star("Sun", sunImage, MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2, 86, 86);
            Planet mercury = new Planet("Mercury", "Not Breathable", "88 Days", "58 Days",
                                        0, "Uninhabited", mercuryImage, 10, 10, true, 10, 10, sun, 50, -4);
            Planet venus = new Planet("Venus", "Not Breathable", "225 Days", "116 Days",
                                        0, "Uninhabited", venusImage, 10, 10, true, 10, 10, sun, 100, -3);
            Planet earth = new Planet("Earth", "Breathable", "365 Days", "1 Day / 24 Hours",
                                        1, "Inhabited", earthImage, 10, 10, true, 20, 20, sun, 150, -2);
            Moon moon = new Moon("Luna", moonImage, 10, 10, true, 5, 5, earth, 23, -4);
            Planet mars = new Planet("Mars", "Not Breathable", "687 Days", "1 Day / 24 Hours",
                                        2, "Uninhabited", marsImage, 10, 10, true, 20, 20, sun, 200, -1.5);
            Planet jupiter = new Planet("Jupiter", "Not Breathable", "12 Years", "10 Hours",
                                        79, "Uninhabited", jupiterImage, 10, 10, true, 30, 30, sun, 280, -1);
            Planet saturn = new Planet("Saturn", "Not Breathable", "29 Years", "11 Hours",
                                        82, "Uninhabited", saturnImage, 10, 10, true, 40, 30, sun, 330, -0.75);
            Planet uranus = new Planet("Uranus", "Not Breathable", "84 Years", "17 Hours",
                                        27, "Uninhabited", uranusImage, 10, 10, true, 30, 30, sun, 370, -0.5);
            Planet neptune = new Planet("Neptune", "Not Breathable", "165 Years", "16 Hours",
                                        14, "Uninhabited", neptuneImage, 10, 10, true, 40, 30, sun, 400, -0.25);

            DataControl.AddObject(mercury);
            DataControl.AddObject(venus);
            DataControl.AddObject(earth);
            DataControl.AddObject(mars);
            DataControl.AddObject(jupiter);
            DataControl.AddObject(saturn);
            DataControl.AddObject(uranus);
            DataControl.AddObject(neptune);
            DataControl.AddObject(sun);
            DataControl.AddObject(moon);
            DataControl.AddObject(comet);

            DataControl.FillCanvas(MainCanvas);

            CompositionTarget.Rendering += DataControl.AnimationUpdate;
        }

        //opens a new window for planet editing
        private void EditPlanetButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPlanet = DataControl.SelectPlanet(orbitIndex);
            int selectedPlanetIndex = DataControl.GetPlanetIndex(selectedPlanet);

            PlanetEditWindow planetEdit = new PlanetEditWindow(selectedPlanet);
            planetEdit.Show();
            
            planetEdit.Closed += delegate
            {
                DataControl.UpdatePlanet(selectedPlanet, selectedPlanetIndex);
                DataControl.RemoveMoons(selectedPlanet, SystemList, systemIndex);
                DataControl.AddMoons(selectedPlanet, SystemList, systemIndex, selectedPlanetIndex);

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
                var planetToRemove = DataControl.SelectPlanet(orbitIndex);

                DataControl.RemoveObject(planetToRemove);
                ((SolarSystem)SystemList.Items[systemIndex]).SystemPlanets.Remove(planetToRemove);
                DataControl.RemoveMoons(planetToRemove, SystemList, systemIndex);

                MainCanvas.Children.Clear();
                orbits.Clear();
                SystemOrbitCreation(DataControl.PlanetCount());
                DataControl.SetOrbitRadius();
                DataControl.FillCanvas(MainCanvas);

                VisibilityChange();
                DeleteSystemButton.Visibility = Visibility.Visible;
                EditSystemButton.Visibility = Visibility.Visible;
            }
        }

        //opens a new window for system creation
        private void CreateSystemButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCreationWindow systemCreationWindow = new SystemCreationWindow();
            systemCreationWindow.Show();
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
                return;
            }

            AddPlanetsWindow addPlanetsWindow = new AddPlanetsWindow((SolarSystem)SystemList.Items[systemIndex]);
            addPlanetsWindow.Show();

            addPlanetsWindow.Closed += delegate
            {
                SystemList.SelectedValuePath = ((SolarSystem)SystemList.Items[systemIndex]).SystemName;
                SystemList.Items.Refresh();

                LoadSystem_Click(sender, e);
            };
        }

        //changes combobox text to selected system name
        private void SystemList_DropDownOpened(object sender, EventArgs e)
        {
            if (SystemList.SelectedIndex != -1)
            {
                SystemList.Text = ((SolarSystem)SystemList.Items[SystemList.SelectedIndex]).SystemName;
            }
        }

        //changes the selected system
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
            if (!solSystemLoaded)
            {
                e.Handled = false;
                return;
            }

            string link = "https://en.wikipedia.org/wiki/";
            link += DataControl.SelectPlanet(orbitIndex).Name + "_(planet)";
            Process.Start(new ProcessStartInfo(link));
            e.Handled = true;
        }

        //dragging the window
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //closes the program
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DataControl.SaveToFile(SystemList);
            MainCanvas.Children.Clear();
            DataControl.ClearSystemObjects();
            DataControl.ImageDelete();
            this.Close();
        }
    }
}