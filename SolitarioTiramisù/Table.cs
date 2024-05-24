using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media.TextFormatting;
using static SolitarioTiramisu.Deck;

namespace SolitarioTiramisu
{
    class Table
    {
        public Deck deck = new Deck();

        //TODO: implementare coerenza tra carte generate a video e vari mazzetti gestiti nel backend come degli stack (guardare sotto)

        // mazzetti di appoggio su cui fare spostamenti
        public Stack<Card> miniDeck1 = new Stack<Card>();
        public Stack<Card> miniDeck2 = new Stack<Card>();
        public Stack<Card> miniDeck3 = new Stack<Card>();
        public Stack<Card> miniDeck4 = new Stack<Card>();

        // mazzetti principali in cui fare la scala
        public Stack<Card> stairDeck1 = new Stack<Card>();
        public Stack<Card> stairDeck2 = new Stack<Card>();
        public Stack<Card> stairDeck3 = new Stack<Card>();
        public Stack<Card> stairDeck4 = new Stack<Card>();

        //muovere carte da a, tra i mazzi inferiori 
        public bool MinorMoveCard(ref Card tmpCard, Stack<Card> to)
        {
            Stack<Card> from = tmpCard.position;
            
            Card tmpCard2;
            if (to.TryPop(out tmpCard2))
            {
                if (tmpCard.seed == tmpCard2.seed)
                {
                    SetCardPosition(ref tmpCard, to);
                    PushInDeck(ref tmpCard2,to);
                    PushInDeck(ref tmpCard, to);
                    from.Pop();
                    return true;
                }
                PushInDeck(ref tmpCard2, to);
                PushInDeck(ref tmpCard, from);
                from.Pop();
                return false;
            } else
            {
                SetCardPosition(ref tmpCard, to);
                PushInDeck(ref tmpCard, to);        
                from.Pop();
                return true;
            }
                
            
/*          
            if(Win() == 0)
            {
                MessageBox.Show("Hai vinto!");
            }
            else
            {
                MessageBox.Show("Hai perso!");
            }   
*/
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
            /*
            if (Win() == 0)
            {
                MessageBox.Show("Hai vinto!");
            }
            else
            {
                MessageBox.Show("Hai perso!");
            }
            */
        }

        //rimischiare il mazzo quando finisce. si puo fare solo 1 volta. Da chiamare una volta per ogni mazzetto.
        public void RedistributeDeck(Stack<Card> miniDeck)
        {
            if (deck.Count() == 0)
            {
                for(int i = 0; i < miniDeck.Count; i++)
                {
                    Card tmp = miniDeck.Pop();
                    deck.Push(ref tmp);
                }
            }
        }
        public Card DrawCardFromDeck()
        {
            if (deck.Count() > 0)
                return deck.Pop();
            else
                throw new InvalidOperationException("Deck is empty.");
        }

        public int Win()
        {
            if(stairDeck1.Count == 10 && stairDeck2.Count == 10 && stairDeck3.Count == 10 && stairDeck4.Count == 10)
            {
                return 0; //ha vinto
            }
            else if(HasLost())
            {
                return 1; //ha perso
            }
            else { }

            return 2; // puo continuare a giocare
        }

        private bool HasLost()
        {
            List<Card> tmpList = new List<Card>();
            tmpList.Add(miniDeck1.Peek());
            tmpList.Add(miniDeck2.Peek());
            tmpList.Add(miniDeck3.Peek());
            tmpList.Add(miniDeck4.Peek());

            List<Card> stairList = new List<Card>();
            stairList.Add(stairDeck1.Peek());
            stairList.Add(stairDeck2.Peek());
            stairList.Add(stairDeck3.Peek());
            stairList.Add(stairDeck4.Peek());

            if (tmpList[1].seed != tmpList[2].seed && tmpList[2].seed != tmpList[3].seed && tmpList[3].seed != tmpList[4].seed)
            {
                for(int i = 0; i < tmpList.Count; i++)
                {
                    foreach (Card c in stairList)
                    {
                        if(c.seed == tmpList[i].seed && c.value+1 < tmpList[i].value)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
                
        }

        public void PushInDeck(ref Card card, Stack<Card> deck)
        {
            deck.Push(card);
            
        } 
        public void SetCardPosition(ref Card card, Stack<Card> to)
        {
            card.position = to;
        }


    }

}

