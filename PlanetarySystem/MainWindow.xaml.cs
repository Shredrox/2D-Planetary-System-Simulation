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
        private DataControl _dataControl = new DataControl();
        private List<Ellipse> _orbits = new List<Ellipse>();
        private int _systemIndex = 0;
        private int _orbitIndex = 0;
        private bool _solSystemLoaded = false;

        private MatrixTransform _mt;
        private Point _lastMousePos;
        private bool _isCanvasDragging = false;

        //sol system images
        private readonly BitmapImage _sunImage = DataControl.CreateImage("sun.png");
        private readonly BitmapImage _earthImage = DataControl.CreateImage("earth.png");
        private readonly BitmapImage _marsImage = DataControl.CreateImage("mars.png");
        private readonly BitmapImage _mercuryImage = DataControl.CreateImage("mercury.png");
        private readonly BitmapImage _venusImage = DataControl.CreateImage("venus.png");
        private readonly BitmapImage _moonImage = DataControl.CreateImage("moon.png");
        private readonly BitmapImage _jupiterImage = DataControl.CreateImage("jupiter.png");
        private readonly BitmapImage _saturnImage = DataControl.CreateImage("saturn.png");
        private readonly BitmapImage _uranusImage = DataControl.CreateImage("uranus.png");
        private readonly BitmapImage _neptuneImage = DataControl.CreateImage("neptune.png");
        private readonly BitmapImage _cometImage = DataControl.CreateImage("comet.png");

        public MainWindow()
        {
            InitializeComponent();

            System.IO.Directory.CreateDirectory(DataControl.userImagesDir);

            //main window background
            BitmapImage windowBackground = DataControl.CreateImage("windowBackground.png");
            ImageBrush imageBackground = new ImageBrush();
            imageBackground.ImageSource = windowBackground;
            this.Background = imageBackground;

            //planet info background
            BitmapImage windowBackground2 = DataControl.CreateImage("windowBackground2.jpg");
            ImageBrush imageBackground2 = new ImageBrush();
            imageBackground2.ImageSource = windowBackground2;
            PlanetInfoPanel.Background = imageBackground2;

            //system info background
            BitmapImage windowBackground3 = DataControl.CreateImage("windowBackground3.jpg");
            ImageBrush imageBackground3 = new ImageBrush();
            imageBackground3.ImageSource = windowBackground3;
            SystemInfoPanel.Background = imageBackground3;

            //canvas background
            BitmapImage backgroundImage = DataControl.CreateImage("spaceCanvasBackground.jpg");
            ImageBrush image = new ImageBrush();
            image.ImageSource = backgroundImage;
            MainCanvas.Background = image;

            _mt = new MatrixTransform();
            MainCanvas.RenderTransform = _mt;

            _dataControl.LoadFromFile(SystemList);
        }

        //draws planet orbits
        private void SystemOrbitCreation(int count)
        {
            var radiuses = _dataControl.GetOrbitRadiuses();

            for (int i = 0; i < count; i++)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.RenderTransform = new TranslateTransform(MainCanvas.ActualWidth / 2 - radiuses[i],
                                                                MainCanvas.ActualHeight / 2 - radiuses[i]);
                ellipse.Width = radiuses[i] * 2;
                ellipse.Height = radiuses[i] * 2;
                ellipse.Stroke = Brushes.White;
                ellipse.StrokeThickness = 2;
                ellipse.MouseLeftButtonDown += EllipseClick;
                ellipse.MouseMove += EllipseSelected;

                _orbits.Add(ellipse);
                MainCanvas.Children.Add(ellipse);
            }
        }

        //an event for orbit selection
        private void EllipseSelected(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < _orbits.Count; i++)
            {
                if (_orbits[i].IsMouseOver == true)
                {
                    _orbits[i].StrokeThickness = 5;
                    _orbits[i].Stroke = Brushes.DeepSkyBlue;
                }
                else
                {
                    _orbits[i].StrokeThickness = 2;
                    _orbits[i].Stroke = Brushes.White;
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

            var selectedOrbitIndex = _orbits.FindIndex(o => o.IsMouseOver == true);

            PlanetInfoTextChange(_dataControl.SelectPlanet(selectedOrbitIndex));
            _orbitIndex = selectedOrbitIndex;

            if (_solSystemLoaded)
            {
                EditPlanetButton.Visibility = Visibility.Hidden;
                DeletePlanetButton.Visibility = Visibility.Hidden;
                LinkTextBlock.Visibility = Visibility.Visible;
                LinkName.Text = "More Info about " + _dataControl.SelectPlanet(_orbitIndex).Name;
            }
        }

        //changes the textblocks to the selected planet's info
        private void PlanetInfoTextChange(CelestialObject planet)
        {
            PlanetInfo.Text = planet.Name;
            PlanetAtmosphere.Text = "Atmosphere: " + ((Planet)planet).Atmosphere;
            PlanetOrbitalPeriod.Text = "Orbital Period: " + ((Planet)planet).OrbitalPeriod;
            PlanetRotationPeriod.Text = "Rotation Period: " + ((Planet)planet).RotationPeriod;
            PlanetMoonCount.Text = "Moon Count: " + ((Planet)planet).MoonCount.ToString();
            PlanetLife.Text = "Life: " + ((Planet)planet).Life;

            SelectedPlanetImage.Source = DataControl.CreateImage(planet.Image.ImageSource.ToString());
        }

        //changes the visibility of window elements
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

        //stop/play the animation
        private bool IsOrbitOn = true;
        private void StopAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOrbitOn)
            {
                CompositionTarget.Rendering -= _dataControl.AnimationUpdate;
                _dataControl.Timer.Stop();
                IsOrbitOn = false;
                StopAnimationButton.Content = "Resume Animation";
            }
            else
            {
                CompositionTarget.Rendering += _dataControl.AnimationUpdate;
                _dataControl.Timer.Start();
                IsOrbitOn = true;
                StopAnimationButton.Content = "Pause Animation";
            }
        }

        private void ChangeBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundChangeWindow backgroundChangeWindow = new BackgroundChangeWindow();
            backgroundChangeWindow.ShowDialog();
        }

        //loads a system from the combobox list of systems
        private void LoadSystem_Click(object sender, RoutedEventArgs e)
        {
            if (SystemList.Items.Count == 0 || SystemList.SelectedItem == null)
            {
                return;
            }

            CompositionTarget.Rendering -= _dataControl.AnimationUpdate;
            _dataControl.Timer.Stop();

            if (_dataControl.SystemObjectsCount() > 0 || _orbits.Count > 0)
            {
                _dataControl.ClearSystemObjects();
                MainCanvas.Children.Clear();
                _orbits.Clear();
            }

            //changes the visibility of needed elements for display
            VisibilityChange();
            SystemStat1Card.Visibility = Visibility.Visible;
            SystemStat2Card.Visibility = Visibility.Visible;
            SystemStat3Card.Visibility = Visibility.Visible;
            DeleteSystemButton.Visibility = Visibility.Visible;
            EditSystemButton.Visibility= Visibility.Visible;
            LinkTextBlock.Visibility = Visibility.Hidden;
            StopAnimationButton.Visibility = Visibility.Visible;
            StopAnimationButton.Content = "Pause Animation";

            LoadedSystemName.Text = ((SolarSystem)SystemList.SelectedItem).SystemName;
            SystemImage.Source = DataControl.CreateImage("starSystem.png");

            IsOrbitOn = true;
            _solSystemLoaded = false;

            //comets
            Random randomNumber = new Random();
            Comet comet = new Comet("Comet", MainCanvas, _cometImage, randomNumber.Next((int)MainCanvas.ActualWidth),
                randomNumber.Next((int)MainCanvas.ActualHeight), true, 20, 20);

            foreach (CelestialObject obj in ((SolarSystem)SystemList.Items[_systemIndex]).SystemPlanets)
            {
                _dataControl.AddObject(obj);
            }
            _dataControl.AddObject(comet);

            SystemOrbitCreation(_dataControl.PlanetCount());

            _dataControl.FillCanvas(MainCanvas);

            //system info 
            SystemName.Text = ((SolarSystem)SystemList.Items[_systemIndex]).SystemName;
            SystemPlanetCount.Text = "System Planet Count: " + _dataControl.PlanetCount().ToString();
            SystemMoonCount.Text = "System Moon Count: " + _dataControl.GetSystemMoonCount().ToString();
            SystemDescription.Text = ((SolarSystem)SystemList.Items[_systemIndex]).Description;

            _dataControl.Timer.Start();
            CompositionTarget.Rendering += _dataControl.AnimationUpdate;
        }

        //loads the Sol System
        private void SolSystemButton_Click(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= _dataControl.AnimationUpdate;
            _dataControl.Timer.Stop();

            if (_dataControl.SystemObjectsCount() > 0 || _orbits.Count > 0)
            {
                _dataControl.ClearSystemObjects();
                MainCanvas.Children.Clear();
                _orbits.Clear();
            }

            VisibilityChange();
            StopAnimationButton.Visibility = Visibility.Visible;
            SystemStat1Card.Visibility = Visibility.Visible;
            SystemStat2Card.Visibility = Visibility.Visible;
            SystemStat3Card.Visibility = Visibility.Visible;  
            StopAnimationButton.Content = "Pause Animation";

            SystemName.Text = "Sol System";
            LoadedSystemName.Text = "Sol System";
            SystemDescription.Text = "The planetary system we call home is located in an outer spiral arm of the Milky Way galaxy." + "\n\n" +
            "Our solar system consists of our star, the Sun, and everything bound to it by gravity – the planets Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, and Neptune;"
            + " " + "dwarf planets such as Pluto; dozens of moons; and millions of asteroids, comets, and meteoroids.";

            SystemImage.Source = DataControl.CreateImage("starSystem.png");

            _solSystemLoaded = true;
            IsOrbitOn = true;

            Random randomNumber = new Random();
            Comet comet = new Comet("Comet", MainCanvas, _cometImage, randomNumber.Next((int)MainCanvas.ActualWidth),
                randomNumber.Next((int)MainCanvas.ActualHeight), true, 20, 20);

            Star sun = new Star("Sun", _sunImage, MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2, 80, 80);
            Planet mercury = new Planet("Mercury", "Not Breathable", "88 Days", "58 Days",
                                        0, "Uninhabited", _mercuryImage, 10, 10, true, 10, 10, sun, 50, -4);
            Planet venus = new Planet("Venus", "Not Breathable", "225 Days", "116 Days",
                                        0, "Uninhabited", _venusImage, 10, 10, true, 10, 10, sun, 100, -3);
            Planet earth = new Planet("Earth", "Breathable", "365 Days", "1 Day / 24 Hours",
                                        1, "Inhabited", _earthImage, 10, 10, true, 20, 20, sun, 150, -2);
            Moon moon = new Moon("Luna", _moonImage, 10, 10, true, 5, 5, earth, 23, -4);
            Planet mars = new Planet("Mars", "Not Breathable", "687 Days", "1 Day / 24 Hours",
                                        2, "Uninhabited", _marsImage, 10, 10, true, 20, 20, sun, 200, -1.5);
            Planet jupiter = new Planet("Jupiter", "Not Breathable", "12 Years", "10 Hours",
                                        79, "Uninhabited", _jupiterImage, 10, 10, true, 30, 30, sun, 280, -1);
            Planet saturn = new Planet("Saturn", "Not Breathable", "29 Years", "11 Hours",
                                        82, "Uninhabited", _saturnImage, 10, 10, true, 40, 30, sun, 330, -0.75);
            Planet uranus = new Planet("Uranus", "Not Breathable", "84 Years", "17 Hours",
                                        27, "Uninhabited", _uranusImage, 10, 10, true, 30, 30, sun, 370, -0.5);
            Planet neptune = new Planet("Neptune", "Not Breathable", "165 Years", "16 Hours",
                                        14, "Uninhabited", _neptuneImage, 10, 10, true, 40, 30, sun, 400, -0.25);

            _dataControl.AddObject(mercury);
            _dataControl.AddObject(venus);
            _dataControl.AddObject(earth);
            _dataControl.AddObject(mars);
            _dataControl.AddObject(jupiter);
            _dataControl.AddObject(saturn);
            _dataControl.AddObject(uranus);
            _dataControl.AddObject(neptune);
            _dataControl.AddObject(sun);
            _dataControl.AddObject(moon);
            _dataControl.AddObject(comet);

            SystemOrbitCreation(8);

            _dataControl.FillCanvas(MainCanvas);

            SystemPlanetCount.Text = "System Planet Count: " + _dataControl.PlanetCount().ToString();
            SystemMoonCount.Text = "System Moon Count: " + _dataControl.GetSystemMoonCount().ToString();

            _dataControl.Timer.Start();
            CompositionTarget.Rendering += _dataControl.AnimationUpdate;
        }

        //opens a new window for planet editing
        private void EditPlanetButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPlanet = _dataControl.SelectPlanet(_orbitIndex);
            int selectedPlanetIndex = _dataControl.GetPlanetIndex(selectedPlanet);

            PlanetEditWindow planetEdit = new PlanetEditWindow(selectedPlanet);
            planetEdit.ShowDialog();
            
            if(planetEdit.DialogResult == true)
            {
                _dataControl.UpdatePlanet(selectedPlanet, selectedPlanetIndex);
                _dataControl.RemoveMoons(selectedPlanet, SystemList, _systemIndex);
                _dataControl.AddMoons(selectedPlanet, SystemList, _systemIndex, selectedPlanetIndex);

                PlanetInfoTextChange(selectedPlanet);
                LoadSystem_Click(sender, e);
            }
        }

        //deletes the selected planet
        private void DeletePlanetButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this planet?",
                    "Deleting Planet", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var planetToRemove = _dataControl.SelectPlanet(_orbitIndex);

                //removes the planet
                _dataControl.RemoveObject(planetToRemove);
                ((SolarSystem)SystemList.Items[_systemIndex]).SystemPlanets.Remove(planetToRemove);
                MainCanvas.Children.Remove(planetToRemove.Shape);

                //removes the deleted planet's moons
                var moonsToRemoveFromCanvas = ((SolarSystem)SystemList.Items[_systemIndex]).SystemPlanets
                  .Where(o => o is Moon)
                      .Where(m => ((Moon)m).GravityCenter == planetToRemove)
                      .ToList();

                for (int i = 0; i < moonsToRemoveFromCanvas.Count; i++)
                {
                    MainCanvas.Children.Remove(moonsToRemoveFromCanvas[i].Shape);
                }

                _dataControl.RemoveMoons(planetToRemove, SystemList, _systemIndex);

                //removes the deleted planet's orbit
                MainCanvas.Children
                    .Remove(_orbits
                            .Where(o => o.Width / 2 == ((Planet)planetToRemove).Radius)
                            .SingleOrDefault());

                _orbits
                    .Remove(_orbits
                            .Where(o => o.Width / 2 == ((Planet)planetToRemove).Radius)
                            .SingleOrDefault());

                SystemPlanetCount.Text = "System Planet Count: " + _dataControl.PlanetCount().ToString();
                SystemMoonCount.Text = "System Moon Count: " + _dataControl.GetSystemMoonCount().ToString();

                VisibilityChange();
                DeleteSystemButton.Visibility = Visibility.Visible;
                EditSystemButton.Visibility = Visibility.Visible;
            }
        }

        //opens a new window for system creation
        private void CreateSystemButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCreationWindow systemCreationWindow = new SystemCreationWindow();
            systemCreationWindow.ShowDialog();

            if(systemCreationWindow.DialogResult == true)
            {
                SystemList.SelectedItem = (SolarSystem)SystemList.Items[_systemIndex];
            }
        }

        //button to edit selected system
        private void EditSystemButton_Click(object sender, RoutedEventArgs e)
        {
            EditSystemWindow editSystemWindow = new EditSystemWindow((SolarSystem)SystemList.Items[_systemIndex], _dataControl);
            editSystemWindow.ShowDialog();

            if(editSystemWindow.DialogResult == true)
            {
                SystemList.SelectedValuePath = ((SolarSystem)SystemList.Items[_systemIndex]).SystemName;
                SystemList.SelectedItem = (SolarSystem)SystemList.Items[_systemIndex];
                SystemList.Items.Refresh();

                LoadSystem_Click(sender, e);
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
                    _systemIndex = SystemList.SelectedIndex;
                    SystemList.Items.RemoveAt(_systemIndex);
                    MainCanvas.Children.Clear();
                    SystemList.Items.Refresh();
                    SystemList.Text = "Systems";

                    SystemName.Text = String.Empty; 
                    LoadedSystemName.Text = String.Empty;
                    SystemStat1Card.Visibility = Visibility.Hidden;
                    SystemStat2Card.Visibility = Visibility.Hidden;
                    SystemStat3Card.Visibility = Visibility.Hidden;

                    VisibilityChange();
                }
            }
        }

        //changes the selected system
        private void SystemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SystemList.SelectedItem != null)
            {
                _systemIndex = SystemList.SelectedIndex;
                SystemName.Text = ((SolarSystem)SystemList.SelectedItem).SystemName;
            }
        }

        //opens wikipedia with e link
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (!_solSystemLoaded)
            {
                e.Handled = false;
                return;
            }

            string link = "https://en.wikipedia.org/wiki/";
            link += _dataControl.SelectPlanet(_orbitIndex).Name + "_(planet)";
            Process.Start(new ProcessStartInfo(link));
            e.Handled = true;
        }

        //canvas panning 
        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isCanvasDragging)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed && MainCanvas.IsMouseCaptured)
            {
                Point mousePos = e.GetPosition(MainCanvas);
                var matrix = _mt.Matrix;
                matrix.Translate(mousePos.X - _lastMousePos.X, mousePos.Y - _lastMousePos.Y);
                _mt.Matrix = matrix;
                _lastMousePos = mousePos;
            }
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainCanvas.CaptureMouse();
            _lastMousePos = e.GetPosition(MainCanvas);
            _isCanvasDragging = true;
        }

        private void MainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainCanvas.ReleaseMouseCapture();
            _isCanvasDragging = false;
        }

        //canvas zooming
        private void MainCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((_mt.Value.OffsetX <= 0 || _mt.Value.OffsetY <= 0) && e.Delta > 0)
            {
                var matrix = _mt.Matrix;
                var mousePosition = e.GetPosition(MainCanvas);
                var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;
                matrix.ScaleAt(scale, scale, mousePosition.X, mousePosition.Y);
                _mt.Matrix = matrix;
                e.Handled = true;
            }
            else if ((_mt.Value.OffsetX < 0 || _mt.Value.OffsetY < 0) && e.Delta < 0)
            {
                var matrix = _mt.Matrix;
                var mousePosition = e.GetPosition(MainCanvas);
                var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;
                matrix.ScaleAt(scale, scale, mousePosition.X, mousePosition.Y);
                if (matrix.OffsetX > 0 || matrix.OffsetY > 0)
                {
                    matrix.SetIdentity();
                    _mt.Matrix = matrix;
                    e.Handled = true;
                    return;
                }
                _mt.Matrix = matrix;
                e.Handled = true;
            }
        }

        //window drag
        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //closes the program
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _dataControl.SaveToFile(SystemList);
            MainCanvas.Children.Clear();
            _dataControl.ClearSystemObjects();
            _dataControl.ImageDelete();
            this.Close();
        }
    }
}