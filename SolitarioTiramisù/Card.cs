using System.Configuration;
using System.Data;
using System.Windows;


namespace SolitarioTiramisù
{
    class Deck
    {
        public struct Card
        {
            public int value { get; }
            public string seed { get; }

            public Card(int value, string seed)
            {
                this.value = value;
                this.seed = seed;
            }
        }

        private Stack<Card> deck = new Stack<Card>();

        public Deck()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    int value = j;
                    string seed = "";

                    switch (i)
                    {
                        case 0:
                            seed = "A"; //tastiera
                            break;
                        case 1:
                            seed = "B"; //mouse
                            break;
                        case 2:
                            seed = "C"; //usb
                            break;
                        case 3:
                            seed = "D"; //cuffie
                            break;
                    }

                    Card c = new Card(value, seed);
                    deck.Push(c);
                }
            }
            Shuffle();
        }   

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

        public Card Draw()
        {
            return deck.Pop();
        }

    }
}
