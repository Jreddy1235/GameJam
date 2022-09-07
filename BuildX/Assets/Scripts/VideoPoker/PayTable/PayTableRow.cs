using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class PayTableRow : ICloneable
{
    [SerializeField] private HandType handType;
    [SerializeField] private string name;
    [SerializeField] private int[] multipliers;

    public HandType HandType => handType;

    public string Name => name;

    public int[] Multipliers => multipliers;

    public object Clone()
    {
        if (!(MemberwiseClone() is PayTableRow row)) return this;
        
        row.multipliers = multipliers.ToArray();
        return row;
    }
}