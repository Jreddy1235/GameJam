using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VideoPoker.Utils;

public class HandView : MonoBehaviour
{
    private readonly List<CardView> _onHoldCards = new List<CardView>();
    private readonly List<CardView> _allCards = new List<CardView>();

    [SerializeField] private GameObject goDealOver;
    [SerializeField] private GameObject goResults;
    [SerializeField] private TMP_Text txtResult;
    [SerializeField] private TMP_Text txtWinAmount;

    public bool IsMouseDown { get; private set; }

    private DealingDeck _dealingDeck;

    private void Awake()
    {
        _dealingDeck = VideoPokerManager.Instance.DealingDeck;
        goDealOver.SetActive(false);
        goResults.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            IsMouseDown = true;
        else if (Input.GetMouseButtonUp(0))
            IsMouseDown = false;
    }

    public void AddCard(CardView cardView)
    {
        _allCards.Add(cardView);
    }

    public void Deal()
    {
        var cards = _dealingDeck.Build().GetCards();
        for (var i = 0; i < _allCards.Count; i++)
        {
            var sprite = VideoPokerManager.Instance.DeckSprites.Get(cards[i].Rank, cards[i].Suit);
            _allCards[i].SetData(i, sprite);
        }

        goDealOver.SetActive(false);
        goResults.SetActive(false);
        _onHoldCards.Clear();
        VideoPokerManager.Instance.IsInputPaused = false;
    }

    public HandType Draw()
    {
        var cards = _dealingDeck.Build(_onHoldCards.Select(t => t.Index)).GetCards();
        foreach (var item in _allCards)
        {
            if (_onHoldCards.Contains(item))
            {
                item.SetData();
            }
            else
            {
                var sprite = VideoPokerManager.Instance.DeckSprites.Get(cards[item.Index].Rank, cards[item.Index].Suit);
                item.SetData(item.Index, sprite);
            }
        }

        VideoPokerManager.Instance.IsInputPaused = true;
        return ShowResults();
    }

    private HandType ShowResults()
    {
        var hand = _dealingDeck.GetWinningHand(VideoPokerManager.Instance.PayTableData.GetHandTypes());
        if (hand == null)
        {
            goDealOver.SetActive(true);
        }
        else
        {
            goResults.SetActive(true);
            var payTableRow = VideoPokerManager.Instance.PayTableData.GetPayTableRow(hand.HandType);
            var betIndex = VideoPokerManager.Instance.CurrentBetMultiplier - 1;
            var betAmountWon = payTableRow.Multipliers[betIndex] * VideoPokerManager.Instance.CurrentBetOption;
            txtWinAmount.text = betAmountWon.KiloFormat();
            txtResult.text = payTableRow.Name;
        }

        return hand?.HandType ?? HandType.None;
    }

    public void OnPointerClick(CardView cardView)
    {
        IsMouseDown = true;
        OnPointerEnter(cardView);
        IsMouseDown = false;
    }

    public void OnPointerEnter(CardView cardView)
    {
        if (VideoPokerManager.Instance.IsInputPaused || !IsMouseDown) return;

        cardView.ToggleOnHold();

        if (cardView.IsOnHold)
            _onHoldCards.Add(cardView);
        else
            _onHoldCards.Remove(cardView);
    }
}