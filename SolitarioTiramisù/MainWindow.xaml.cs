using System;
using System.IO;
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

            string relativePath = "soundboard/background.mp3";
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = Path.Combine(baseDirectory, relativePath);

            if (File.Exists(absolutePath))
            {
                try
                {
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

        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
        }

        public void DisableMusic()
        {
            BackgroundMusic.Stop();
        }

        public void EnableMusic()
        {
            BackgroundMusic.Play();
        }
    }
}
