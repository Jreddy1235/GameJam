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

        private float _ballGenerationFrequency = 3f;

        [SerializeField]
        private ReadySteadyGoView _readySteadyGoView;

        [SerializeField] private GameObject _roundOverText;
        [SerializeField] private GameObject _restartButton;

        #endregion

        #region Methods

        public void Awake()
        {
            _roundOverText.SetActive(false);
            _restartButton.SetActive(false);
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
            CoreGameObjectsLocator.Default.CardsLayoutManager.ClearCards();
            Invoke("ShowDialog", 1f);
        }

        private void ShowDialog()
        {
            CoreGameObjectsLocator.Default.DialogManager.ShowSelectCardsCountDialog(OnCountOfCardsSelected);
        }

        private void OnCountOfCardsSelected(BingoCardsLayout layout)
        {
            CoreGameObjectsLocator.Default.CardsFactory.CreateAndLayout(layout);
            CoreGameObjectsLocator.Default.CardsCollection.DisableAllCards();
            _readySteadyGoView.Show();
        }

        private void OnAllCardsFinishToPlay(object sender, AllCardsFinishToPlayEventArgs e)
        {
            _roundOverText.SetActive(true);
            Debug.Log("Game is over. Count of win cards: " + e.WinCardsCount);
            CoreGameObjectsLocator.Default.BingoBallsSource.Stop();
        }

        private void OnAllBingoBallsFinished()
        {
            Debug.Log("Game is over. Count of win cards: ");
            //CoreGameObjectsLocator.Default.CardsLayoutManager.SetVisibilityOfCards(false);
            _restartButton.SetActive(true);
            _roundOverText.SetActive(true);
        }

        private void OnGo(object sender, EventArgs e)
        {
            CoreGameObjectsLocator.Default.CardsCollection.EnableAllCards();
            CoreGameObjectsLocator.Default.BingoBallsSource.Begin(_ballGenerationFrequency);
        }

        #endregion
    }
}