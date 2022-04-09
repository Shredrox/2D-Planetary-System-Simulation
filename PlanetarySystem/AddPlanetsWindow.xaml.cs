﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlanetarySystem
{
    public partial class AddPlanetsWindow : Window
    {                                   
        private ImageSource addImage = new BitmapImage(new Uri(@"C:\Users\User\Desktop\Stuff\add.png", UriKind.RelativeOrAbsolute));
        private ImageSource addImage2 = new BitmapImage(new Uri(@"C:\Users\User\Desktop\Stuff\add2.png", UriKind.RelativeOrAbsolute));
        private List<Image> images = new List<Image>();
        private List<TextBlock> textBlocks = new List<TextBlock>();
        private SolarSystem editedSystem = new SolarSystem();
        private List<CelestialObject> onlyPlanets;

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

            onlyPlanets = solarSystem.SystemPlanets.Where(s => s.GetType() == typeof(Planet)).ToList();

            for (int i = 0; i < onlyPlanets.Count; i++)
            {
                string fullPath;
                if (onlyPlanets[i].Image.ImageSource.ToString().Contains("file://"))
                {
                    fullPath = onlyPlanets[i].Image.ImageSource.ToString();
                }
                else if (onlyPlanets[i].Image.ImageSource.ToString().Contains("/Images"))
                {
                    fullPath = onlyPlanets[i].Image.ImageSource.ToString();
                }
                else
                {
                    fullPath = System.IO.Path.GetFullPath(onlyPlanets[i].Image.ImageSource.ToString());
                }

                images[i].Source = ImageSetter(fullPath);
                textBlocks[i].Text = onlyPlanets[i].Name;
            }

            editedSystem = solarSystem;
        }

        private BitmapImage ImageSetter(string source)
        {
            BitmapImage planetImage = new BitmapImage();
            planetImage.BeginInit();
            planetImage.UriSource = new Uri(source, UriKind.RelativeOrAbsolute);
            planetImage.EndInit();

            return planetImage;
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
                            onlyPlanets = editedSystem.SystemPlanets.Where(p => p.GetType() == typeof(Planet)).ToList();
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
