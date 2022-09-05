using System.Collections.Generic;


public interface IHand
{
    HandType HandType { get; set; }

    List<int> KeyCardIndices { get; set; }

    bool Validate(Deck deck);

    bool Validate(Deck deck, CardRank wildCard);

}
