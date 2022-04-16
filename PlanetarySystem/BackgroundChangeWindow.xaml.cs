using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlanetarySystem
{
    public partial class BackgroundChangeWindow : Window
    {
        private Image selectedImage = new Image();

        public BackgroundChangeWindow()
        {
            InitializeComponent();

            Background1.Source = ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/spaceCanvasBackground.jpg");
            Background1.MouseDown += delegate
            {
                Background1Card.Background = Brushes.Cyan;
                Background2Card.Background = Brush1;
                Background3Card.Background = Brush1;
                Background4Card.Background = Brush1;
                Background5Card.Background = Brush1;
                selectedImage = Background1;
            };

            Background2.Source = ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/spaceCanvasBackground2.jpg");
            Background2.MouseDown += delegate
            {
                Background1Card.Background = Brush1;
                Background2Card.Background = Brushes.Cyan;
                Background3Card.Background = Brush1;
                Background4Card.Background = Brush1;
                Background5Card.Background = Brush1;    
                selectedImage = Background2;
            };

            Background3.Source = ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/spaceCanvasBackground3.jpg");
            Background3.MouseDown += delegate
            {
                Background1Card.Background = Brush1;
                Background2Card.Background = Brush1;
                Background3Card.Background = Brushes.Cyan;
                Background4Card.Background = Brush1;
                Background5Card.Background = Brush1;
                selectedImage = Background3;
            };

            Background4.Source = ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/spaceCanvasBackground4.jpg");
            Background4.MouseDown += delegate
            {
                Background1Card.Background = Brush1;
                Background2Card.Background = Brush1;
                Background3Card.Background = Brush1;
                Background4Card.Background = Brushes.Cyan;
                Background5Card.Background = Brush1;
                selectedImage = Background4;
            };

            Background5.Source = ((MainWindow)Application.Current.MainWindow).CreateImage("../../Images/spaceCanvasBackground5.jpg");
            Background5.MouseDown += delegate
            {
                Background1Card.Background = Brush1;
                Background2Card.Background = Brush1;
                Background3Card.Background = Brush1;
                Background4Card.Background = Brush1;
                Background5Card.Background = Brushes.Cyan;
                selectedImage = Background5;
            };
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
