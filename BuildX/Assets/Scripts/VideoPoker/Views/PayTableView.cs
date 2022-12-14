using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoPoker.Utils;
using UniRx;

public class PayTableView : MonoBehaviour
{
    [SerializeField] private PayTableRowView payTableRowPrefab;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject[] multiplierSelections;
    [SerializeField] private TMP_Text txtCurrentBet;
    [SerializeField] private TMP_Text txtBetRange;
    [SerializeField] private GameObject goDealBtn;
    [SerializeField] private Button btnDeal;
    [SerializeField] private GameObject goDrawBtn;
    [SerializeField] private HandView handView;

    private List<PayTableRowView> _payTableRows;
    private PayTableRowView _selectedRow;

    private void Start()
    {
        GeneratePayTable();
        goDealBtn.SetActive(true);
        goDrawBtn.SetActive(false);
        SetBetRanges();
        HUDManager.Instance.TotalChips.Subscribe(SetDealBtn);
    }

    private void SetDealBtn(int amount)
    {
        btnDeal.interactable = amount >= VideoPokerManager.Instance.CurrentBet;
    }

    private void GeneratePayTable()
    {
        Clear();
        _payTableRows = new List<PayTableRowView>();
        var payTableData = VideoPokerManager.Instance.GetPayTableData();
        foreach (var item in payTableData)
        {
            var itemGo = Instantiate(payTableRowPrefab, itemsContainer);
            itemGo.SetData(item, VideoPokerManager.Instance.PayTableData.ColorDeselected);
            itemGo.transform.SetAsFirstSibling();
            _payTableRows.Add(itemGo);
        }
    }

    private void Clear()
    {
        if (_payTableRows == null) return;

        _payTableRows.ForEach(t => Destroy(t.gameObject));
        _payTableRows.Clear();
    }

    private void SetMultiplierSelection()
    {
        foreach (var item in multiplierSelections)
        {
            item.SetActive(false);
        }

        if (multiplierSelections.Length >= VideoPokerManager.Instance.CurrentBetMultiplier)
            multiplierSelections[VideoPokerManager.Instance.CurrentBetMultiplier - 1].SetActive(true);
    }

    private void SetBetRanges()
    {
        var range = VideoPokerManager.Instance.GetBetMultiplierRange();
        txtBetRange.text = range.Item1.KiloFormat() + "-" + range.Item2.KiloFormat();
        SetBetMultiplier();
    }

    private void SetBetMultiplier()
    {
        txtCurrentBet.text = VideoPokerManager.Instance.CurrentBet.KiloFormat();
        SetDealBtn(HUDManager.Instance.TotalChips.Value);
        SetMultiplierSelection();
    }

    public void OnDealBtnClick()
    {
        goDealBtn.SetActive(false);
        goDrawBtn.SetActive(true);
        HUDManager.Instance.TotalChips.Value -= VideoPokerManager.Instance.CurrentBet;
        handView.Deal();
        if (_selectedRow != null)
            _selectedRow.ToggleSelection(false, VideoPokerManager.Instance.PayTableData.ColorDeselected);
    }

    public void OnDrawButtonClick()
    {
        goDealBtn.SetActive(true);
        goDrawBtn.SetActive(false);
        var (handType,amountWon) = handView.Draw();

        if (handType == HandType.None)
        {
            SoundManager.Play(AudioType.NoMatch);
            return;
        }
        _selectedRow = _payTableRows.Find(t => t.HandType == handType);
        _selectedRow.ToggleSelection(true, VideoPokerManager.Instance.PayTableData.ColorSelected);
        HUDManager.Instance.TotalChips.Value += amountWon;
        SoundManager.Play(AudioType.GotChips);
    }

    public void OnBetIncreaseBtnClick()
    {
        VideoPokerManager.Instance.IncrementBetMultiplier();
        SetBetMultiplier();
    }

    public void OnBetDecreaseBtnClick()
    {
        VideoPokerManager.Instance.DecrementBetMultiplier();
        SetBetMultiplier();
    }

    public void OnBetRangeBtnClick()
    {
        VideoPokerManager.Instance.ChangeBetRange();
        SetBetRanges();
        GeneratePayTable();
    }
}