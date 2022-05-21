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
        private DataControl control = new DataControl();
        private List<CelestialObject> onlyPlanets;
        private SolarSystem editedSystem = new SolarSystem();

        private BitmapImage addImage = DataControl.CreateImage("../../Images/add.png");
        private BitmapImage addImage2 = DataControl.CreateImage("../../Images/add2.png");

        private List<Image> images = new List<Image>();
        private List<TextBlock> textBlocks = new List<TextBlock>();

        public EditSystemWindow(SolarSystem solarSystem, DataControl dataControl)
        {
            InitializeComponent();

            control = dataControl;

            SystemName.Text = solarSystem.SystemName;
            SystemDescriptionEdit.Text = solarSystem.Description;

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

            onlyPlanets = control.GetOnlyPlanets(solarSystem);

            int position = 0;
            for (int i = 0; i < onlyPlanets.Count; i++)
            {
                switch (((Planet)onlyPlanets[i]).Radius)
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
                images[position].Source = DataControl.CreateImage(onlyPlanets[i].Image.ImageSource.ToString());
                textBlocks[position].Text = onlyPlanets[i].Name;
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
                    NewPlanetWindow newPlanetWindow = new NewPlanetWindow(editedSystem, i);
                    newPlanetWindow.ShowDialog();

                    if(newPlanetWindow.DialogResult == true)
                    {
                        onlyPlanets = editedSystem.SystemPlanets
                                .Where(p => p is Planet)
                                .ToList();

                        images[imageIndex].Source = newPlanetWindow.NewImage();
                        textBlocks[imageIndex].Text = onlyPlanets[onlyPlanets.Count - 1].Name;
                    }
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
            editedSystem.Description = SystemDescriptionEdit.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
