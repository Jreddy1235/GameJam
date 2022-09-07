using VideoPoker.PayTable;
using UnityEngine;

public class VideoPokerManager
{
    private static VideoPokerManager CreateInstance()
    {
        var instance = new VideoPokerManager
        {
            _payTableData = Resources.Load<PayTableData>("PayTableData"),
            _deckSprites = Resources.Load<DeckSprites>("DeckSprites"),
        };

        instance.Initialize();
        return instance;
    }

    private static VideoPokerManager _instance;
    private PayTableData _payTableData;
    private DeckSprites _deckSprites;

    public static VideoPokerManager Instance => _instance ?? (_instance = CreateInstance());

    public int CurrentBetOption => _payTableData.GetBetOptions()[CurrentBetRangeIndex];
    public int CurrentBet => CurrentBetMultiplier * CurrentBetOption;
    public int CurrentBetMultiplier { get; set; }
    public int CurrentBetRangeIndex { get; set; }

    public PayTableData PayTableData => _payTableData;

    public void Initialize()
    {
        CurrentBetRangeIndex = 0;
        CurrentBetMultiplier = _payTableData.GetBetMultiplierRange(0).Item1;
    }

    public PayTableRow[] GetPayTableData()
    {
        return _payTableData.GetPayTableData(CurrentBetOption);
    }

    public (int, int) GetBetMultiplierRange()
    {
        var range = _payTableData.GetBetMultiplierRange(0);
        return (range.Item1 * CurrentBetOption, range.Item2 * CurrentBetOption);
    }

    public void ChangeBetRange()
    {
        CurrentBetRangeIndex++;
        if (CurrentBetRangeIndex >= _payTableData.GetBetOptions().Length)
            CurrentBetRangeIndex = 0;
    }

    public void IncrementBetMultiplier()
    {
        CurrentBetMultiplier++;
        if (CurrentBetMultiplier > _payTableData.GetBetMultiplierRange(0).Item2)
            CurrentBetMultiplier = _payTableData.GetBetMultiplierRange(0).Item1;
    }
    
    public void DecrementBetMultiplier()
    {
        CurrentBetMultiplier--;
        if (CurrentBetMultiplier < _payTableData.GetBetMultiplierRange(0).Item1)
            CurrentBetMultiplier = _payTableData.GetBetMultiplierRange(0).Item2;
    }
}