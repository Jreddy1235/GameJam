using System;
using BrilliantBingo.Code.Infrastructure.Events.Args;
using BrilliantBingo.Code.Infrastructure.Events.Handlers;
using BrilliantBingo.Code.Infrastructure.Generators.Interfaces;
using BrilliantBingo.Code.Infrastructure.Models;
using BrilliantBingo.Code.Scripts;
using UnityEngine;

namespace BrilliantBingo.Code.Infrastructure.Generators
{
    public class BingoBallsSource : MonoBehaviour, IBingoBallsSource
    {
        #region Fields

        private BingoBallGenerator _bingoBallGenerator;

        private bool _enabled;

        #endregion

        #region Methods

        public void Awake()
        {
            GameController.OnCardsCountSelected += StartBallGenerator;
        }

        private void StartBallGenerator()
        {
            _bingoBallGenerator = new BingoBallGenerator();
            _enabled = true;
        }

        private void RequestNextBingoBall()
        {
            if (!_enabled) return;
            var ball = _bingoBallGenerator.GenerateRandomBingoBall();
            OnBingoBallGenerated(ball);
        }

        #endregion

        #region IBingoBallsSource

        #region Events

        public event BingoBallGeneratedEventHandler BingoBallGenerated;
        public event Action OnAllBingoBallsFinished;
        public event Action OnAllBingoBallsRestart;

        private void OnBingoBallGenerated(BingoBall ball)
        {
            if (ball == null)
            {
                OnAllBingoBallsFinished?.Invoke();
                Stop();
                return;
            }
            var handler = BingoBallGenerated;
            if (handler == null) return;
            handler(this, new BingoBallGeneratedEventArgs(ball));
        }

        #endregion

        #region Methods

        public void Begin(float frequency)
        {
            InvokeRepeating("RequestNextBingoBall", 0.1f, frequency);
            
        }

        public void Restart()
        {
            OnAllBingoBallsRestart?.Invoke();
            Stop();
        }
        public void Stop()
        {
            _enabled = false;
            gameObject.SetActive(false);
            CancelInvoke("RequestNextBingoBall");
        }

        #endregion

        #endregion
    }
}