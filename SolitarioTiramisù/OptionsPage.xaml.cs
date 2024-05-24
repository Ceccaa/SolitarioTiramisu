using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SolitarioTiramisu
{
    public partial class OptionsPage : Page
    {
        public OptionsPage()
        {
            InitializeComponent();

        }

        private void DisableMusic_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.DisableMusic();
            }

        }

        private void LanguageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void MainButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));

        }
    }
}
