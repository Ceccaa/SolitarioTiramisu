using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SolitarioTiramisu
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            SetLogoImage();
            SetButtonContent();
            Loaded += MainPage_Loaded;
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                if (btnLanguage.Content.ToString() == "IT")
                {
                    mainWindow.ChangeLanguage("en-US");
                }
                else
                {
                    mainWindow.ChangeLanguage("it-IT");
                }

                // Aggiorna il contenuto del pulsante dopo il cambio di lingua
                btnLanguage.Content = Application.Current.Resources["LanguageButton"];
                // Aggiorna il contenuto di tutti i pulsanti
                SetButtonContent();
            }
        }

        private void SetLogoImage()
        {
            try
            {
                Image logo = new Image
                {
                    Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("../../../images/Logo.png"), UriKind.Absolute)),
                    Stretch = Stretch.Uniform,
                    Width = 980,  // Dimensione regolabile
                    Height = 620  // Dimensione regolabile
                };

                // Posiziona il logo molto vicino al bordo superiore
                Canvas.SetLeft(logo, (LogoContainer.ActualWidth - logo.Width) / 2);
                Canvas.SetTop(logo, 0);  // Molto vicino al bordo superiore
                LogoContainer.Children.Add(logo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        private void SetButtonContent()
        {
            btnStart.Content = Application.Current.Resources["StartGameButton"];
            btnRules.Content = Application.Current.Resources["RulesButton"];
            btnClose.Content = Application.Current.Resources["CloseButton"];
            btnOptions.Content = Application.Current.Resources["OptionButton"];
            btnLanguage.Content = Application.Current.Resources["LanguageButton"];
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                UpdateMusicButtonImage(mainWindow.IsMusicEnabled);
            }
        }

        private void DisableMusic_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                if (mainWindow.IsMusicEnabled)
                {
                    mainWindow.DisableMusic();
                    UpdateMusicButtonImage(false);
                }
                else
                {
                    mainWindow.EnableMusic();
                    UpdateMusicButtonImage(true);
                }
            }
        }

        private void UpdateMusicButtonImage(bool isMusicEnabled)
        {
            var musicImage = btnMusic.Template.FindName("MusicImage", btnMusic) as Image;
            if (musicImage != null)
            {
                string imagePath = isMusicEnabled ? "../../../images/musicOn.jpg" : "../../../images/musicOff.jpg";
                musicImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(imagePath), UriKind.Absolute));
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("GamePage.xaml", UriKind.Relative));
        }

        private void RulesButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("RulesPage.xaml", UriKind.Relative));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("OptionsPage.xaml", UriKind.Relative));
        }
    }
}
