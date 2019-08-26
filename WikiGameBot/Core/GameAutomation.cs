using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Core
{
    public class GameAutomation
    {

        private readonly IGameReaderWriter _gameReaderWriter;
        public DateTime? _lastCheckTime { get; set; }
        public GameAutomation(IGameReaderWriter gameReaderWriter)
        {
            _lastCheckTime = null;
            _gameReaderWriter = gameReaderWriter;
        }

        public async Task RunAutomatedTasksAsync()
        {
            if (_lastCheckTime == null || (DateTime.Now - _lastCheckTime).Value.TotalMinutes > 60.0)
            {
                _lastCheckTime = DateTime.Now;
                await EndOldGames();
            }
        }

        /// <summary>
        /// Finds all active games and ends every game that is 1 day old or older
        /// </summary>
        private async Task EndOldGames()
        {
            var activeGames = await _gameReaderWriter.GetActiveGamesAsync();
            foreach (var activeGame in activeGames)
            {
                var elapsedTime = DateTime.Now - activeGame.ThreadTimeStamp;
                if (elapsedTime.TotalDays >= 1.0)
                    _gameReaderWriter.EndGame(activeGame.Id);
            }
                         
        }
    }
}
