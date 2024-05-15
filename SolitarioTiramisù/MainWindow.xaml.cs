using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolitarioTiramisù
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Point startPoint;

        private void RedRectangle_Move(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                startPoint = e.GetPosition(canvas);
                DragDrop.DoDragDrop(RedRectangle, RedRectangle, DragDropEffects.Move);
            }
        }

        //Set Zindex
        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(canvas);
            Point offset = new Point(dropPosition.X - startPoint.X, dropPosition.Y - startPoint.Y);

            double newLeft = Canvas.GetLeft(RedRectangle) + offset.X;
            double newTop = Canvas.GetTop(RedRectangle) + offset.Y;

            // Imposta la nuova posizione del cubo all'interno dei limiti del canvas
            newLeft = Math.Max(0, Math.Min(newLeft, canvas.ActualWidth - RedRectangle.ActualWidth));
            newTop = Math.Max(0, Math.Min(newTop, canvas.ActualHeight - RedRectangle.ActualHeight));

            Canvas.SetLeft(RedRectangle, newLeft);
            Canvas.SetTop(RedRectangle, newTop);

            startPoint = dropPosition;
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {


        }


        private void deck_Click(object sender, RoutedEventArgs e)
        {
            
            
        }
    }
}