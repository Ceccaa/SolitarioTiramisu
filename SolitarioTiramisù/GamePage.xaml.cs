using SolitarioTiramisu;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SolitarioTiramisu
{
    public partial class GamePage : Page
    {
        private int it = 0;
        private Point startPoint;
        private Rectangle draggedCard;
        private Table table = new Table();

        public GamePage()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Chiudi la finestra principale
            Window.GetWindow(this)?.Close();
        }

        private void CardRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Rectangle rectangle = sender as Rectangle;
                if (rectangle != null && draggedCard == null)
                {
                    startPoint = e.GetPosition(canvas);
                    draggedCard = rectangle;
                    Panel.SetZIndex(draggedCard, ++it);
                    DragDrop.DoDragDrop(rectangle, rectangle, DragDropEffects.Move);
                }
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Rectangle)) && draggedCard != null)
            {
                Point dropPosition = e.GetPosition(canvas);

                // Ottieni il rettangolo target più vicino
                Rectangle closestRectangle = GetClosestRectangle(dropPosition);

                if (closestRectangle != null)
                {
                    double horizontalPosition = Canvas.GetLeft(closestRectangle);
                    double verticalPosition = Canvas.GetTop(closestRectangle);

                    // Set the new position of the rectangle
                    Canvas.SetLeft(draggedCard, horizontalPosition);
                    Canvas.SetTop(draggedCard, verticalPosition);
                }

                e.Handled = true;
                Panel.SetZIndex(draggedCard, ++it);
                draggedCard = null;
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Rectangle)))
            {
                e.Effects = DragDropEffects.Move;
                Point position = e.GetPosition(canvas);

                // Move the dragged card with the mouse cursor
                if (draggedCard != null)
                {
                    Canvas.SetLeft(draggedCard, position.X - draggedCard.Width / 2);
                    Canvas.SetTop(draggedCard, position.Y - draggedCard.Height / 2);
                }

                e.Handled = true;
            }
        }

        private Rectangle GetClosestRectangle(Point position)
        {
            Rectangle closestRectangle = null;
            double closestDistance = double.MaxValue;

            foreach (var child in canvas.Children)
            {
                if (child is Rectangle rectangle && rectangle.Name.StartsWith("targetPanel"))
                {
                    double left = Canvas.GetLeft(rectangle);
                    double top = Canvas.GetTop(rectangle);
                    Point rectCenter = new Point(left + rectangle.Width / 2, top + rectangle.Height / 2);

                    double distance = GetDistance(position, rectCenter);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestRectangle = rectangle;
                    }
                }
            }

            return closestRectangle;
        }

        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private void deck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var targetPanels = new List<Rectangle>
                {
                    targetPanel5, targetPanel6, targetPanel7, targetPanel8
                };

                foreach (var targetPanel in targetPanels)
                {

                    Deck.Card drawnCard = table.DrawCardFromDeck();
                    string imagePath = $"../../../images/{drawnCard.ImagePath}";

                    if (!System.IO.File.Exists(imagePath))
                    {
                        MessageBox.Show($"Image file not found: {imagePath}");
                        return;
                    }

                    Rectangle rectangle = new Rectangle
                    {
                        Width = 175,
                        Height = 235,
                        Fill = new ImageBrush(new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)))
                    };
                   
                    double horizontalPosition = Canvas.GetLeft(targetPanel);
                    double verticalPosition = Canvas.GetTop(targetPanel);
                    Panel.SetZIndex(rectangle, ++it);
                    Canvas.SetLeft(rectangle, horizontalPosition);
                    Canvas.SetTop(rectangle, verticalPosition);

                    // Abilitare il drag and drop
                    rectangle.MouseMove += CardRectangle_MouseMove;

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
