using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using System.Text;
using WikiGameBot.Bot;
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
        /// Adds entry to game
        /// </summary>
        /// <param name="gameEntry"></param>
        LoaderResponse AddGameEntry(GameEntry gameEntry);

        /// <summary>
        /// Gets thread timestamp associated with <paramref name="gameId"/>
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        DateTime GetThreadTs(int gameId);

        /// <summary>
        /// Ends game associated with <paramref name="gameId"/>
        /// </summary>
        /// <param name="gameId"></param>
        PrintMessage EndGame(int gameId);

        /// <summary>
        /// Add player to database if it's their first time playing
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        void AddUserIfFirstTimePlaying(string UserId, string UserName);

        /// <summary>
        /// Increment win-count associated with player
        /// </summary>
        /// <param name="UserId"></param>
        void IncrementPlayerWinCount(string UserId);

        /// <summary>
        /// Increment entry-count associated with player speficied by <paramref name="UserId"/>
        /// </summary>
        /// <param name="UserId"></param>
        void IncrementPlayerEntryCount(string UserId);

        /// <summary>
        /// Generate stats for game specificed by <paramref name="gameId"/>
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        GameStatistics GetGameStatistics(int gameId);

        /// <summary>
        /// Returns best entry for game specificed by <paramref name="gameId"/>
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Entities.GameEntry GetWinningEntry(int gameId);
    }
}
