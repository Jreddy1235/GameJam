using UnityEngine;
using UnityEngine.UI;

namespace BrilliantBingo.Code.Infrastructure.Views
{
    public class GeneratedNumberView : MonoBehaviour
    {
        #region Fields

        private Text _text;

        private Button _button;

        private bool _generated;

        private Color _defaultDisabledColor;
        private Color _defaultNormalColor;
        private Color _defaultHighlightedColor;
        #endregion

        #region Methods

        public void Awake()
        {
            _text = GetComponentInChildren<Text>();
            _button = GetComponent<Button>();
            var c = _button.colors;
            _defaultDisabledColor = c.disabledColor;
            _defaultNormalColor = c.normalColor;
            _defaultHighlightedColor = c.highlightedColor;
        }

        public void SetNumber(int number)
        {
            _text.text = number.ToString();
        }

        public void MarkAsGenerated()
        {   
            var c = _button.colors;
            c.disabledColor = Color.red;
            c.normalColor = Color.red;
            c.highlightedColor = Color.red;
            _button.colors = c;
            _text.color = Color.white;
            _generated = true;
        }

        public void ResetNumber()
        {
            var c = _button.colors;
            c.disabledColor = _defaultDisabledColor;
            c.normalColor = _defaultNormalColor;
            c.highlightedColor = _defaultHighlightedColor;
            _button.colors = c;
            _text.color = Color.black;
            _generated = false;
        }

        public void Disable()
        {
            _button.interactable = false;
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