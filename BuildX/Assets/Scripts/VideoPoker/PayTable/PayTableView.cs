using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VideoPoker.PayTable;
using VideoPoker.Utils;

public class PayTableView : MonoBehaviour
{
    [SerializeField] private PayTableRowView payTableRowPrefab;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject[] multiplierSelections;
    [SerializeField] private TMP_Text txtCurrentBet;
    [SerializeField] private TMP_Text txtBetRange;
    [SerializeField] private GameObject goDealBtn;
    [SerializeField] private GameObject goDrawBtn;
    [SerializeField] private HandView handView;

    private List<PayTableRowView> _payTableRows;
    private PayTableRowView _selectedRow;

    private void OnEnable()
    {
        GeneratePayTable();
        goDealBtn.SetActive(true);
        goDrawBtn.SetActive(false);
        SetBetRanges();
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
        SetMultiplierSelection();
    }

    public void OnDealBtnClick()
    {
        goDealBtn.SetActive(false);
        goDrawBtn.SetActive(true);
        handView.Deal();
        if (_selectedRow != null)
            _selectedRow.ToggleSelection(false, VideoPokerManager.Instance.PayTableData.ColorDeselected);
    }

    public void OnDrawButtonClick()
    {
        goDealBtn.SetActive(true);
        goDrawBtn.SetActive(false);
        var handType = handView.Draw();

        if (handType == HandType.None) return;
        _selectedRow = _payTableRows.Find(t => t.HandType == handType);
        _selectedRow.ToggleSelection(true, VideoPokerManager.Instance.PayTableData.ColorSelected);
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