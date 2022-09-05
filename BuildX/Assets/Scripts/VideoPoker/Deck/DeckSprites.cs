using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckSprites : ScriptableObject
{
	[System.Serializable]
	public struct CardStruct
	{
		public CardRank rank;
		public CardSuit suit;
		public Sprite sprite;
	}

	[SerializeField] private CardStruct[] cardSprites;

	public Sprite Get (CardRank rank, CardSuit suit)
	{
		return cardSprites.FirstOrDefault (t => t.rank == rank && t.suit == suit).sprite;
	}
}
