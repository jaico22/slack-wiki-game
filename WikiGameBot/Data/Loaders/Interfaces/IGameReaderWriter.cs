using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using System.Text;
using WikiGameBot.Core;

namespace WikiGameBot.Data.Loaders.Interfaces
{
    public interface IGameReaderWriter
    {
        /// <summary>
        /// Returns ID related to thread with existing timestamp
        /// </summary>
        /// <param name="message"></param>
        /// <returns>
        ///     -1 = Game Not Found
        ///     Otherwise = Id associated with game
        /// </returns>
        int FindGameId(NewMessage message);

        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <param name="message"></param>
        void CreateNewGame(NewMessage message);

        /// <summary>
        /// Adds enntry to game
        /// </summary>
        /// <param name="gameEntry"></param>
        void AddGameEntry(GameEntry gameEntry);

        /// <summary>
        /// Gets thread timestamp associated with <see cref="gameId"/>
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        DateTime GetThreadTs(int gameId);

        GameStatistics GetGameStatistics(int gameId);
    }
}
