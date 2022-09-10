using System;
using System.Globalization;
using BrilliantBingo.Code.Infrastructure.Core;
using BrilliantBingo.Code.Infrastructure.Events.Args;
using BrilliantBingo.Code.Infrastructure.Events.Handlers;
using BrilliantBingo.Code.Infrastructure.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrilliantBingo.Code.Infrastructure.Views
{
    public class CardNumberView : MonoBehaviour
    {
        [SerializeField] private Color _pressedColor;
        [SerializeField] private TMP_Text _text;

        #region Events

        public event CardNumberMarkedEventHandler Marked;

        private void OnMarked()
        {
            var handler = Marked;
            if (handler == null) return;
            handler(this, new CardNumberMarkedEventArgs(_columnLetter, _verticalIndex, _number));
        }

        public event CardNumberUnmarkedEventHandler Unmarked;

        private void OnUnmarked()
        {
            var handler = Unmarked;
            if (handler == null) return;
            handler(this, new CardNumberUnmarkedEventArgs(_columnLetter, _verticalIndex));
        }

        #endregion

        #region Fields

        private BingoLetter _columnLetter;

        private int _verticalIndex;

        private int _number;
        private int _leanTweenId1 = -1;
        private int _leanTweenId2 = -1;

        private bool _initialized;
        
        private Button _button;

        private bool _marked;

        #endregion

        #region Methods

        public void Initialize(BingoLetter columnLetter, int verticalIndex, int number)
        {
            if (_initialized)
            {
                throw new InvalidOperationException("CardNumberView already initialzied");
            }

            _columnLetter = columnLetter;
            _verticalIndex = verticalIndex;
            _number = number;

            GetButton().onClick.AddListener(Clicked);
            GetText().text = _number.ToString(CultureInfo.InvariantCulture);

            _initialized = true;
        }

        private void Clicked()
        {
            if (_marked)
            {
                Unmark();
            }
            else
            {
                Mark();
            }
        }

        private void Mark()
        {
            _button.targetGraphic.color = _pressedColor;
            _text.color = Color.white;


            var gnm = CoreGameObjectsLocator.Default.GeneratedNumbersManager;
            var isValid = gnm.CheckIfNumberWasGenerated(_number);

            if (isValid)
            {
                _marked = true;
                OnMarked();
            }
            else
            {
                if (_leanTweenId1 > -1) LeanTween.cancel(gameObject, _leanTweenId1);
                if (_leanTweenId2 > -1) LeanTween.cancel(_text.gameObject, _leanTweenId2);

                _leanTweenId1 = LeanTween.color(transform as RectTransform, Color.white,
                    GameData.Instance.WrongDaubDisappearTime).id;
                _leanTweenId2 = LeanTween.colorText(_text.transform as RectTransform, _pressedColor,
                    GameData.Instance.WrongDaubDisappearTime).id;
            }
        }

        private void Unmark()
        {
            if (_marked) return;

            _button.targetGraphic.color = Color.white;
            _text.color = _pressedColor;

            _marked = false;
            OnUnmarked();
        }

        public void EnableInput()
        {
            _button.interactable = true;
        }

        public void DisableInput()
        {
            _button.interactable = false;
        }

        private TMP_Text GetText()
        {
            if (_text == null)
            {
                _text = GetComponentInChildren<TMP_Text>();
            }

            return _text;
        }

        private Button GetButton()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }

            return _button;
        }

        #endregion
    }
}