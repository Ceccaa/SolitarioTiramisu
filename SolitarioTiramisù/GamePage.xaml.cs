using SolitarioTiramisu;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static SolitarioTiramisu.Deck;

namespace SolitarioTiramisu
{
    public partial class GamePage : Page
    {
        private int zIndexCounter = 0;
        private Point startPoint;
        private Rectangle draggedCard;
        private Point originalPosition;
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
                    originalPosition = new Point(Canvas.GetLeft(draggedCard), Canvas.GetTop(draggedCard));
                    Panel.SetZIndex(draggedCard, ++zIndexCounter);
                    DragDrop.DoDragDrop(rectangle, rectangle, DragDropEffects.Move);
                }
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Rectangle)) && draggedCard != null)
            {
                Point dropPosition = e.GetPosition(canvas);
                Rectangle closestRectangle = GetClosestRectangle(dropPosition);

                if (closestRectangle != null)
                {
                    Card card = table.Deck.GetCardFromRectangle(draggedCard);
                    bool isMoveValid = PerformMove(card, closestRectangle);

                    if (isMoveValid)
                    {
                        double newLeft = Canvas.GetLeft(closestRectangle);
                        double newTop = Canvas.GetTop(closestRectangle);
                        Canvas.SetLeft(draggedCard, newLeft);
                        Canvas.SetTop(draggedCard, newTop);
                    }
                    else
                    {
                        Canvas.SetLeft(draggedCard, originalPosition.X);
                        Canvas.SetTop(draggedCard, originalPosition.Y);
                    }

                    e.Handled = true;
                    draggedCard = null;
                }
            }
        }

        private bool PerformMove(Card card, Rectangle targetRectangle)
        {
            if (targetRectangle == targetPanel5)
            {
                return table.MinorMoveCard(ref card, table.MiniDeck1);
            }
            else if (targetRectangle == targetPanel6)
            {
                return table.MinorMoveCard(ref card, table.MiniDeck2);
            }
            else if (targetRectangle == targetPanel7)
            {
                return table.MinorMoveCard(ref card, table.MiniDeck3);
            }
            else if (targetRectangle == targetPanel8)
            {
                return table.MinorMoveCard(ref card, table.MiniDeck4);
            }
            return false;
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Rectangle)) && draggedCard != null)
            {
                e.Effects = DragDropEffects.Move;
                Point position = e.GetPosition(canvas);
                Canvas.SetLeft(draggedCard, position.X - draggedCard.Width / 2);
                Canvas.SetTop(draggedCard, position.Y - draggedCard.Height / 2);
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
                var targetPanels = new List<Rectangle> { targetPanel5, targetPanel6, targetPanel7, targetPanel8 };

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
                    Panel.SetZIndex(rectangle, ++zIndexCounter);
                    Canvas.SetLeft(rectangle, horizontalPosition);
                    Canvas.SetTop(rectangle, verticalPosition);

                    rectangle.MouseMove += CardRectangle_MouseMove;

                    canvas.Children.Add(rectangle);

                    AssignCardToDeck(drawnCard, targetPanel);
                    table.Deck.LinkCardToRectangle(ref drawnCard, rectangle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Deck_Click method: {ex.GetType().Name}\n{ex.Message}\n{ex.StackTrace}");
            }
        }

        private void AssignCardToDeck(Card card, Rectangle targetPanel)
        {
            if (targetPanel == targetPanel5)
            {
                table.SetCardPosition(ref card, table.MiniDeck1);
                table.PushInDeck(ref card, table.MiniDeck1);
            }
            else if (targetPanel == targetPanel6)
            {
                table.SetCardPosition(ref card, table.MiniDeck2);
                table.PushInDeck(ref card, table.MiniDeck2);
            }
            else if (targetPanel == targetPanel7)
            {
                table.SetCardPosition(ref card, table.MiniDeck3);
                table.PushInDeck(ref card, table.MiniDeck3);
            }
            else if (targetPanel == targetPanel8)
            {
                table.SetCardPosition(ref card, table.MiniDeck4);
                table.PushInDeck(ref card, table.MiniDeck4);
            }
        }
    }
}
