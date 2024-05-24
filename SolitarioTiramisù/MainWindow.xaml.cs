using System.Windows;

namespace SolitarioTiramisu
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

            // Imposta il percorso del file audio
            BackgroundMusic.Source = new Uri("pack://application:,,,/background_music.mp3");
            BackgroundMusic.MediaEnded += BackgroundMusic_MediaEnded; // Riproduci in loop
            BackgroundMusic.Play();
        }

        // Metodo per riprodurre la musica in loop
        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
        }

        public void DisableMusic()
        {
            BackgroundMusic.Stop();
        }

    }
}
