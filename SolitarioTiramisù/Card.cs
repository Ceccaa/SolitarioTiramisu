    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes; 


namespace SolitarioTiramisu
    {
        //Oggetto mazzo
        class Deck
        {
            //Struct carta
            public struct Card
            {
                public int value { get; }
                public string seed { get; }
                
                public string ImagePath { get; }

                public string position { get; set; }

                public Card(int value, string seed , string imagePath, string position)
                {
                    this.value = value;
                    this.seed = seed;
                    this.ImagePath = imagePath;
                    this.position = position;   
                }
            }

            //Rappresentazione del mazzo in memoria
            private Stack<Card> deck = new Stack<Card>();
            private Dictionary<Rectangle, Card> cardRectangleMap = new Dictionary<Rectangle, Card>();

        //riempimento del mazzo
            public Deck()
            {

                string folderPath = $"../../../images";
                string[] imageFiles = Directory.GetFiles(folderPath);

                Random random = new Random();

                foreach (string imagePath in imageFiles)
                {
                    string imageFileName = System.IO.Path.GetFileName(imagePath);


                    string[] fileNameParts = imageFileName.Split('_', '.');
                    string parts = fileNameParts[0];

                    //check cards
                    if (fileNameParts.Length >= 2 && fileNameParts[0] != "RETRO" && fileNameParts[0] != "Logo")
                    {
                        string nome ="";
                        int temp;
                        string seed = "";
                        for(int i = 0; i<parts.Length;i++)
                        {
                            if (int.TryParse(parts[i].ToString(),out temp))
                            {
                                nome += parts[i].ToString();
                            } else
                            {
                                if (i != 0)
                                {
                                    seed = parts[i].ToString();
                                }
                            }
                        }

                        int value = int.Parse(nome);
                        Card card = new Card(value, seed, imageFileName, "");
                        deck.Push(card);
                    }
                }

                Shuffle();
            }   

            //Mischio mazzo
            private void Shuffle()
            {
                Random r = new Random();
                Card[] temp = deck.ToArray();
                deck.Clear();
                foreach (Card c in temp.OrderBy(x => r.Next()))
                {
                    deck.Push(c);
                }
            }   

            //Pesco carta
            public Card Pop()
            {
                return deck.Pop();
            }

            public int Count()
            {
                return deck.Count;
            }

            public void Push(Card card)
            {
                //TODO: Valutare controlli eventuali
                deck.Push(card);
            }
            public void AssignPosition(Card card, string pos)
            {
                card.position = pos;
            }

            public void LinkCardToRectangle(Card card, Rectangle rectangle)
            {
                cardRectangleMap[rectangle] = card;
            }

            public Card GetCardFromRectangle(Rectangle rectangle)
            {
                return cardRectangleMap[rectangle];
            }




    }
    }
