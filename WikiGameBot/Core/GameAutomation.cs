using System;
using System.Collections.Generic;
using System.Text;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Core
{
    public class GameAutomation
    {

        private readonly IGameReaderWriter _gameReaderWriter;

        public GameAutomation(IGameReaderWriter gameReaderWriter)
        {
            _gameReaderWriter = gameReaderWriter;
        }

        /// <summary>
        /// Finds all active games and ends every game
        /// </summary>
        public void EndOldGames()
        {
            var activeGames = _gameReaderWriter.GetActiveGames();
            foreach (var activeGame in activeGames)
                _gameReaderWriter.EndGame(activeGame.Id);           
        }
    }
}
