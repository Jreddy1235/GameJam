using System;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;

[CreateAssetMenu(menuName = "VideoPoker/Deck Sprites", fileName = "DeckSprites")]
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

    public Sprite Get(CardRank rank, CardSuit suit)
    {
        return cardSprites.FirstOrDefault(t => t.rank == rank && t.suit == suit).sprite;
    }

#if UNITY_EDITOR
    [SerializeField] private string spritesFolder = "Textures/VideoPoker/Deck";

    [Button]
    private void PopulateSprites()
    {
        var fullPath = $"{Application.dataPath}/{spritesFolder}";
        if (!System.IO.Directory.Exists(fullPath))
        {
            return;
        }

        var folders = new string[] {$"Assets/{spritesFolder}"};
        var guids = UnityEditor.AssetDatabase.FindAssets("t:Sprite", folders);

        var newSprites = new Sprite[guids.Length];

        var mismatch = newSprites.Length != cardSprites.Length;

        for (var i = 0; i < newSprites.Length; i++)
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            newSprites[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
            mismatch |= (i < cardSprites.Length && cardSprites[i].sprite != newSprites[i]);
        }

        if (!mismatch) return;
        cardSprites = new CardStruct[newSprites.Length];
        for (var i = 0; i < cardSprites.Length; i++)
        {
            var names = newSprites[i].name.Split('_');
            cardSprites[i].sprite = newSprites[i];
            cardSprites[i].rank = (CardRank) Enum.Parse(typeof(CardRank), "_" + names[1]);
            cardSprites[i].suit = (CardSuit) Enum.Parse(typeof(CardSuit), names[0]);
        }

        EditorUtility.SetDirty(this);
        Debug.Log($"{name} sprite list updated.");
    }
#endif
}