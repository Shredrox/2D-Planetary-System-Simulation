using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CelestialObjectsLibrary;

namespace PlanetarySystem
{
    public partial class BackgroundChangeWindow : Window
    {
        private Image _selectedImage = new Image();

        public BackgroundChangeWindow()
        {
            InitializeComponent();

            Background1.Source = DataControl.CreateImage("spaceCanvasBackground.jpg");
            Background1.MouseDown += (s, e) =>
            {
                Background1Card.Background = Brushes.Cyan;
                Background2Card.Background = Brush1;
                Background3Card.Background = Brush1;
                Background4Card.Background = Brush1;
                Background5Card.Background = Brush1;
                _selectedImage = Background1;
            };

            Background2.Source = DataControl.CreateImage("spaceCanvasBackground2.jpg");
            Background2.MouseDown += (s, e) =>
            {
                Background1Card.Background = Brush1;
                Background2Card.Background = Brushes.Cyan;
                Background3Card.Background = Brush1;
                Background4Card.Background = Brush1;
                Background5Card.Background = Brush1;    
                _selectedImage = Background2;
            };

            Background3.Source = DataControl.CreateImage("spaceCanvasBackground3.jpg");
            Background3.MouseDown += (s, e) =>
            {
                Background1Card.Background = Brush1;
                Background2Card.Background = Brush1;
                Background3Card.Background = Brushes.Cyan;
                Background4Card.Background = Brush1;
                Background5Card.Background = Brush1;
                _selectedImage = Background3;
            };

            Background4.Source = DataControl.CreateImage("spaceCanvasBackground4.jpg");
            Background4.MouseDown += (s, e) =>
            {
                Background1Card.Background = Brush1;
                Background2Card.Background = Brush1;
                Background3Card.Background = Brush1;
                Background4Card.Background = Brushes.Cyan;
                Background5Card.Background = Brush1;
                _selectedImage = Background4;
            };

            Background5.Source = DataControl.CreateImage("spaceCanvasBackground5.jpg");
            Background5.MouseDown += (s, e) =>
            {
                Background1Card.Background = Brush1;
                Background2Card.Background = Brush1;
                Background3Card.Background = Brush1;
                Background4Card.Background = Brush1;
                Background5Card.Background = Brushes.Cyan;
                _selectedImage = Background5;
            };
        }

        private void ApplyChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedImage.Source == null)
            {
                MessageBox.Show("Please select background.");
                return;
            }

            ImageBrush canvasBackground = new ImageBrush();
            canvasBackground.ImageSource = _selectedImage.Source;
            ((MainWindow)Application.Current.MainWindow).MainCanvas.Background = canvasBackground;

            DialogResult = true;
        }
    }
}
