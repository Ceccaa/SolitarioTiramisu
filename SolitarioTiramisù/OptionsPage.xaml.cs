using System;
using System.Windows;
using System.Windows.Controls;

namespace SolitarioTiramisu
{
    public partial class OptionsPage : Page
    {
        public OptionsPage()
        {
            InitializeComponent();
            Loaded += OptionsPage_Loaded;
        }

        private void OptionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                // Aggiorna il contenuto del pulsante in base allo stato della musica
                btnMusic.Content = mainWindow.IsMusicEnabled ? "DISATTIVA MUSICA" : "ATTIVA MUSICA";
            }
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

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementa il comportamento del pulsante del cambio lingua
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }
    }
}
