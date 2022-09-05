using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Deck
{
	protected Card[] cards;

	public Card this [int index] {
		get {
			return cards [index];
		}
	}

	public abstract Deck Build ();

	public Card[] GetCards (Predicate<Card> predicate)
	{
		return cards.Where (t => predicate.Invoke (t)).ToArray ();
	}

	public Card[] GetCards ()
	{
		return cards;
	}

	public override string ToString ()
	{
		string str = "";
		foreach (var item in cards) {
			str += item.Suit.ToString () + item.Rank.ToString ();
			str += " , ";
		}
		if (str.Length > 0)
			str = str.Substring (0, str.Length - 3);
		return str;
	}
}
