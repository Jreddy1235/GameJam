using System;
using BrilliantBingo.Code.Infrastructure.Core;
using BrilliantBingo.Code.Infrastructure.Events.Args;
using BrilliantBingo.Code.Infrastructure.Layout;
using BrilliantBingo.Code.Infrastructure.Views;
using UniRx;
using UnityEngine;

namespace BrilliantBingo.Code.Scripts
{
    public class GameController : MonoBehaviour
    {
        #region Fields

        public static event Action OnCardsCountSelected;
        public static event Action OnRestartTapped;
        public static event Action OnRoundOver;
        private float _ballGenerationFrequency = 3f;

        [SerializeField]
        private ReadySteadyGoView _readySteadyGoView;
        [SerializeField] private GameObject _generatedNumbersPanel;
        [SerializeField] private GameObject _restartButton;
        [SerializeField] private GameObject _restartAgainButton;

        private IDisposable _restartHandle;
        #endregion

        #region Methods

        public void Awake()
        {
            _generatedNumbersPanel.SetActive(false);
            _readySteadyGoView.Hide();
            _readySteadyGoView.Go += OnGo;

            CoreGameObjectsLocator.Default.CardsCollection.AllCardsFinishToPlay -= OnAllCardsFinishToPlay;
            CoreGameObjectsLocator.Default.CardsCollection.AllCardsFinishToPlay += OnAllCardsFinishToPlay;

            CoreGameObjectsLocator.Default.BingoBallsSource.OnAllBingoBallsFinished += OnAllBingoBallsFinished;
        }

        public void Start()
        {
            _restartAgainButton.SetActive(false);
            _restartButton.SetActive(false);
            Invoke("ShowDialog", 1f);
        }

        public void Restart()
        {
            _restartButton.SetActive(false);
            _restartAgainButton.SetActive(true);
            _restartHandle = Observable.ReturnUnit()
                .Delay(TimeSpan.FromSeconds(GameData.Instance.RestartAgainDuration))
                .Subscribe(_ =>
                {
                    _restartButton.SetActive(true);
                    _restartAgainButton.SetActive(false);
                });
        }
        
        public void RestartAgain()
        {
            _restartHandle?.Dispose();
            OnRestartTapped?.Invoke();
            CoreGameObjectsLocator.Default.CardsLayoutManager.ClearCards();
            CoreGameObjectsLocator.Default.BingoBallsSource.Restart();
            CoreGameObjectsLocator.Default.CardsCollection.Restart();
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
            _restartButton.SetActive(true);

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
            OnRoundOver?.Invoke();
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