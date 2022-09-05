using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardRank
{
	_Joker = -1,
	_A,
	_2,
	_3,
	_4,
	_5,
	_6,
	_7,
	_8,
	_9,
	_10,
	_J,
	_Q,
	_K,
}

public enum CardSuit
{
	None = -1,
	Club,
	Diamond,
	Heart,
	Spade
}

public enum CardColor
{
	Black,
	Red
}

[System.Serializable]
public class Card
{
	public CardRank Rank;

	public CardSuit Suit;

	public CardColor Color;

	public Card (CardSuit suit, CardRank rank)
	{
		Rank = rank;
		Suit = suit;
		Color = (suit == CardSuit.Club || suit == CardSuit.Spade) ? CardColor.Black : CardColor.Red;
	}


}

