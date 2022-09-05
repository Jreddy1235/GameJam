using System;
using System.Collections.Generic;
using System.Linq;

public static class PokerHands
{
    public static List<IHand> GetAllHands()
    {
        List<IHand> allHands = new List<IHand>();
        allHands.Add(new FiveOfAKind());
        allHands.Add(new Flush());
        allHands.Add(new Four2s3s4s());
        allHands.Add(new FourAces());
        allHands.Add(new FourAcesWithKingQueenJack());
        allHands.Add(new FourDeuces());
        allHands.Add(new FourFivesToKings());
        allHands.Add(new FourOfAKind());
        allHands.Add(new FullHouse());
        allHands.Add(new JacksOrBetter());
        allHands.Add(new KingsOrBetter());
        allHands.Add(new RoyalFlush());
        allHands.Add(new Straight());
        allHands.Add(new StraightFlush());
        allHands.Add(new ThreeOfAKind());
        allHands.Add(new TwoPair());
        return allHands;
    }

    public static IEnumerable<IEnumerable<CardRank>> GroupWhile(this IEnumerable<CardRank> seq)
    {
        List<CardRank> list = new List<CardRank>();
        if (seq.Contains(CardRank._K) && seq.Contains(CardRank._Q) && seq.Contains(CardRank._J) && seq.Contains(CardRank._10) && seq.Contains(CardRank._A))
        {
            list = seq.ToList();
        }
        else
        {
            var ordSeq = seq.OrderBy(t => (int)t).ToList();
            for (int i = 0; i < ordSeq.Count; i++)
            {
                if (i == ordSeq.Count - 1)
                {
                    list.Add(ordSeq[i]);
                    break;
                }
                else if ((int)ordSeq[i + 1] - (int)ordSeq[i] == 1f)
                {
                    list.Add(ordSeq[i]);
                }
            }
            list = list.Distinct().ToList();
        }
        return new List<IEnumerable<CardRank>>() { list };
    }
    
    public static List<IHand> GetHands(IEnumerable<HandType> handTypes)
    {
        return GetAllHands().Where(t => handTypes.Contains(t.HandType)).ToList();
    }

    public static IHand GetWinningHand(this Deck deck, IEnumerable<HandType> handTypes)
    {
        var allHands = GetAllHands();
        var hands = handTypes.Select(t => allHands.FirstOrDefault(x => x.HandType == t));
        foreach (var item in hands)
        {
            if (item == null)
                continue;
            if (item.Validate(deck))
                return item;
        }
        return null;
    }

    public static IHand GetWinningHand(this Deck deck, IEnumerable<HandType> handTypes, CardRank wilCard)
    {
        var allHands = GetAllHands();
        var hands = handTypes.Select(t => allHands.FirstOrDefault(x => x.HandType == t));
        foreach (var item in hands)
        {
            if (item == null)
                continue;
            if (item.Validate(deck, wilCard))
                return item;
        }
        return null;
    }

    public static List<IHand> GetAllWinningHands(this Deck deck)
    {
        var hands = GetAllHands();
        List<IHand> winningHands = new List<IHand>();
        foreach (var item in hands)
        {
            if (item.Validate(deck))
                winningHands.Add(item);
        }
        return winningHands;
    }

    public static List<IHand> GetAllWinningHands(this Deck deck, CardRank wilCard)
    {
        var hands = GetAllHands();
        List<IHand> winningHands = new List<IHand>();
        foreach (var item in hands)
        {
            if (item.Validate(deck, wilCard))
                winningHands.Add(item);
        }
        return winningHands;
    }

    public class JacksOrBetter : IHand
    {
        public JacksOrBetter()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            var wildCards = cards.Where(t => t == wildCard);
            if (Validate(cards))
                return true;
            else if (wildCards.Count() > 1)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            else if (wildCards.Count() > 0 && cards.Any(t => ((int)t >= (int)CardRank._J) || t == CardRank._A)
                     && ((int)wildCard < (int)CardRank._J && wildCard != CardRank._A))
            {

                KeyCardIndices = indexedCards.Where(t => ((int)t.card >= (int)CardRank._J) || t.card == CardRank._A)
                    .Select(t => t.index).ToList();
                return true;
            }
            return false;
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            cards = cards.Where(t => ((int)t >= (int)CardRank._J) || t == CardRank._A);
            if (cards.Count() > 1)
            {
                var groupedCards = cards.ToLookup(t => t);
                int count = groupedCards.Max(t => t.Count());
                var selectedGroup = groupedCards.First(t => t.Count() == count);
                if (count >= 2)
                {
                    KeyCardIndices = indexedCards.Where(t => selectedGroup.Contains(t.card))
                        .Select(t => t.index).ToList();
                    return true;
                }
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }

