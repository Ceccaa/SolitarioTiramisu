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
        private Deck mazzo = new Deck();

        // Dictionary to track cards associated with each target panel
        private Dictionary<Rectangle, List<Rectangle>> targetPanelCards = new Dictionary<Rectangle, List<Rectangle>>();

        public GamePage()
        {
            InitializeComponent();
            table.OnGameEnd += GameTable_OnGameEnd; // Sottoscrizione all'evento
        }

        private void GameTable_OnGameEnd(string message)
        {
            // Mostra il messaggio di vittoria o sconfitta
            Dispatcher.Invoke(() =>
            {
                statusMessage.Text = message;
                statusMessage.Visibility = Visibility.Visible;
            });
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
                    Card card = mazzo.GetCardFromRectangle(draggedCard);
                    bool isMoveValid = PerformMove(card, closestRectangle);

                    if (isMoveValid)
                    {
                        double newLeft = Canvas.GetLeft(closestRectangle);
                        double newTop = Canvas.GetTop(closestRectangle);
                        Canvas.SetLeft(draggedCard, newLeft);
                        Canvas.SetTop(draggedCard, newTop);

                        // Update the card position in the dictionary
                        UpdateCardPositionInDictionary(draggedCard, closestRectangle);
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

        private void UpdateCardPositionInDictionary(Rectangle card, Rectangle newTargetPanel)
        {
            foreach (var kvp in targetPanelCards)
            {
                if (kvp.Value.Contains(card))
                {
                    kvp.Value.Remove(card);
                    break;
                }
            }

            if (!targetPanelCards.ContainsKey(newTargetPanel))
            {
                targetPanelCards[newTargetPanel] = new List<Rectangle>();
            }
            targetPanelCards[newTargetPanel].Add(card);
        }

        private bool PerformMove(Card card, Rectangle targetRectangle)
        {
            if (targetRectangle == targetPanel5)
            {
                return table.MinorMoveCard(card.Position, table.MiniDeck1);
            }
            else if (targetRectangle == targetPanel6)
            {
                return table.MinorMoveCard(card.Position, table.MiniDeck2);
            }
            else if (targetRectangle == targetPanel7)
            {
                return table.MinorMoveCard(card.Position, table.MiniDeck3);
            }
            else if (targetRectangle == targetPanel8)
            {
                return table.MinorMoveCard(card.Position, table.MiniDeck4);
            }
            else if (targetRectangle == targetPanel1)
            {
                return table.StairMoveCard(card.Position, table.StairDeck1);
            }
            else if (targetRectangle == targetPanel2)
            {
                return table.StairMoveCard(card.Position, table.StairDeck2);
            }
            else if (targetRectangle == targetPanel3)
            {
                return table.StairMoveCard(card.Position, table.StairDeck3);
            }
            else
            {
                return table.StairMoveCard(card.Position, table.StairDeck4);
            }
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
                    mazzo.LinkCardToRectangle(drawnCard, rectangle);

                    // Track the card in the targetPanelCards dictionary
                    if (!targetPanelCards.ContainsKey(targetPanel))
                    {
                        targetPanelCards[targetPanel] = new List<Rectangle>();
                    }
                    targetPanelCards[targetPanel].Add(rectangle);
                }
            }
            catch (Exception ex)
            {
                ClearTargetPanel(targetPanel5);
                ClearTargetPanel(targetPanel6);
                ClearTargetPanel(targetPanel7);
                ClearTargetPanel(targetPanel8);

                table.RedistributeDeck(table.MiniDeck4);
                table.RedistributeDeck(table.MiniDeck3);
                table.RedistributeDeck(table.MiniDeck2);
                table.RedistributeDeck(table.MiniDeck1);
            }
        }

        private void ClearTargetPanel(Rectangle targetPanel)
        {
            if (targetPanelCards.TryGetValue(targetPanel, out var cards))
            {
                foreach (var card in cards)
                {
                    canvas.Children.Remove(card);
                }
                cards.Clear();
            }
        }

        private void AssignCardToDeck(Card card, Rectangle targetPanel)
        {
            if (targetPanel == targetPanel5)
            {
                table.SetCardPosition(card, table.MiniDeck1);
                table.PushInDeck(card, table.MiniDeck1);
            }
            else if (targetPanel == targetPanel6)
            {
                table.SetCardPosition(card, table.MiniDeck2);
                table.PushInDeck(card, table.MiniDeck2);
            }
            else if (targetPanel == targetPanel7)
            {
                table.SetCardPosition(card, table.MiniDeck3);
                table.PushInDeck(card, table.MiniDeck3);
            }
            else if (targetPanel == targetPanel8)
            {
                table.SetCardPosition(card, table.MiniDeck4);
                table.PushInDeck(card, table.MiniDeck4);
            }
        }
    }
}