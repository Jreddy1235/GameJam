using System.Collections.Generic;
using VideoPoker.PayTable;
using UnityEngine;

public class VideoPokerManager
{
    public static VideoPokerManager Instance => _instance ?? (_instance = CreateInstance());

    private static VideoPokerManager _instance;

    public DealingDeck DealingDeck { get; private set; }
    public int CurrentBetOption => PayTableData.GetBetOptions()[CurrentBetRangeIndex];
    public int CurrentBet => CurrentBetMultiplier * CurrentBetOption;
    public int CurrentBetMultiplier { get; set; }
    public int CurrentBetRangeIndex { get; set; }
    public bool IsInputPaused { get; set; }
    public PayTableData PayTableData { get; private set; }
    public DeckSprites DeckSprites { get; private set; }

    private static VideoPokerManager CreateInstance()
    {
        var instance = new VideoPokerManager
        {
            PayTableData = Resources.Load<PayTableData>("PayTableData"),
            DeckSprites = Resources.Load<DeckSprites>("DeckSprites"),
        };

        instance.Initialize();
        return instance;
    }

    public void Initialize()
    {
        CurrentBetRangeIndex = 0;
        CurrentBetMultiplier = PayTableData.GetBetMultiplierRange(0).Item1;
        DealingDeck = new DealingDeck(PayTableData.IsJokersWild);
    }

    public PayTableRow[] GetPayTableData()
    {
        return PayTableData.GetPayTableData(CurrentBetOption);
    }

    public (int, int) GetBetMultiplierRange()
    {
        var range = PayTableData.GetBetMultiplierRange(0);
        return (range.Item1 * CurrentBetOption, range.Item2 * CurrentBetOption);
    }

    public void ChangeBetRange()
    {
        CurrentBetRangeIndex++;
        if (CurrentBetRangeIndex >= PayTableData.GetBetOptions().Length)
            CurrentBetRangeIndex = 0;
    }

    public void IncrementBetMultiplier()
    {
        CurrentBetMultiplier++;
        if (CurrentBetMultiplier > PayTableData.GetBetMultiplierRange(0).Item2)
            CurrentBetMultiplier = PayTableData.GetBetMultiplierRange(0).Item1;
    }

    public void DecrementBetMultiplier()
    {
        CurrentBetMultiplier--;
        if (CurrentBetMultiplier < PayTableData.GetBetMultiplierRange(0).Item1)
            CurrentBetMultiplier = PayTableData.GetBetMultiplierRange(0).Item2;
    }
}