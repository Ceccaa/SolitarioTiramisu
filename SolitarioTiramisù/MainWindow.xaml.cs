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
        private Point startPoint;
        private Rectangle dragRectangle;
        private Table table = new Table();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RedRectangle_Move(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                startPoint = e.GetPosition(canvas);
                DragDrop.DoDragDrop(RedRectangle, RedRectangle, DragDropEffects.Move);
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Rectangle)))
            {
                Rectangle rect = e.Data.GetData(typeof(Rectangle)) as Rectangle;

                Rectangle closestRect = GetClosestRectangle(e.GetPosition(canvas));
                if (closestRect != null)
                {
                    // Remove the rectangle from its current parent (if necessary)
                    if (rect.Parent is Canvas)
                    {
                        ((Canvas)rect.Parent).Children.Remove(rect);
                    }

                    // Add the rectangle to the Canvas if it's not already there
                    if (!canvas.Children.Contains(rect))
                    {
                        canvas.Children.Add(rect);
                    }

                    // Set the new position of the rectangle
                    Canvas.SetLeft(rect, Canvas.GetLeft(closestRect));
                    Canvas.SetTop(rect, Canvas.GetTop(closestRect));
                }

                e.Handled = true;

                // Remove the drag rectangle
                if (dragRectangle != null)
                {
                    canvas.Children.Remove(dragRectangle);
                    dragRectangle = null;
                }
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;

            Point dropPosition = e.GetPosition(canvas);

            // Update the position of the drag rectangle
            if (dragRectangle == null)
            {
                dragRectangle = new Rectangle
                {
                    Width = RedRectangle.Width,
                    Height = RedRectangle.Height,
                    Fill = RedRectangle.Fill,
                    Stroke = new SolidColorBrush(Colors.Gray),
                    StrokeThickness = 2,
                    Opacity = 0.5
                };
                canvas.Children.Add(dragRectangle);
            }

            Canvas.SetLeft(dragRectangle, dropPosition.X - dragRectangle.Width / 2);
            Canvas.SetTop(dragRectangle, dropPosition.Y - dragRectangle.Height / 2);

            e.Handled = true;
        }

        private Rectangle GetClosestRectangle(Point dropPosition)
        {
            Rectangle closestRect = null;
            double closestDistance = double.MaxValue;

            foreach (UIElement child in canvas.Children)
            {
                if (child is Rectangle rectangle && rectangle != RedRectangle && rectangle != dragRectangle)
                {
                    Point rectCenter = new Point(Canvas.GetLeft(rectangle) + rectangle.Width / 2, Canvas.GetTop(rectangle) + rectangle.Height / 2);
                    double distance = GetDistance(dropPosition, rectCenter);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestRect = rectangle;
                    }
                }
            }

            return closestRect;
        }

        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private void deck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Note: Container dei targetPanels
                int targetPanelCount = canvas.Children.OfType<Rectangle>()
                                                      .Count(rect => rect.Name.StartsWith("targetPanel"));

                IEnumerable<Rectangle> targetPanels = canvas.Children.OfType<Rectangle>()
                                                                    .Where(rect => rect.Name.StartsWith("targetPanel"))
                                                                    .Take(targetPanelCount / 2);



                List<Rectangle> rectanglesToAdd = new List<Rectangle>();
                foreach (Rectangle targetPanel in targetPanels)
                {
                    Deck.Card drawnCard = table.DrawCardFromDeck();
                    //percorso
                    string imagePath = $"../../../images/{drawnCard.ImagePath}";

                    if (!System.IO.File.Exists(imagePath))
                    {
                        MessageBox.Show($"Image file not found: {imagePath}");
                        return;
                    }

                    Rectangle rectangle = new Rectangle();
                    //crea effettiva immagine nel rettangolo
                    rectangle.Fill = new ImageBrush(new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)));

                    rectangle.Width = 175;
                    rectangle.Height = 235;

                    double horizontalPosition = Canvas.GetLeft(targetPanel);
                    double verticalPosition = Canvas.GetTop(targetPanel5);

                    Canvas.SetLeft(rectangle, horizontalPosition);
                    Canvas.SetTop(rectangle, verticalPosition);

                    rectanglesToAdd.Add(rectangle);
                }

                foreach (Rectangle rectangle in rectanglesToAdd)
                {
                    canvas.Children.Add(rectangle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in deck_Click method: {ex.GetType().Name}\n{ex.Message}\n{ex.StackTrace}");
            }
        }

    }

}