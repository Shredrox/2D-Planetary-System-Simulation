using System;
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
    public partial class BackgroundChangeWindow : Window
    {
        private List<MaterialDesignThemes.Wpf.Card> materialDesignCardList = new List<MaterialDesignThemes.Wpf.Card>();
        private List<Image> imageList = new List<Image>();
        private Image selectedImage = new Image();

        public BackgroundChangeWindow()
        {
            InitializeComponent();

            materialDesignCardList.Add(Background1Card);
            materialDesignCardList.Add(Background2Card);
            materialDesignCardList.Add(Background3Card);
            materialDesignCardList.Add(Background4Card);
            materialDesignCardList.Add(Background5Card);

            imageList.Add(Background1);
            imageList.Add(Background2);
            imageList.Add(Background3);
            imageList.Add(Background4);
            imageList.Add(Background5);
            
            BitmapImage backgroundImage = new BitmapImage();
            backgroundImage.BeginInit();
            backgroundImage.CacheOption = BitmapCacheOption.OnLoad;
            backgroundImage.UriSource = new Uri("../../Images/spaceCanvasBackground.jpg", UriKind.RelativeOrAbsolute);
            backgroundImage.EndInit();
            backgroundImage.Freeze();
            Background1.Source = backgroundImage;
            Background1.MouseDown += ImageSelect;

            BitmapImage background2Image = new BitmapImage();
            background2Image.BeginInit();
            background2Image.CacheOption = BitmapCacheOption.OnLoad;
            background2Image.UriSource = new Uri("../../Images/spaceCanvasBackground2.jpg", UriKind.RelativeOrAbsolute);
            background2Image.EndInit();
            background2Image.Freeze();
            Background2.Source = background2Image;
            Background2.MouseDown += ImageSelect;

            BitmapImage background3Image = new BitmapImage();
            background3Image.BeginInit();
            background3Image.CacheOption = BitmapCacheOption.OnLoad;
            background3Image.UriSource = new Uri("../../Images/spaceCanvasBackground3.jpg", UriKind.RelativeOrAbsolute);
            background3Image.EndInit();
            background3Image.Freeze();
            Background3.Source = background3Image;
            Background3.MouseDown += ImageSelect;

            BitmapImage background4Image = new BitmapImage();
            background4Image.BeginInit();
            background4Image.CacheOption = BitmapCacheOption.OnLoad;
            background4Image.UriSource = new Uri("../../Images/spaceCanvasBackground4.jpg", UriKind.RelativeOrAbsolute);
            background4Image.EndInit();
            background4Image.Freeze();
            Background4.Source = background4Image;
            Background4.MouseDown += ImageSelect;

            BitmapImage background5Image = new BitmapImage();
            background5Image.BeginInit();
            background5Image.CacheOption = BitmapCacheOption.OnLoad;
            background5Image.UriSource = new Uri("../../Images/spaceCanvasBackground5.jpg", UriKind.RelativeOrAbsolute);
            background5Image.EndInit();
            background5Image.Freeze();
            Background5.Source = background5Image;
            Background5.MouseDown += ImageSelect;
        }

        private void ImageSelect(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < imageList.Count; i++)
            {
                if(imageList[i].IsMouseOver == true)
                {
                    materialDesignCardList[i].Background = Brushes.Cyan;
                    selectedImage = imageList[i];
                }
                else
                {
                    materialDesignCardList[i].Background = Brush1;
                }
            }
        }

        private void ApplyChangesButton_Click(object sender, RoutedEventArgs e)
        {
            ImageBrush canvasBackground = new ImageBrush();
            canvasBackground.ImageSource = selectedImage.Source;
            ((MainWindow)Application.Current.MainWindow).MainCanvas.Background = canvasBackground;

            this.Close();
        }
    }
}
