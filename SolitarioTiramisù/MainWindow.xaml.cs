using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace SolitarioTiramisu
{
    public partial class MainWindow : Window
    {
        public bool IsMusicEnabled { get; private set; } = true;

        public MainWindow()
        {
            InitializeComponent();

            //permette di avere la finestra sempre grande
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

            //prendo il percorso della musica di sottofondo
            string relativePath = "soundboard/background.mp3";
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = Path.Combine(baseDirectory, relativePath);

            //se il percorso non esiste lancio un'eccezione
            if (File.Exists(absolutePath))
            {
                try
                {
                    //faccio partire la musica
                    BackgroundMusic.Source = new Uri(absolutePath, UriKind.Absolute);
                    BackgroundMusic.MediaEnded += BackgroundMusic_MediaEnded; // Riproduci in loop
                    BackgroundMusic.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error setting music source: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"File not found: {absolutePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region musica di sottofondo
        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
        }
        #endregion


        public void DisableMusic()
        {
            BackgroundMusic.Stop();
            IsMusicEnabled = false;
        }

        public void EnableMusic()
        {
            BackgroundMusic.Play();
            IsMusicEnabled = true;
        }

        #region funzione cambio lingua
        public void ChangeLanguage(string culture)
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (culture)
            {
                //risorsa in italiano
                case "it-IT":
                    dict.Source = new Uri("Resources/Strings.it.xaml", UriKind.Relative);
                    break;

                //risorsa in inglese
                case "en-US":
                default:
                    dict.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
            }

            // Trova e rimuovi il dizionario corrente
            ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                          where d.Source != null && d.Source.OriginalString.StartsWith("Resources/Strings.")
                                          select d).FirstOrDefault();

            if (oldDict != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
            }

            // Aggiungi il nuovo dizionario
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
        #endregion
    }
}
