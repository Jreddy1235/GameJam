using System;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VideoPoker.Utils;

public class GambleView : MonoBehaviour
{
    [SerializeField] private TMP_Text txtCost;
    [SerializeField] private TMP_Text txtWon;
    [SerializeField] private Image imgCard;
    [SerializeField] private GameObject goCardBack;
    [SerializeField] private GameObject goLose;
    [SerializeField] private GameObject goWin;
    [SerializeField] private GameObject goTint;

    private bool _inTransition;
    private CardSuit[] _selectedSuits;

    private void Start()
    {
        txtCost.text = GameData.Instance.GambleCost.KiloFormat();
        HUDManager.Instance.TotalChips.Subscribe(_ => CheckIfGamblePossible());
        HideResults();
    }

    private void CheckIfGamblePossible()
    {
        if(_inTransition) return;
        
        var isPossible = HUDManager.Instance.TotalChips.Value >= GameData.Instance.GambleCost;
        goTint.SetActive(!isPossible);
    }

    private void ShowResults()
    {
        var card = FullDeck.GetRandomCard();
        imgCard.sprite = VideoPokerManager.Instance.DeckSprites.Get(card.Rank, card.Suit);
        goCardBack.SetActive(false);
        _inTransition = true;
        Observable.ReturnUnit()
            .Delay(TimeSpan.FromSeconds(GameData.Instance.AutoHideGambleResultsDuration))
            .Subscribe(_ => HideResults());

        if (_selectedSuits.Contains(card.Suit))
        {
            goWin.SetActive(true);
            var ticketsWon = GameData.Instance.GambleRewardMultiplier * (_selectedSuits.Length == 1 ? 4 : 2);
            txtWon.text = "" + ticketsWon;
            HUDManager.Instance.TotalTickets.Value += ticketsWon;
        }
        else
        {
            goLose.SetActive(true);
        }
    }

    private void HideResults()
    {
        _inTransition = false;
        goCardBack.SetActive(true);
        goWin.SetActive(false);
        goLose.SetActive(false);
        CheckIfGamblePossible();
    }

    private void DoOnGamble()
    {
        if (_inTransition) return;

        ShowResults();
        HUDManager.Instance.TotalChips.Value -= GameData.Instance.GambleCost;
    }
    
    public void OnRedBtnClick()
    {
        _selectedSuits = new[] {CardSuit.Diamond, CardSuit.Heart};
        DoOnGamble();
    }

    public void OnBlackBtnClick()
    {
        _selectedSuits = new[] {CardSuit.Spade, CardSuit.Club};
        DoOnGamble();
    }

    public void OnSpadeBtnClick()
    {
        _selectedSuits = new[] {CardSuit.Spade};
        DoOnGamble();
    }

    public void OnDiamondBtnClick()
    {
        _selectedSuits = new[] {CardSuit.Diamond};
        DoOnGamble();
    }

    public void OnHeartBtnClick()
    {
        _selectedSuits = new[] {CardSuit.Heart};
        DoOnGamble();
    }

    public void OnClubBtnClick()
    {
        _selectedSuits = new[] {CardSuit.Club};
        DoOnGamble();
    }
}