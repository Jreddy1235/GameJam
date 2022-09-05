using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FullDeck : Deck
{
	private static Deck instance;

	private static Deck Instance {
		get {
			if (instance == null)
				instance = new FullDeck ().Build ();
			return instance;
		}
	}

	public static IEnumerable<Card> AllCards {
		get {
			return Instance.GetCards ();
		}
	}

	public static Card GetRandomCard ()
	{
		return AllCards.ElementAt (Random.Range (0, AllCards.Count ()));
	}

	public override Deck Build ()
	{
		int suitLength = System.Enum.GetNames (typeof(CardSuit)).Length - 1;
		int rankLength = System.Enum.GetNames (typeof(CardRank)).Length - 1;
		cards = new Card[suitLength * rankLength];
		for (int i = 0; i < suitLength; i++) {
			for (int j = 0; j < rankLength; j++) {
				Card card = new Card ((CardSuit)i, (CardRank)j);
				cards [rankLength * i + j] = card;
			}
		}
		return this;
	}
}
