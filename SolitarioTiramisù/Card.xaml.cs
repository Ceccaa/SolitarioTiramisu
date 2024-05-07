using System.Configuration;
using System.Data;
using System.Windows;


namespace SolitarioTiramisù
{
    class Deck
    {
        private struct Card
        {
            public int value;
            public string seed;
        }

        private Stack<Card> deck = new Stack<Card>();

        public Deck()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    Card c = new Card();
                    c.value = j;
                    switch (i)
                    {
                        case 0:
                            c.seed = "A"; //tastiera
                            break;
                        case 1:
                            c.seed = "B"; //mouse
                            break;
                        case 2:
                            c.seed = "C"; //usb
                            break;
                        case 3:
                            c.seed = "D"; //cuffie
                            break;
                    }
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

        private Card Draw()
        {
            return deck.Pop();
        }





    }

}
