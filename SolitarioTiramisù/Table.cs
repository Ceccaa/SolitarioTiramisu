using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Windows;
using static SolitarioTiramisù.Deck;

namespace SolitarioTiramisù
{
    class Table
    {
        // mazzetti di appoggio su cui fare spostamenti
        Stack<Card> miniDeck1 = new Stack<Card>();
        Stack<Card> miniDeck2 = new Stack<Card>();
        Stack<Card> miniDeck3 = new Stack<Card>();
        Stack<Card> miniDeck4 = new Stack<Card>();

        // mazzetti principali in cui fare la scala
        Stack<Card> stairDeck1 = new Stack<Card>();
        Stack<Card> stairDeck2 = new Stack<Card>();
        Stack<Card> stairDeck3 = new Stack<Card>();
        Stack<Card> stairDeck4 = new Stack<Card>();

        public void MoveCard(Stack<Card> from, Stack<Card> to)
        {
            //Todo: Una carta puo essere spostata in un altro mazzetto inferiore solo 1 volta

            Card tmpCard = from.Pop();
            Card tmpCard2 = to.Pop();
            if(tmpCard.seed == tmpCard2.seed)
            {
                to.Push(tmpCard2);
                to.Push(tmpCard);
            }
            
        }

    }
}
