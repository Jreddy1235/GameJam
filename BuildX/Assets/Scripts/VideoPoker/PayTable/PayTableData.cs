using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VideoPoker.PayTable
{
    [CreateAssetMenu(menuName = "VideoPoker/PayTable Data", fileName = "PayTableData")]
    public class PayTableData : ScriptableObject
    {
        [SerializeField] private PayTableRow[] data;
        [SerializeField] private int[] betOptions;
        [SerializeField] private Color colorSelected;
        [SerializeField] private Color colorDeselected;
        [SerializeField] private bool isJokersWild;

        public bool IsJokersWild => isJokersWild;
        public Color ColorSelected => colorSelected;
        public Color ColorDeselected => colorDeselected;

        public (int,int) GetBetMultiplierRange(int index)
        {
            return (data[index].Multipliers.First(),data[index].Multipliers.Last());
        }
          
        public IEnumerable<HandType> GetHandTypes()
        {
            return data.Select(t=>t.HandType);
        }
        
        public PayTableRow[] GetPayTableData(int betAmount)
        {
            var currentData = data.Select(t => t.Clone() as PayTableRow).ToArray();
            foreach (var row in currentData)
            {
                for (var i = 0; i < row.Multipliers.Length; i++)
                {
                    row.Multipliers[i] *= betAmount;
                }
            }

            return currentData;
        }

        public int[] GetBetOptions()
        {
            return betOptions;
        }

        public PayTableRow GetPayTableRow(HandType handType)
        {
            return data.First(t => t.HandType == handType);
        }
    }
}