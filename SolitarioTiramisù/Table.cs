using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using static SolitarioTiramisu.Deck;

namespace SolitarioTiramisu
{
    class Table
    {
        private Deck Deck = new Deck();

        // TODO: implementare coerenza tra carte generate a video e vari mazzetti gestiti nel backend come degli stack (guardare sotto)

        // mazzetti di appoggio su cui fare spostamenti
        public Stack<Card> MiniDeck1 = new Stack<Card>();
        public Stack<Card> MiniDeck2 = new Stack<Card>();
        public Stack<Card> MiniDeck3 = new Stack<Card>();
        public Stack<Card> MiniDeck4 = new Stack<Card>();

        // mazzetti principali in cui fare la scala
        public Stack<Card> StairDeck1 = new Stack<Card>();
        public Stack<Card> StairDeck2 = new Stack<Card>();
        public Stack<Card> StairDeck3 = new Stack<Card>();
        public Stack<Card> StairDeck4 = new Stack<Card>();
        private int redistribute = 0;

        // muovere carte da a, tra i mazzi inferiori 
        public bool MinorMoveCard(Stack<Card> from, Stack<Card> to)
        {
            if (Win() == 0)
            {
                Console.WriteLine("Hai vinto!");
                return true;
            }
            else if (Win() == 1)
            {
                Console.WriteLine("Hai perso!");
                return false;
            }
            else
            {

                Card tmpCard = from.Pop();

                if (to.TryPop(out Card tmpCard2))
                {
                    if (tmpCard.Seed == tmpCard2.Seed)
                    {
                        SetCardPosition(tmpCard, to);
                        PushInDeck(tmpCard2, to);
                        PushInDeck(tmpCard, to);
                        return true;
                    }

                    PushInDeck(tmpCard2, to);
                    PushInDeck(tmpCard, from);
                    return false;
                }
                else
                {
                    SetCardPosition(tmpCard, to);
                    PushInDeck(tmpCard, to);
                    return true;
                }
            }

        }

        // muovere carte da mazzi inferiori a mazzi superiori per fare la scala
        public bool StairMoveCard(Stack<Card> from, Stack<Card> to)
        { 
            if (Win() == 0)
            {
               Console.WriteLine("Hai vinto!");
                return true;
            }
            else if(Win() == 1)
            {
                Console.WriteLine("Hai perso!");
                return false;
            }
            else
            {
                Card tmpCard = from.Pop();

                if (to.TryPeek(out Card tmpCard2))
                {
                    if (tmpCard.Seed == tmpCard2.Seed && tmpCard.Value == tmpCard2.Value + 1)
                    {
                        SetCardPosition(tmpCard, to);
                        to.Push(tmpCard2);
                        to.Push(tmpCard);
                        return true;
                    }
                    else
                    {
                        SetCardPosition(tmpCard, from);
                        from.Push(tmpCard);
                        return false;
                    }
                }
                else
                {
                    if (tmpCard.Value == 1)
                    {
                        SetCardPosition(tmpCard, to);
                        to.Push(tmpCard);
                        return true;
                    }
                    SetCardPosition(tmpCard, from);
                    from.Push(tmpCard);
                    return false;

                }
            }
        }

        // rimischiare il mazzo quando finisce. si puo fare solo 1 volta. Da chiamare una volta per ogni mazzetto.
        public void RedistributeDeck(Stack<Card> miniDeck)
        {
            if(redistribute >= 4)
            {
                Console.WriteLine("Hai perso!");
                return;
            }
            else
            {
                redistribute++;
                while (miniDeck.Count > 0)
                {
                    Card tmp = miniDeck.Pop();
                    Deck.Push(tmp);
                }
            }


            
        }

        public Card DrawCardFromDeck()
        {
            if (Deck.Count() > 0)
                return Deck.Pop();
            else
                throw new InvalidOperationException("Deck is empty.");
        }

        public int Win()
        {
            if (StairDeck1.Count == 10 && StairDeck2.Count == 10 && StairDeck3.Count == 10 && StairDeck4.Count == 10)
            {
                return 0; // ha vinto
            }
            else if (HasLost())
            {
                return 1; // ha perso
            }

            return 2; // può continuare a giocare
        }

        private bool HasLost()
        {
            List<Card> miniList = new List<Card>();
            if (MiniDeck1.TryPeek(out Card tmp11)) miniList.Add(tmp11);
            if (MiniDeck2.TryPeek(out Card tmp22)) miniList.Add(tmp22);
            if (MiniDeck3.TryPeek(out Card tmp33)) miniList.Add(tmp33);
            if (MiniDeck4.TryPeek(out Card tmp44)) miniList.Add(tmp44);

            List<Card> stairList = new List<Card>();
            if (StairDeck1.TryPeek(out Card tmp1)) stairList.Add(tmp1);
            if (StairDeck2.TryPeek(out Card tmp2)) stairList.Add(tmp2);
            if (StairDeck3.TryPeek(out Card tmp3)) stairList.Add(tmp3);
            if (StairDeck4.TryPeek(out Card tmp4)) stairList.Add(tmp4);

            if(stairList.Count < 4)
            {
                return false;
            }

            if (miniList.Count < 4)
            {
                return false;
            }

            if (miniList[0].Seed != miniList[1].Seed && miniList[1].Seed != miniList[2].Seed && miniList[2].Seed != miniList[3].Seed)
            {
                for (int i = 0; i < miniList.Count; i++)
                {
                    foreach (Card c in stairList)
                    {
                        if (c.Seed == miniList[i].Seed && c.Value + 1 < miniList[i].Value)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        public void PushInDeck(Card card, Stack<Card> to)
        {
            to.Push(card);
        }

        public void SetCardPosition(Card card, Stack<Card> to)
        {
            card.Position = to;
        }
    }
}
