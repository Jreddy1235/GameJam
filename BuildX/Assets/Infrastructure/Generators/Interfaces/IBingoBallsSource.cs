using System;
using BrilliantBingo.Code.Infrastructure.Events.Handlers;

namespace BrilliantBingo.Code.Infrastructure.Generators.Interfaces
{
    public interface IBingoBallsSource
    {
        #region Events

        event BingoBallGeneratedEventHandler BingoBallGenerated;
        event Action OnAllBingoBallsFinished; 
        event Action OnAllBingoBallsRestart;

        #endregion

        #region Methods

        void Begin(float frequency);

        void Stop();

        void Restart();

        #endregion
    }
}