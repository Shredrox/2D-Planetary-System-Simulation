using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CelestialObjectsLibrary;

namespace PlanetarySystem
{
    public partial class AddPlanetsWindow : Window
    {                                   
        private BitmapImage addImage = DataControl.CreateImage("../../Images/add.png");
        private BitmapImage addImage2 = DataControl.CreateImage("../../Images/add2.png");

        private List<Image> images = new List<Image>();
        private List<TextBlock> textBlocks = new List<TextBlock>();

        private List<CelestialObject> onlyPlanets;
        private SolarSystem editedSystem = new SolarSystem();
        
        public AddPlanetsWindow(SolarSystem solarSystem)
        {
            InitializeComponent();

            SystemName.Text = solarSystem.SystemName;

            images.Add(Planet1Image);
            images.Add(Planet2Image);
            images.Add(Planet3Image);
            images.Add(Planet4Image);
            images.Add(Planet5Image);
            images.Add(Planet6Image);
            images.Add(Planet7Image);
            images.Add(Planet8Image);
            textBlocks.Add(Planet1Name);
            textBlocks.Add(Planet2Name);
            textBlocks.Add(Planet3Name);
            textBlocks.Add(Planet4Name);
            textBlocks.Add(Planet5Name);
            textBlocks.Add(Planet6Name);
            textBlocks.Add(Planet7Name);
            textBlocks.Add(Planet8Name);

            for (int i = 0; i < images.Count; i++)
            {
                images[i].Source = addImage;
                images[i].MouseLeftButtonDown += ImageClick;
                images[i].MouseMove += ImageSelected;
            }

            onlyPlanets = DataControl.GetOnlyPlanets(solarSystem);

            for (int i = 0; i < onlyPlanets.Count; i++)
            {
                images[i].Source = DataControl.CreateImage(System.IO.Path.GetFullPath(onlyPlanets[i].Image.ImageSource.ToString()));
                textBlocks[i].Text = onlyPlanets[i].Name;
            }

            editedSystem = solarSystem;
        }

        private void ImageClick(object s, MouseEventArgs e)
        {
            int imageIndex = 0;
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].IsMouseOver == true && images[i].Source == addImage2)
                {
                    imageIndex = i;
                    NewPlanetWindow newPlanetWindow = new NewPlanetWindow(editedSystem, onlyPlanets);
                    newPlanetWindow.Show();

                    newPlanetWindow.Closed += delegate
                    {
                        if (newPlanetWindow.IsNewPlanet)
                        {
                            onlyPlanets = editedSystem.SystemPlanets
                                .Where(p => p is Planet)
                                .ToList();

                            images[imageIndex].Source = newPlanetWindow.NewImage();
                            textBlocks[imageIndex].Text = onlyPlanets[onlyPlanets.Count - 1].Name;
                        }
                    };
                }
            }
        }

        private void ImageSelected(object s , MouseEventArgs e)
        {
            for (int i = 0; i < images.Count; i++) 
            {
                if (images[i].IsMouseOver == true && images[i].Source == addImage)
                {
                    images[i].Source = addImage2;
                }
                else if(images[i].IsMouseOver == false && images[i].Source == addImage2)
                {
                    images[i].Source = addImage;
                }
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            editedSystem.SystemName = SystemName.Text;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
