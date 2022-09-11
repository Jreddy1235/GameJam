using TMPro;
using UniRx;
using UnityEngine;
using VideoPoker.Utils;

public class HUDView : MonoBehaviour
{
    [SerializeField] private TMP_Text txtChips;
    [SerializeField] private TMP_Text txtTickets;

    private void Awake()
    {
        Camera.main.aspect = 0.5625f;
        Application.targetFrameRate = 60;
        HUDManager.Instance.TotalChips = new ReactiveProperty<int>(GameData.Instance.DefaultChipsAmount);
        HUDManager.Instance.TotalChips.Subscribe(x => UpdateChipsHUD());

        HUDManager.Instance.TotalTickets = new ReactiveProperty<int>(GameData.Instance.DefaultTicketsAmount);
        HUDManager.Instance.TotalTickets.Subscribe(x => UpdateTicketsHUD());
    }

    private void UpdateChipsHUD()
    {
        txtChips.text = HUDManager.Instance.TotalChips.Value.KiloFormat();
    }
    
    private void UpdateTicketsHUD()
    {
        txtTickets.text = HUDManager.Instance.TotalTickets.Value.KiloFormat();
    }

    public void OnAddChipsBtnClick()
    {
        HUDManager.Instance.TotalChips.Value += GameData.Instance.AddChipsAmount;
    }

    public void OnAddTicketsBtnClick()
    {
        HUDManager.Instance.TotalTickets.Value += GameData.Instance.AddTicketsAmount;
    }
}