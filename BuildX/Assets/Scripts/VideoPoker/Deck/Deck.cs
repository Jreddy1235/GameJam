using System.Linq;
using System;

public abstract class Deck
{
    protected Card[] cards;

    public Card this[int index]
    {
        get { return cards[index]; }
    }

    public abstract Deck Build();

    public Card[] GetCards(Predicate<Card> predicate)
    {
        return cards.Where(predicate.Invoke).ToArray();
    }

    public Card[] GetCards()
    {
        return cards;
    }

    public override string ToString()
    {
        var str = "";
        foreach (var item in cards)
        {
            str += item.Suit + item.Rank.ToString();
            str += " , ";
        }

        if (str.Length > 0)
            str = str.Substring(0, str.Length - 3);
        return str;
    }
}