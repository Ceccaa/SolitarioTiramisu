using System;
using System.Windows;
using System.Windows.Controls;

namespace SolitarioTiramisu
{
    public partial class finalPage : Page
    {
        public finalPage()
        {
            InitializeComponent();
        }

        public void UpdateResult(string message)
        {
            result.Text = message;
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }
    }
}
