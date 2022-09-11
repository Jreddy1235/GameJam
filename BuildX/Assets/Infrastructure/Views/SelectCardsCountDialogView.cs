using BrilliantBingo.Code.Infrastructure.Events.Args;
using BrilliantBingo.Code.Infrastructure.Events.Handlers;
using BrilliantBingo.Code.Infrastructure.Layout;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrilliantBingo.Code.Infrastructure.Views
{
    public class SelectCardsCountDialogView : MonoBehaviour
    {
        #region Events

        public event CountOfCardsToPlaySelectedEventHandler CountOfCardsSeleced;
        private void OnCountOfCardsSelected(BingoCardsLayout layout)
        {
            var handler = CountOfCardsSeleced;
            if (handler == null) return;
            handler(this, new CountOfCardsToPlaySelectedEventArgs(layout));
        }

        #endregion

        #region Fields

        private const string DialogAppearanceAnimationTriggerName
            = "ShowSelectCardsCountDialogTrigger";

        [SerializeField]
        private Button _oneCardGameButton;

        [SerializeField]
        private Button _twoCardGameButton;

        [SerializeField]
        private Button _threeCardGameButton;

        [SerializeField]
        private Button _fourCardGameButton;

        [SerializeField] private TMP_Text txtOneCardCost;
        [SerializeField] private TMP_Text txtTwoCardCost;
        [SerializeField] private TMP_Text txtThreeCardCost;
        [SerializeField] private TMP_Text txtFourCardCost;
        
        private bool _initialized;

        #endregion

        #region Methods

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Initialize()
        {
            if (_initialized) return;
            _oneCardGameButton.onClick.AddListener(OnOneCardGameButtonClick);
            _twoCardGameButton.onClick.AddListener(OnTwoCardGameButtonClick);
            _threeCardGameButton.onClick.AddListener(OnThreeCardGameButtonClick);
            _fourCardGameButton.onClick.AddListener(OnFourCardGameButtonClick);
            txtOneCardCost.text = GameData.Instance.BingoOneCardCost.ToString();
            txtTwoCardCost.text = GameData.Instance.BingoTwoCardCost.ToString();
            txtThreeCardCost.text = GameData.Instance.BingoThreeCardCost.ToString();
            txtFourCardCost.text = GameData.Instance.BingoFourCardCost.ToString();
            _initialized = true;
        }
        
        private void OnOneCardGameButtonClick()
        {
            OnCountOfCardsSelected(BingoCardsLayout.SingleCard);
            HUDManager.Instance.TotalTickets.Value -= GameData.Instance.BingoOneCardCost;
        }

        private void OnTwoCardGameButtonClick()
        {
            OnCountOfCardsSelected(BingoCardsLayout.TwoCards);
            HUDManager.Instance.TotalTickets.Value -= GameData.Instance.BingoTwoCardCost;
        }

        private void OnThreeCardGameButtonClick()
        {
            OnCountOfCardsSelected(BingoCardsLayout.ThreeCards);
            HUDManager.Instance.TotalTickets.Value -= GameData.Instance.BingoThreeCardCost;
        }

        private void OnFourCardGameButtonClick()
        {
            OnCountOfCardsSelected(BingoCardsLayout.FourCards);
            HUDManager.Instance.TotalTickets.Value -= GameData.Instance.BingoFourCardCost;
        }

        #endregion
    }
}