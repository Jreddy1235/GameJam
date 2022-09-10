using UnityEngine;

[CreateAssetMenu(menuName = "Main/Game Data", fileName = "GameData")]
public class GameData : ScriptableObject
{
    private static GameData _instance;

    public static GameData Instance => _instance ? _instance : _instance = Resources.Load<GameData>("GameData");

    [SerializeField] private int defaultChipsAmount;
    [SerializeField] private int addChipsAmount;
    [SerializeField] private int defaultTicketsAmount;
    [SerializeField] private int addTicketsAmount;
    [SerializeField] private int gambleCost;
    [SerializeField] private float autoHideGambleResultsDuration;
    [SerializeField] private int gambleRewardMultiplier;
    [SerializeField] private float bingoHeadersDelay;
    [SerializeField] private float wrongDaubDisappearTime;

    public int DefaultChipsAmount => defaultChipsAmount;
    public int AddChipsAmount => addChipsAmount;
    public int DefaultTicketsAmount => defaultTicketsAmount;
    public int AddTicketsAmount => addTicketsAmount;
    public float AutoHideGambleResultsDuration => autoHideGambleResultsDuration;
    public int GambleRewardMultiplier => gambleRewardMultiplier;

    public int GambleCost => gambleCost;

    public float BingoHeadersDelay => bingoHeadersDelay;

    public float WrongDaubDisappearTime => wrongDaubDisappearTime;
}