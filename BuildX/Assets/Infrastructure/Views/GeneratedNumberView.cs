using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrilliantBingo.Code.Infrastructure.Views
{
    public class GeneratedNumberView : MonoBehaviour
    {
        #region Fields

        private TMP_Text _text;

        private Image _button;

        private bool _generated;
        private Color _color;
        #endregion

        #region Methods

        public void Awake()
        {
            _text = GetComponentInChildren<TMP_Text>();
            _button = GetComponent<Image>();
            _color = _button.color;
        }

        public void SetNumber(int number)
        {
            _text.text = number.ToString();
        }

        public void MarkAsGenerated()
        {
            _color.a = 1;
            _button.color = _color;
            _generated = true;
        }

        public void ResetNumber()
        {
            _color.a = 0;
            _button.color = _color;
            _generated = false;
        }

        #endregion

        #region Properties

        public bool Generated
        {
            get { return _generated; }
        }

        #endregion
    }
}