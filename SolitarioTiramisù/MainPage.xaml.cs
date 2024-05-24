using SolitarioTiramisu;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            Rectangle Logo = new Rectangle
            {
                Width = 1180,
                Height = 640,
                Fill = new ImageBrush(new BitmapImage(new Uri("../../../images/Logo.png", UriKind.RelativeOrAbsolute)))
            };
            Sfondo.Children.Add(Logo);
            Logo.HorizontalAlignment = HorizontalAlignment.Center;
            Logo.VerticalAlignment = VerticalAlignment.Top;
            
            
            
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
            // Chiudi la finestra principale
            Window.GetWindow(this)?.Close();
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("OptionsPage.xaml", UriKind.Relative));
        }
    }
}
