using UnityEngine;
using TMPro;
using VideoPoker.Utils;

namespace VideoPoker.PayTable
{
    public class PayTableRowView : MonoBehaviour
    {
        [SerializeField] private GameObject goArrow;
        [SerializeField] private TMP_Text txtName;
        [SerializeField] private TMP_Text[] txtMultipliers;

        public void SetData(PayTableRow payTableRow, Color defaultColor)
        {
            goArrow.SetActive(false);
            txtName.text = payTableRow.Name;
            txtName.color = defaultColor;
            for (var i = 0; i < txtMultipliers.Length; i++)
            {
                txtMultipliers[i].color = defaultColor;
                if(payTableRow.Multipliers.Length <= i) continue;
                
                txtMultipliers[i].text = payTableRow.Multipliers[i].KiloFormat();
            }
        }

        public void ToggleSelection(bool isSelected, Color color)
        {
            goArrow.SetActive(isSelected);
            txtName.color = color;
            foreach (var item in txtMultipliers)
            {
                item.color = color;
            }
        }
    }
}