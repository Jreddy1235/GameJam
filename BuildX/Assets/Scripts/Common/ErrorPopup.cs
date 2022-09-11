using System;
using TMPro;
using UnityEngine;

public class ErrorPopup : BasePopup
{
    private const string NotEnoughText = "You do not have enough {0} to proceed...";
    private const string ChipsText = "Chips";
    private const string TicketsText = "Tickets";
    private const string BuyNowText = "Buy Now";

    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtDescription;
    [SerializeField] private TMP_Text txtFooter;
    [SerializeField] private GameObject goSubmit;
    [SerializeField] private TMP_Text txtSubmit;

    private Action _action;

    public void OnSubmit()
    {
        _action?.Invoke();
        Hide();
    }
    
    public void SetText(string title, string description, string footer, string submit)
    {
        if (!string.IsNullOrEmpty(title))
            txtTitle.text = title;
        if (!string.IsNullOrEmpty(description))
            txtDescription.text = description;
        if (!string.IsNullOrEmpty(footer))
            txtFooter.text = footer;
        if (!string.IsNullOrEmpty(submit))
            txtSubmit.text = submit;
    }

    public void SetAction(Action action)
    {
        goSubmit.SetActive(action != null);
        _action = action;
    }

    public void ShowNotEnoughChips()
    {
        SetText(null, string.Format(NotEnoughText, ChipsText), null, BuyNowText);
        SetAction(() => HUDManager.Instance.TotalChips.Value += GameData.Instance.AddChipsAmount);
        Show();
    }

    public void ShowNotEnoughTickets()
    {
        SetText(null, string.Format(NotEnoughText, TicketsText), null, BuyNowText);
        SetAction(() => HUDManager.Instance.TotalTickets.Value += GameData.Instance.AddTicketsAmount);
        Show();
    }
}