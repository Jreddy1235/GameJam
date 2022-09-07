using System.Collections.Generic;
using System.Linq;

public class DealingDeck : Deck
{
    private bool _isJokersWild;
    public DealingDeck(bool isJokersWild)
    {
        _isJokersWild = isJokersWild;
    }
    
    public override Deck Build()
    {
        var tempCards = FullDeck.AllCards.ToList();
        if (_isJokersWild)
            tempCards.Add(new Card(CardSuit.None, CardRank._Joker));
        cards = tempCards.OrderBy(t => System.Guid.NewGuid()).Take(5).ToArray();
        return this;
    }

    public Deck Build(Card[] cards)
    {
        this.cards = cards;
        return this;
    }

    public Deck Build(IEnumerable<int> indices)
    {
        if (cards == null)
            Build();
        var excludeCards = cards.Select((t, i) => new { index = i, card = t })
            .Where(t => indices.Any(x => x == t.index)).Select(t => t.card);
        var tempCards = FullDeck.AllCards.Where(t => !excludeCards.Any(x => x.Rank == t.Rank && x.Suit == t.Suit)).ToList();
        if (_isJokersWild)
            tempCards.Add(new Card(CardSuit.None, CardRank._Joker));
        var indicesArr = indices as int[] ?? indices.ToArray();
        tempCards = tempCards.OrderBy(t => System.Guid.NewGuid()).Take(GetCards().Length - indicesArr.Count()).ToList();
        var k = 0;
        for (var i = 0; i < cards.Length; i++)
        {
            if (!indicesArr.Contains(i))
                cards[i] = tempCards[k++];
        }

        return this;
    }
}
