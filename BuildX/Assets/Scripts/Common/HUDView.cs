using TMPro;
using UniRx;
using UnityEngine;
using VideoPoker.Utils;

public class HUDView : MonoBehaviour
{
    [SerializeField] private TMP_Text txtChips;
    [SerializeField] private TMP_Text txtCoins;
    [SerializeField] private int defaultChipsAmount;
    [SerializeField] private int addChipsAmount;
    [SerializeField] private int defaultCoinsAmount;
    [SerializeField] private int addCoinsAmount;

    private void Awake()
    {
        HUDManager.Instance.TotalChips = new ReactiveProperty<int>(defaultChipsAmount);
        HUDManager.Instance.TotalChips.Subscribe(x => UpdateChipsHUD());

        HUDManager.Instance.TotalCoins = new ReactiveProperty<int>(defaultCoinsAmount);
        HUDManager.Instance.TotalCoins.Subscribe(x => UpdateCoinsHUD());
    }

    private void UpdateChipsHUD()
    {
        txtChips.text = HUDManager.Instance.TotalChips.Value.KiloFormat();
    }
    
    private void UpdateCoinsHUD()
    {
        txtCoins.text = HUDManager.Instance.TotalCoins.Value.KiloFormat();
    }

    public void OnAddChipsBtnClick()
    {
        HUDManager.Instance.TotalChips.Value += addChipsAmount;
    }

    public void OnAddCoinsBtnClick()
    {
        HUDManager.Instance.TotalCoins.Value += addCoinsAmount;
    }
}