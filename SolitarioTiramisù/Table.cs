using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Windows;
using static SolitarioTiramisù.Deck;

namespace SolitarioTiramisù
{
    class Table
    {
        private Deck deck = new Deck();

        // mazzetti di appoggio su cui fare spostamenti
        private Stack<Card> miniDeck1 = new Stack<Card>();
        private Stack<Card> miniDeck2 = new Stack<Card>();
        private Stack<Card> miniDeck3 = new Stack<Card>();
        private Stack<Card> miniDeck4 = new Stack<Card>();

        // mazzetti principali in cui fare la scala
        private Stack<Card> stairDeck1 = new Stack<Card>();
        private Stack<Card> stairDeck2 = new Stack<Card>();
        private Stack<Card> stairDeck3 = new Stack<Card>();
        private Stack<Card> stairDeck4 = new Stack<Card>();

        //muovere carte da a, tra i mazzi inferiori 
        public void MinorMoveCard(Stack<Card> from, Stack<Card> to)
        {
            Card tmpCard = from.Pop();
            Card tmpCard2 = to.Pop();
            if(tmpCard.seed == tmpCard2.seed)
            {
                to.Push(tmpCard2);
                to.Push(tmpCard);
            }
        }

        //muovere carte da mazzi inferiori a mazzi superiori per fare la scala
        public void StairMoveCard(Stack<Card> from, Stack<Card> to)
        {
            Card tmpCard = from.Pop();
            Card tmpCard2 = to.Pop();
            if (tmpCard.seed == tmpCard2.seed && tmpCard.value > tmpCard2.value)
            {
                to.Push(tmpCard2);
                to.Push(tmpCard);
            }
        }

        //rimischiare il mazzo quando finisce. si puo fare solo 1 volta. Da chiamare una volta per ogni mazzetto.
        public void RedistributeDeck(Stack<Card> miniDeck)
        {
            if (deck.Count() == 0)
            {
                for(int i = 0; i < miniDeck.Count; i++)
                {
                    Card tmp = miniDeck.Pop();
                    deck.Push(tmp);
                }
            }
        }

    }
}