        #endregion
    }

    public class KingsOrBetter : IHand
    {
        public KingsOrBetter()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            var wildCards = cards.Where(t => t == wildCard);
            if (Validate(cards))
                return true;
            else if (wildCards.Count() > 0 && cards.Any(t => ((int)t >= (int)CardRank._K) || t == CardRank._A)
                     && ((int)wildCard < (int)CardRank._J && wildCard != CardRank._A))
            {
                KeyCardIndices = indexedCards.Where(t => ((int)t.card >= (int)CardRank._K) || t.card == CardRank._A)
                   .Select(t => t.index).ToList();
                return true;
            }
            return false;
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            cards = cards.Where(t => ((int)t >= (int)CardRank._K) || t == CardRank._A);
            if (cards.Count() > 1)
            {
                var groupedCards = cards.ToLookup(t => t);
                int count = groupedCards.Max(t => t.Count());
                var selectedGroup = groupedCards.First(t => t.Count() == count);
                if (count >= 2)
                {
                    KeyCardIndices = indexedCards.Where(t => selectedGroup.Contains(t.card))
                        .Select(t => t.index).ToList();
                    return true;
                }
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class TwoPair : IHand
    {
        public TwoPair()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            var wildCards = cards.Where(t => t == wildCard);
            if (wildCards.Count() > 1)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            else if (wildCards.Count() > 0)
            {
                var groupedCards = cards.ToLookup(t => t);
                int count = groupedCards.Max(t => t.Count());
                var selectedGroup = groupedCards.First(t => t.Count() == count);
                if (count >= 2)
                {
                    KeyCardIndices = indexedCards.Where(t => selectedGroup.Contains(t.card) || t.card == wildCard)
                        .Select(t => t.index).ToList();
                    KeyCardIndices.Add(indexedCards.First(t => !KeyCardIndices.Contains(t.index)).index);
                    return true;
                }
            }
            return Validate(cards);
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            var groupedCards = cards.ToLookup(t => t);
            int count = groupedCards.Count(t => t.Count() == 2);

            if (count >= 2)
            {
                KeyCardIndices = new List<int>();
                foreach (var item in groupedCards)
                {
                    if (item.Count() >= 2)
                        KeyCardIndices.AddRange(indexedCards.Where(t => item.Contains(t.card)).Select(t => t.index));
                }
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class ThreeOfAKind : IHand
    {
        public ThreeOfAKind()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            var wildCards = cards.Where(t => t == wildCard);
            if (wildCards.Count() > 1)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            else if (wildCards.Count() > 0)
            {
                var groupedCards = cards.ToLookup(t => t);
                int count = groupedCards.Max(t => t.Count());
                var selectedGroup = groupedCards.First(t => t.Count() == count);
                KeyCardIndices = indexedCards.Where(t => selectedGroup.Contains(t.card)).Select(t => t.index).ToList();
                if (count > 2)

                    return true;
                else if (count == 2 && !groupedCards.First(t => t.Count() == 2).Contains(wildCard))
                {
                    KeyCardIndices.AddRange(indexedCards.Where(t => t.card == wildCard).Select(t => t.index));
                    return true;
                }
            }
            return Validate(cards);
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            var groupedCards = cards.ToLookup(t => t);
            int count = groupedCards.Max(t => t.Count());

            if (count >= 3)
            {
                KeyCardIndices = new List<int>();
                foreach (var item in groupedCards)
                {
                    if (item.Count() >= 3)
                        KeyCardIndices.AddRange(indexedCards.Where(t => item.Contains(t.card)).Select(t => t.index));
                }
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class FourOfAKind : IHand
    {
        public FourOfAKind()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            var wildCards = cards.Where(t => t == wildCard);
            var groupedCards = cards.ToLookup(t => t);
            if (wildCards.Count() > 2)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            else if (wildCards.Count() > 1)
            {
                int count = groupedCards.Max(t => t.Count());
                if (count >= 2 && groupedCards.Where(t => t.Count() >= 2).Count() > 1)
                {
                    KeyCardIndices = new List<int>();
                    foreach (var item in groupedCards)
                    {
                        if (item.Count() >= 2)
                            KeyCardIndices.AddRange(indexedCards.Where(t => item.Contains(t.card)).Select(t => t.index));
                    }
                    return true;
                }
            }
            else if (wildCards.Count() > 0)
            {
                int count = groupedCards.Max(t => t.Count());
                if (count >= 3)
                {
                    KeyCardIndices = new List<int>();
                    foreach (var item in groupedCards)
                    {
                        if (item.Count() >= 2)
                            KeyCardIndices.AddRange(indexedCards.Where(t => item.Contains(t.card)).Select(t => t.index));
                    }
                    KeyCardIndices.AddRange(indexedCards.Where(t => t.card == wildCard).Select(t => t.index));
                    return true;
                }
            }
            return Validate(cards);
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            var indexedCards = cards.Select((t, i) => new { index = i, card = t });
            var groupedCards = cards.ToLookup(t => t);
            int count = groupedCards.Max(t => t.Count());
            if (count >= 4)
            {
                KeyCardIndices = new List<int>();
                foreach (var item in groupedCards)
                {
                    if (item.Count() >= 4)
                        KeyCardIndices.AddRange(indexedCards.Where(t => item.Contains(t.card)).Select(t => t.index));
                }
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class Straight : IHand
    {
        public Straight()
        {
            KeyCardIndices = Enumerable.Range(0, 5).ToList();
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards, wildCard);
        }

        private bool Validate(IEnumerable<CardRank> cards, CardRank wildCard)
        {
            var wildCards = cards.Select((s, i) => new { i, s }).Where(t => t.s == wildCard).Select(t => t.i);
            if (wildCards.Count() > 0)
            {
                foreach (var index in wildCards)
                {
                    var newCards = cards.ToList();
                    var query = System.Enum.GetValues(typeof(CardRank)).Cast<CardRank>().Except(new CardRank[] { wildCard });
                    foreach (var item in query)
                    {
                        newCards[index] = item;
                        bool isValid = Validate(newCards, wildCard);
                        if (isValid)
                        {
                            return true;
                        }
                    }
                }
            }
            return Validate(cards);
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            var excludeCards = new List<CardRank>() {
                CardRank._10,
                CardRank._J,
                CardRank._Q,
                CardRank._K,
                CardRank._A,
            }.Except(cards);
            if (cards.Count() == 0)
            {
                return true;
            }
            if (cards.GroupWhile().Max(t => t.Count()) >= 5)
            {
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class Flush : IHand
    {
        public Flush()
        {
            KeyCardIndices = Enumerable.Range(0, 5).ToList();
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Suit);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var cards = deck.GetCards().Select(t => t.Suit);
            var wildCards = deck.GetCards().Where(t => t.Rank == wildCard);
            var grouped = deck.GetCards().Where(t => t.Rank != wildCard).ToLookup(t => t.Suit);
            if (wildCards.Count() > 3)
            {
                return true;
            }
            else if (wildCards.Count() > 2)
            {
                if (grouped.Max(t => t.Count()) >= 2)
                {
                    return true;
                }
            }
            else if (wildCards.Count() > 1)
            {
                if (grouped.Max(t => t.Count()) >= 3)
                {
                    return true;
                }
            }
            else if (wildCards.Count() > 0)
            {
                if (grouped.Max(t => t.Count()) >= 4)
                {
                    return true;
                }
            }
            return Validate(cards);
        }

        private bool Validate(IEnumerable<CardSuit> cards)
        {
            if (cards.ToLookup(t => t).Max(t => t.Count()) >= 5)
            {
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class FullHouse : IHand
    {
        public FullHouse()
        {
            KeyCardIndices = Enumerable.Range(0, 5).ToList();
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            if (Validate(deck))
                return true;
            else if (wildCard == CardRank._2)
                HandType = HandType.FullHouseDeuce;
            else if (wildCard == CardRank._Joker)
                HandType = HandType.FullHouseJoker;

            var cards = deck.GetCards().Select(t => t.Rank);
            var wildCards = cards.Where(t => t == wildCard);
            var grouped = cards.Where(t => t != wildCard).ToLookup(t => t);
            if (Validate(cards))
                return true;
            else if (wildCards.Count() > 2)
            {
                return true;
            }
            else if (wildCards.Count() > 1)
            {
                if (grouped.Max(t => t.Count()) > 1)
                    return true;
            }
            else if (wildCards.Count() > 0)
            {
                if (grouped.Max(t => t.Count()) > 2)
                    return true;
                if (grouped.Count(t => t.Count() == 2) == 2)
                    return true;
            }
            return false;
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            var grouped = cards.ToLookup(t => t);
            if (grouped.Count == 2 && grouped.Max(t => t.Count()) == 3)
                return true;
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class StraightFlush : IHand
    {
        public StraightFlush()
        {
            KeyCardIndices = Enumerable.Range(0, 5).ToList();
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards();
            if (Validate(cards.Select(t => t.Suit)))
                return Validate(cards.Select(t => t.Rank));
            return false;
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            if (deck.GetCards().Where(t => t.Rank != wildCard).ToLookup(t => t.Suit).Count() == 1)
            {
                return Validate(cards, wildCard);
            }
            return false;
        }

        private bool Validate(IEnumerable<CardRank> cards, CardRank wildCard)
        {
            var wildCards = cards.Select((s, i) => new { i, s }).Where(t => t.s == wildCard).Select(t => t.i);
            if (wildCards.Count() > 0)
            {
                foreach (var index in wildCards)
                {
                    var newCards = cards.ToList();
                    var query = System.Enum.GetValues(typeof(CardRank)).Cast<CardRank>().Except(new CardRank[] { wildCard });
                    foreach (var item in query)
                    {
                        newCards[index] = item;
                        bool isValid = Validate(newCards, wildCard);
                        if (isValid)
                            return true;
                    }
                }
            }
            return Validate(cards);
        }

        private bool Validate(IEnumerable<CardSuit> cards)
        {
            if (cards.ToLookup(t => t).Max(t => t.Count()) >= 5)
                return true;
            return false;
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            if (cards.GroupWhile().Max(t => t.Count()) >= 5)
            {
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class RoyalFlush : IHand
    {
        public RoyalFlush()
        {
            KeyCardIndices = Enumerable.Range(0, 5).ToList();
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards();
            if (Validate(cards.Select(t => t.Suit)))
                return Validate(cards.Select(t => t.Rank));
            return false;
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            if (Validate(deck))
                return true;
            else if (wildCard == CardRank._2)
                HandType = HandType.DeuceRoyalFlush;
            else if (wildCard == CardRank._Joker)
                HandType = HandType.RoyalFlushWild;

            var cards = deck.GetCards().Select(t => t.Rank);
            if (deck.GetCards().Where(t => t.Rank != wildCard).ToLookup(t => t.Suit).Count() == 1)
            {
                return Validate(cards, wildCard);
            }
            return false;
        }

        private bool Validate(IEnumerable<CardRank> cards, CardRank wildCard)
        {
            var extraCards = new List<CardRank>() {
                CardRank._10,
                CardRank._J,
                CardRank._Q,
                CardRank._K,
                CardRank._A,
            }.Except(cards);
            int count = extraCards.Count();
            var wildCards = cards.Where(t => t == wildCard);
            var notWildCards = cards.Where(t => t != wildCard);
            foreach (var item in notWildCards)
            {
                if ((int)item > (int)CardRank._A && (int)item < (int)CardRank._10)
                    return false;
            }
            if (wildCards.Count() > 3 && count < 5)
                return true;
            else if (wildCards.Count() > 2 && count < 4)
                return true;
            else if (wildCards.Count() > 1 && count < 3)
                return true;
            else if (wildCards.Count() > 0 && count < 2)
                return true;
            return false;
        }

        private bool Validate(IEnumerable<CardSuit> cards)
        {
            if (cards.ToLookup(t => t).Max(t => t.Count()) >= 5)
                return true;
            return false;
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            if (!new List<CardRank>() {
                CardRank._10,
                CardRank._J,
                CardRank._Q,
                CardRank._K,
                CardRank._A,
            }.Except(cards).Any())
            {
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class FourDeuces : IHand
    {
        public FourDeuces()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var indexedCards = deck.GetCards().Select((t, i) => new { index = i, card = t.Rank });
            if (deck.GetCards(t => t.Rank == CardRank._2).Count() == 4)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._2).Select(t => t.index).ToList();
                return true;
            }
            return false;
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var indexedCards = deck.GetCards().Select((t, i) => new { index = i, card = t.Rank });
            int count = deck.GetCards(t => t.Rank != CardRank._2 && t.Rank != wildCard).Count();
            if (count <= 1)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._2 || t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class FourAces : IHand
    {
        public FourAces()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var indexedCards = deck.GetCards().Select((t, i) => new { index = i, card = t.Rank });
            if (deck.GetCards(t => t.Rank == CardRank._A).Count() == 4)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._A).Select(t => t.index).ToList();
                return true;
            }
            return false;
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var indexedCards = deck.GetCards().Select((t, i) => new { index = i, card = t.Rank });
            int count = deck.GetCards(t => t.Rank != CardRank._A && t.Rank != wildCard).Count();
            if (count <= 1)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._A || t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class Four2s3s4s : IHand
    {
        public Four2s3s4s()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var indexedCards = deck.GetCards().Select((t, i) => new { index = i, card = t.Rank });
            if (deck.GetCards(t => t.Rank == CardRank._2).Count() == 4)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._2).Select(t => t.index).ToList();
                return true;
            }
            if (deck.GetCards(t => t.Rank == CardRank._3).Count() == 4)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._3).Select(t => t.index).ToList();
                return true;
            }
            if (deck.GetCards(t => t.Rank == CardRank._4).Count() == 4)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._4).Select(t => t.index).ToList();
                return true;
            }
            return false;
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            var indexedCards = deck.GetCards().Select((t, i) => new { index = i, card = t.Rank });
            int count = deck.GetCards(t => t.Rank != CardRank._2 && t.Rank != wildCard).Count();
            if (count <= 1)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._2 || t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            count = deck.GetCards(t => t.Rank != CardRank._3 && t.Rank != wildCard).Count();
            if (count <= 1)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._3 || t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            count = deck.GetCards(t => t.Rank != CardRank._4 && t.Rank != wildCard).Count();
            if (count <= 1)
            {
                KeyCardIndices = indexedCards.Where(t => t.card == CardRank._4 || t.card == wildCard).Select(t => t.index).ToList();
                return true;
            }
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class FiveOfAKind : IHand
    {
        public FiveOfAKind()
        {
            KeyCardIndices = Enumerable.Range(0, 5).ToList();
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards().Select(t => t.Rank);
            return Validate(cards);
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            if (deck.GetCards().Where(t => t.Rank != wildCard).ToLookup(t => t.Rank).Count >= 2)
                return false;
            return true;
        }

        private bool Validate(IEnumerable<CardRank> cards)
        {
            int count = cards.ToLookup(t => t).Max(t => t.Count());
            if (count >= 5)
                return true;
            return false;
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class FourFivesToKings : IHand
    {
        public FourFivesToKings()
        {
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards();
            var indexedCards = cards.Select((t, i) => new { index = i, card = t.Rank });
            for (int i = (int)CardRank._5; i < (int)(CardRank._K + 1); i++)
            {
                if (cards.Where(t => (int)t.Rank == i).Count() == 4)
                {
                    KeyCardIndices = indexedCards.Where(t => (int)t.card == i).Select(t => t.index).ToList();
                    return true;
                }
            }
            return false;
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            return Validate(deck);
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }

    public class FourAcesWithKingQueenJack : IHand
    {
        public FourAcesWithKingQueenJack()
        {
            KeyCardIndices = Enumerable.Range(0, 5).ToList();
            HandType = (HandType)System.Enum.Parse(typeof(HandType), GetType().Name);
        }

        #region IHand implementation

        public bool Validate(Deck deck)
        {
            var cards = deck.GetCards();
            if (cards.Where(t => t.Rank == CardRank._A).Count() == 4)
            {
                if (cards.Any(t => t.Rank != CardRank._A && (t.Rank == CardRank._K || t.Rank == CardRank._Q || t.Rank == CardRank._J)))
                    return true;
            }
            return false;
        }

        public bool Validate(Deck deck, CardRank wildCard)
        {
            return Validate(deck);
        }

        public HandType HandType { get; set; }

        public List<int> KeyCardIndices { get; set; }
        #endregion
    }


}
