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

    private List<PayTableRowView> _payTableRows;

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
    }

    public void OnDrawButtonClick()
    {
        goDealBtn.SetActive(true);
        goDrawBtn.SetActive(false);
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
    // public List<int> BetAmounts { get; set; }
    //
    // public int Count
    // {
    //     get { return rows.Count; }
    // }
    //
    // public const int ColCount = 5;
    //
    // public Dictionary<int, int> BetRanges { get; set; }
    //
    // private List<PayTableRow> rows;
    // private GameMode gameMode;
    //
    // public PayTableView(GameMode gameMode, GameRoom gameRoom)
    // {
    //     this.gameMode = gameMode;
    //     rows = GameController.instance.Progression.GetPayTableRows(gameMode, gameRoom);
    //     BetAmounts = rows.Select(t => t.minBetAmount).Where(t => t > 0).ToList();
    //     BetRanges = GetBetRanges();
    // }
    //
    // public string GetHandDescription(HandType handType)
    // {
    //     return rows.Find(t => t.handType == handType).name;
    // }
    //
    // public List<HandType> GetHandTypes()
    // {
    //     return rows.Select(t => t.handType).ToList();
    // }
    //
    // public int GetMaxBetAmount(int betRangeIndex)
    // {
    //     return BetRanges.ElementAt(betRangeIndex).Value;
    // }
    //
    // public int GetMinBetAmount(int betRangeIndex)
    // {
    //     return BetRanges.ElementAt(betRangeIndex).Key;
    // }
    //
    // public int GetBetIndex(int currentBet, int betRangeIndex)
    // {
    //     int minBet = BetRanges.ElementAt(betRangeIndex).Key;
    //     PayTableRow row = rows.Last();
    //     for (int i = 0; i < row.multipliers.Count; i++)
    //     {
    //         if (row.multipliers[i] * minBet == currentBet)
    //         {
    //             return i;
    //         }
    //     }
    //
    //     return -1;
    // }
    //
    // public KeyValuePair<int, int> GetNearestBet(GameMode gameMode, long totalChips)
    // {
    //     int amount = GameController.instance.Progression.GetDefaultBetAmount(gameMode, totalChips);
    //     if (amount > BetRanges.Last().Value)
    //         return new KeyValuePair<int, int>(BetRanges.Count - 1, BetRanges.Last().Value);
    //     if (amount < BetRanges.First().Key)
    //         return new KeyValuePair<int, int>(0, BetRanges.First().Key);
    //     var tempBet = BetRanges.First();
    //     if (tempBet.Key == tempBet.Value)
    //         tempBet = BetRanges.Last(t => t.Key <= amount);
    //     else
    //         tempBet = BetRanges.First(t => t.Key <= amount && t.Value > amount);
    //     PayTableRow row = rows.Last();
    //     if (gameMode == GameMode.HotPoker || gameMode == GameMode.Scattershot || gameMode == GameMode.MultiStagePoker)
    //     {
    //         for (int i = 0; i < rows.Count; i++)
    //         {
    //             if (rows[i].minBetAmount * tempBet.Key >= amount)
    //             {
    //                 return new KeyValuePair<int, int>(BetRanges.Keys.ToList().FindIndex(t => t == tempBet.Key),
    //                     tempBet.Key);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         for (int i = 0; i < row.multipliers.Count; i++)
    //         {
    //             if (row.multipliers[i] * tempBet.Key >= amount)
    //             {
    //                 return new KeyValuePair<int, int>(BetRanges.Keys.ToList().FindIndex(t => t == tempBet.Key),
    //                     (int) row.multipliers[i] * tempBet.Key);
    //             }
    //         }
    //     }
    //
    //     return new KeyValuePair<int, int>(row.multipliers.Count - 1, (int) row.multipliers.Last() * tempBet.Key);
    // }
    //
    // public int GetNextBetAmount(int currentBet, int betRangeIndex)
    // {
    //     int minBet = BetRanges.ElementAt(betRangeIndex).Key;
    //     PayTableRow row = rows.Last();
    //     for (int i = 0; i < row.multipliers.Count; i++)
    //     {
    //         if (row.multipliers[i] * minBet == currentBet)
    //         {
    //             if (i == row.multipliers.Count - 1)
    //                 return (int) (row.multipliers[0] * minBet);
    //             else
    //                 return (int) (row.multipliers[i + 1] * minBet);
    //         }
    //     }
    //
    //     return -1;
    // }
    //
    // public int GetPreviousBetAmount(int currentBet, int betRangeIndex)
    // {
    //     int minBet = BetRanges.ElementAt(betRangeIndex).Key;
    //     PayTableRow row = rows.Last();
    //     for (int i = 0; i < row.multipliers.Count; i++)
    //     {
    //         if (row.multipliers[i] * minBet == currentBet)
    //         {
    //             if (i == 0)
    //                 return (int) (row.multipliers[row.multipliers.Count - 1] * minBet);
    //             else
    //                 return (int) (row.multipliers[i - 1] * minBet);
    //         }
    //     }
    //
    //     return -1;
    // }
    //
    // private Dictionary<int, int> GetBetRanges()
    // {
    //     Dictionary<int, int> betRangeDict = new Dictionary<int, int>();
    //     for (int i = 0; i < BetAmounts.Count; i++)
    //     {
    //         betRangeDict[BetAmounts[i]] = (int) rows.Last().multipliers.Last() * BetAmounts[i];
    //     }
    //
    //     return betRangeDict;
    // }
    //
    // public Dictionary<string, List<int>> GetBetData(int betAmount)
    // {
    //     Dictionary<string, List<int>> betData = new Dictionary<string, List<int>>();
    //     for (int i = 0; i < rows.Count; i++)
    //     {
    //         betData[rows[i].name] = new List<int>();
    //         for (int j = 0; j < rows[i].multipliers.Count; j++)
    //         {
    //             switch (gameMode)
    //             {
    //                 case GameMode.ClassicPoker:
    //                     betData[rows[i].name].Add((int) (rows[i].multipliers[j] * betAmount));
    //                     break;
    //                 case GameMode.HotPoker:
    //                 case GameMode.MultiStagePoker:
    //                     betData[rows[i].name].Add((int) ((rows[i].multipliers[j] * betAmount) / 4));
    //                     break;
    //                 case GameMode.High5SequelChallenge:
    //                 case GameMode.Scattershot:
    //                     betData[rows[i].name].Add((int) ((rows[i].multipliers[j] * betAmount) / 5));
    //                     break;
    //             }
    //         }
    //     }
    //
    //     return betData;
    // }
    //
    // public Dictionary<string, List<int>> GetBetData(List<int> betAmounts, int betRangeIndex, out int skipCount)
    // {
    //     skipCount = 0;
    //     switch (gameMode)
    //     {
    //         case GameMode.HotPoker:
    //         case GameMode.MultiStagePoker:
    //             betAmounts = betAmounts.Concat(betAmounts.Take(betRangeIndex)).Skip(betRangeIndex).Take(ColCount)
    //                 .ToList();
    //             break;
    //         case GameMode.High5SequelChallenge:
    //         case GameMode.Scattershot:
    //             skipCount = Mathf.Clamp(betRangeIndex, 0, betAmounts.Count - ColCount);
    //             betAmounts = betAmounts.Skip(skipCount).Take(ColCount).ToList();
    //             break;
    //     }
    //
    //     Dictionary<string, List<int>> betData = new Dictionary<string, List<int>>();
    //     for (int i = 0; i < betAmounts.Count; i++)
    //     {
    //         var tempBetData = GetBetData(betAmounts[i]);
    //         if (betData.Count == 0)
    //         {
    //             foreach (var item in tempBetData)
    //             {
    //                 betData[item.Key] = item.Value;
    //             }
    //         }
    //         else
    //         {
    //             foreach (var item in betData)
    //             {
    //                 item.Value.AddRange(tempBetData[item.Key]);
    //             }
    //         }
    //     }
    //
    //     return betData;
    // }
}