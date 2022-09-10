using System;
using BrilliantBingo.Code.Infrastructure.Core;
using BrilliantBingo.Code.Infrastructure.Events.Args;
using BrilliantBingo.Code.Infrastructure.Layout;
using BrilliantBingo.Code.Infrastructure.Views;
using UnityEngine;

namespace BrilliantBingo.Code.Scripts
{
    public class GameController : MonoBehaviour
    {
        #region Fields

        public static event Action OnCardsCountSelected;
        private float _ballGenerationFrequency = 3f;

        [SerializeField]
        private ReadySteadyGoView _readySteadyGoView;
        [SerializeField] private GameObject _generatedNumbersPanel;
        [SerializeField] private GameObject _roundOverText;
        [SerializeField] private GameObject _restartButton;

        #endregion

        #region Methods

        public void Awake()
        {
            _roundOverText.SetActive(false);
            _restartButton.SetActive(false);
            _generatedNumbersPanel.SetActive(false);
            _readySteadyGoView.Hide();
            _readySteadyGoView.Go += OnGo;

            CoreGameObjectsLocator.Default.CardsCollection.AllCardsFinishToPlay -= OnAllCardsFinishToPlay;
            CoreGameObjectsLocator.Default.CardsCollection.AllCardsFinishToPlay += OnAllCardsFinishToPlay;

            CoreGameObjectsLocator.Default.BingoBallsSource.OnAllBingoBallsFinished += OnAllBingoBallsFinished;
        }

        public void Start()
        {
            _restartButton.SetActive(false);
            _roundOverText.SetActive(false);
            Invoke("ShowDialog", 1f);
        }

        public void Restart()
        {
            CoreGameObjectsLocator.Default.CardsLayoutManager.ClearCards();
            CoreGameObjectsLocator.Default.BingoBallsSource.Restart();
            CoreGameObjectsLocator.Default.GeneratedNumbersManager.ResetNumbers();
            Start();
        }
        private void ShowDialog()
        {
            CoreGameObjectsLocator.Default.DialogManager.ShowSelectCardsCountDialog(OnCountOfCardsSelected);
        }

        private void OnCountOfCardsSelected(BingoCardsLayout layout)
        {
            CoreGameObjectsLocator.Default.CardsFactory.CreateAndLayout(layout);
            CoreGameObjectsLocator.Default.CardsCollection.DisableAllCards();
            _generatedNumbersPanel.SetActive(true);
            _readySteadyGoView.Show();
            OnCardsCountSelected?.Invoke();
        }

        private void OnAllCardsFinishToPlay(object sender, AllCardsFinishToPlayEventArgs e)
        {
            Debug.Log("Game is over. Count of win cards: " + e.WinCardsCount);
            RoundOver();
        }

        private void OnAllBingoBallsFinished()
        {
            Debug.Log("Game is over. All Numbers are called");
            RoundOver();
        }

        private void RoundOver()
        {
            _restartButton.SetActive(true);
            _roundOverText.SetActive(true);
            _readySteadyGoView.Hide();
            CoreGameObjectsLocator.Default.BingoBallsSource.Stop();
            CoreGameObjectsLocator.Default.CardsCollection.ClearCollection();
        }

        private void OnGo(object sender, EventArgs e)
        {
            CoreGameObjectsLocator.Default.CardsCollection.EnableAllCards();
            CoreGameObjectsLocator.Default.BingoBallsSource.Begin(_ballGenerationFrequency);
        }

        #endregion
    }
}