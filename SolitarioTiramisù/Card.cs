using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SolitarioTiramisu
{
    // Oggetto mazzo
    public class Deck
    {
        public class Card
        {
            // Properties with private setters for immutability
            public int Value { get; private set; }
            public string Seed { get; private set; }
            public string ImagePath { get; private set; }

            // Property with a public setter
            public Stack<Card> Position { get; set; }

            // Constructor to initialize properties
            public Card(int value, string seed, string imagePath, Stack<Card> position)
            {
                Value = value;
                Seed = seed;
                ImagePath = imagePath;
                Position = position;
            }
        }

        // Rappresentazione del mazzo in memoria
        private Stack<Card> mazzo = new Stack<Card>();
        private Dictionary<Rectangle, Card> cardRectangleMap = new Dictionary<Rectangle, Card>();

        // Riempimento del mazzo
        public Deck()
        {
            string folderPath = "../../../images";
            string[] imageFiles = Directory.GetFiles(folderPath);

            Random random = new Random();

            foreach (string imagePath in imageFiles)
            {
                string imageFileName = System.IO.Path.GetFileName(imagePath);

                string[] fileNameParts = imageFileName.Split(new char[] { '_', '.' }, StringSplitOptions.RemoveEmptyEntries);
                string parts = fileNameParts[0];

                // Check cards
                if (fileNameParts.Length >= 2 && fileNameParts[0] != "RETRO" && fileNameParts[0] != "Logo" && fileNameParts[0] != "options" && fileNameParts[0] != "musicOff"
                    && fileNameParts[0] != "musicOn" && fileNameParts[0] != "book" && fileNameParts[0] != "exit" && fileNameParts[0] != "start")
                {
                    string nome = "";
                    int temp;
                    string seed = "";
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (int.TryParse(parts[i].ToString(), out temp))
                        {
                            nome += parts[i].ToString();
                        }
                        else
                        {
                            if (i != 0)
                            {
                                seed = parts[i].ToString();
                            }
                        }
                    }

                    int value = int.Parse(nome);
                    Card card = new Card(value, seed, imageFileName, null);
                    mazzo.Push(card);
                }
            }

            Shuffle();
        }

        // Mischio mazzo
        private void Shuffle()
        {
            Random r = new Random();
            Card[] temp = mazzo.ToArray();
            mazzo.Clear();
            foreach (Card c in temp.OrderBy(x => r.Next()))
            {
                mazzo.Push(c);
            }
        }

        // Pesco carta
        public Card Pop()
        {
            return mazzo.Pop();
        }

        public int Count()
        {
            return mazzo.Count;
        }

        public void Push(Card card)
        {
            // TODO: Valutare controlli eventuali
            mazzo.Push(card);
        }

        public void LinkCardToRectangle(Card card, Rectangle rectangle)
        {
            cardRectangleMap[rectangle] = card;
        }

        public Card GetCardFromRectangle(Rectangle rectangle)
        {
            return cardRectangleMap.TryGetValue(rectangle, out Card card) ? card : null;
        }
    }
}
