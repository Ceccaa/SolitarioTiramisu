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

        private void DisableMusic_Click(object sender, RoutedEventArgs e)
        {
            // Ottieni il contenuto del pulsante come stringa
            string buttonContent = btnMusic.Content.ToString();

            // Verifica se la finestra corrente è MainWindow
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                if (buttonContent == "ATTIVA MUSICA")
                {
                    // Attiva la musica
                    mainWindow.EnableMusic();

                    // Cambia il contenuto del pulsante in "DISATTIVA"
                    btnMusic.Content = "DISATTIVA MUSICA";
                }
                else if (buttonContent == "DISATTIVA MUSICA")
                {
                    // Disattiva la musica
                    mainWindow.DisableMusic();

                    // Cambia il contenuto del pulsante in "ATTIVA"
                    btnMusic.Content = "ATTIVA MUSICA";
                }
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
