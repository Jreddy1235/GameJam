using UniRx;

public class HUDManager
{
    private static HUDManager _instance;

    public static HUDManager Instance => _instance ?? (_instance = new HUDManager());
    
    public ReactiveProperty<int> TotalChips { get; set; }
    public ReactiveProperty<int> TotalCoins { get; set; }

}