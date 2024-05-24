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
