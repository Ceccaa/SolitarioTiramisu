using System;
using System.Windows;
using System.Windows.Controls;

namespace SolitarioTiramisu
{
    public partial class RulesPage : Page
    {
        public RulesPage()
        {
            InitializeComponent();
        }

        #region ti permette di andare alla MainPage.xaml.cs
        private void GoToGameButton_Click(object sender, RoutedEventArgs e)
        {
           
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }
        #endregion
    }
}

