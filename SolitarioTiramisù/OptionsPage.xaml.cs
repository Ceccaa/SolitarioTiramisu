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
                btnMusic.Content = mainWindow.IsMusicEnabled
                    ? Application.Current.Resources["BtnMusicOff"]
                    : Application.Current.Resources["BtnMusicOn"];
                btnLanguage.Content = Application.Current.Resources["LanguageButton"];
                // Imposta il contenuto del pulsante "MENU'"
                btnMenu.Content = Application.Current.Resources["MenuButton"];
            }
        }

        private void DisableMusic_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                if (mainWindow.IsMusicEnabled)
                {
                    mainWindow.DisableMusic();
                    btnMusic.Content = Application.Current.Resources["BtnMusicOn"];
                }
                else
                {
                    mainWindow.EnableMusic();
                    btnMusic.Content = Application.Current.Resources["BtnMusicOff"];
                }
            }
        }

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                if (btnLanguage.Content.ToString() == "ITALIANO")
                {
                    mainWindow.ChangeLanguage("en-US");
                }
                else
                {
                    mainWindow.ChangeLanguage("it-IT");
                }

                // Aggiorna il contenuto del pulsante dopo il cambio di lingua
                btnLanguage.Content = Application.Current.Resources["LanguageButton"];
                btnMusic.Content = mainWindow.IsMusicEnabled
                    ? Application.Current.Resources["BtnMusicOff"]
                    : Application.Current.Resources["BtnMusicOn"];
                // Aggiorna il contenuto del pulsante "MENU'"
                btnMenu.Content = Application.Current.Resources["MenuButton"];
            }
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }
    }
}
