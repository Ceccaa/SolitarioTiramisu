using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SolitarioTiramisu
{
    public partial class finalPage : Page
    {
        public finalPage(string message)
        {
            InitializeComponent();
            UpdateResult(message);
            SetResultImage(message);
        }

        public void UpdateResult(string message)
        {
            result.Text = message;
        }

        private void SetResultImage(string message)
        {
            try
            {
                Image resultImage = new Image
                {
                    Stretch = Stretch.Uniform,
                    Width = 250,  // Dimensione regolabile
                    Height = 250  // Dimensione regolabile
                };

                string imagePath;
                string messageKey;

                if (message == "HAI VINTO!" || message == "YOU WON!")
                {
                    imagePath = System.IO.Path.GetFullPath("../../../assets/trophy.jpg");
                    messageKey = "WinMessage";
                }
                else
                {
                    imagePath = System.IO.Path.GetFullPath("../../../assets/gameOver.jpg");
                    messageKey = "LoseMessage";
                }

                resultImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

                // Posiziona l'immagine nel centro del contenitore
                resultImage.Loaded += (s, e) =>
                {
                    Canvas.SetLeft(resultImage, (Sfondo.ActualWidth - resultImage.Width) / 2);
                    Canvas.SetTop(resultImage, 0);  // Vicino al bordo superiore
                };

                // Aggiungi l'immagine al contenitore Sfondo
                Sfondo.Children.Add(resultImage);

                // Imposta il messaggio di risultato
                if (Application.Current.Resources.Contains(messageKey))
                {
                    result.Text = (string)Application.Current.Resources[messageKey];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }


        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            // Naviga usando la finestra principale
            Window mainWindow = Application.Current.MainWindow;
            Frame mainFrame = (Frame)mainWindow.FindName("MainFrame");
            mainFrame.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }
    }
}
