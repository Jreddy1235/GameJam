using System.Collections.Generic;
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
    [SerializeField] private float restartAgainDuration;
    [SerializeField] private List<AudioClip> numbersAudio;
    [SerializeField] private int chipsWonFromBingo;
    [SerializeField] private int bingoOneCardCost;
    [SerializeField] private int bingoTwoCardCost;
    [SerializeField] private int bingoThreeCardCost;
    [SerializeField] private int bingoFourCardCost;

    public int DefaultChipsAmount => defaultChipsAmount;
    public int AddChipsAmount => addChipsAmount;
    public int DefaultTicketsAmount => defaultTicketsAmount;
    public int AddTicketsAmount => addTicketsAmount;
    public float AutoHideGambleResultsDuration => autoHideGambleResultsDuration;
    public int GambleRewardMultiplier => gambleRewardMultiplier;

    public int GambleCost => gambleCost;

    public float BingoHeadersDelay => bingoHeadersDelay;

    public float WrongDaubDisappearTime => wrongDaubDisappearTime;

    public float RestartAgainDuration => restartAgainDuration;
    
    public List<AudioClip> NumbersAudio => numbersAudio;
    public int ChipsWonFromBingo => chipsWonFromBingo;

    public int BingoOneCardCost => bingoOneCardCost;

    public int BingoTwoCardCost => bingoTwoCardCost;

    public int BingoThreeCardCost => bingoThreeCardCost;

    public int BingoFourCardCost => bingoFourCardCost;
}