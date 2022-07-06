using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CelestialObjectsLibrary;

namespace PlanetarySystem
{
    public partial class EditSystemWindow : Window
    {
        private DataControl _control = new DataControl();
        private List<CelestialObject> _onlyPlanets;
        private SolarSystem _editedSystem = new SolarSystem();

        private readonly BitmapImage _addImage = DataControl.CreateImage("add.png");
        private readonly BitmapImage _addImage2 = DataControl.CreateImage("add2.png");

        private List<Image> _images = new List<Image>();
        private List<TextBlock> _textBlocks = new List<TextBlock>();

        public EditSystemWindow(SolarSystem solarSystem, DataControl dataControl)
        {
            InitializeComponent();

            _control = dataControl;

            SystemName.Text = solarSystem.SystemName;
            SystemDescriptionEdit.Text = solarSystem.Description;

            _images.Add(Planet1Image);
            _images.Add(Planet2Image);
            _images.Add(Planet3Image);
            _images.Add(Planet4Image);
            _images.Add(Planet5Image);
            _images.Add(Planet6Image);
            _images.Add(Planet7Image);
            _images.Add(Planet8Image);
            _textBlocks.Add(Planet1Name);
            _textBlocks.Add(Planet2Name);
            _textBlocks.Add(Planet3Name);
            _textBlocks.Add(Planet4Name);
            _textBlocks.Add(Planet5Name);
            _textBlocks.Add(Planet6Name);
            _textBlocks.Add(Planet7Name);
            _textBlocks.Add(Planet8Name);

            for (int i = 0; i < _images.Count; i++)
            {
                _images[i].Source = _addImage;
                _images[i].MouseLeftButtonDown += ImageClick;
                _images[i].MouseMove += ImageSelected;
            }

            _onlyPlanets = _control.GetOnlyPlanets(solarSystem);

            int position = 0;
            for (int i = 0; i < _onlyPlanets.Count; i++)
            {
                switch (((Planet)_onlyPlanets[i]).Radius)
                {
                    case 50: position = 0; break;
                    case 100: position = 1; break;
                    case 150: position = 2; break;
                    case 200: position = 3; break;
                    case 280: position = 4; break;
                    case 330: position = 5; break;
                    case 370: position = 6; break;
                    case 400: position = 7; break;
                }
                _images[position].Source = DataControl.CreateImage(_onlyPlanets[i].Image.ImageSource.ToString());
                _textBlocks[position].Text = _onlyPlanets[i].Name;
            }

            _editedSystem = solarSystem;
        }

        private void ImageClick(object s, MouseEventArgs e)
        {
            for (int i = 0; i < _images.Count; i++)
            {
                if (_images[i].IsMouseOver == true && _images[i].Source == _addImage2)
                {
                    NewPlanetWindow newPlanetWindow = new NewPlanetWindow(_editedSystem, i);
                    newPlanetWindow.ShowDialog();

                    if(newPlanetWindow.DialogResult == true)
                    {
                        _onlyPlanets = _editedSystem.SystemPlanets
                                .Where(p => p is Planet)
                                .ToList();

                        _images[i].Source = newPlanetWindow.NewImage();
                        _textBlocks[i].Text = _onlyPlanets[_onlyPlanets.Count - 1].Name;
                    }
                }
            }
        }

        private void ImageSelected(object s , MouseEventArgs e)
        {
            for (int i = 0; i < _images.Count; i++) 
            {
                if (_images[i].IsMouseOver == true && _images[i].Source == _addImage)
                {
                    _images[i].Source = _addImage2;
                }
                else if(_images[i].IsMouseOver == false && _images[i].Source == _addImage2)
                {
                    _images[i].Source = _addImage;
                }
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            _editedSystem.SystemName = SystemName.Text;
            _editedSystem.Description = SystemDescriptionEdit.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
