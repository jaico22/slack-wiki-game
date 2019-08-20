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

        /// <summary>
        /// Add player to database if it's their first time playing
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        void AddUserIfFirstTimePlaying(string UserId, string UserName);

        
        /// <summary>
        /// Generate stats for current game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        GameStatistics GetGameStatistics(int gameId);
    }
}
