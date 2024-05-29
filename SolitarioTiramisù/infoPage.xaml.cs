using System;
using System.Windows;
using System.Windows.Controls;

namespace SolitarioTiramisu
{
    public partial class infoPage : Page
    {
        public infoPage()
        {
            InitializeComponent();
        }


        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }
    }
}
