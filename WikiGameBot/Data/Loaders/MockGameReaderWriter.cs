using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlackAPI.WebSocketMessages;
using WikiGameBot.Core;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Data.Loaders
{
    public class MockGameReaderWriter : IGameReaderWriter
    {
        public DateTime _mockGameThreadTs { get; set; }
        private List<Entities.GameEntry> gameEntries = new List<Entities.GameEntry>();
        public int FindGameId(NewMessage message)
        {
            Console.WriteLine($"Checking Game: ThreadTs={message.thread_ts}");
            if (message.thread_ts == _mockGameThreadTs)
            {
                Console.WriteLine("Game Found");
                return 1;
            }
            return -1;
        }

        public void CreateNewGame(NewMessage message)
        {
            Console.WriteLine($"New Game Create: Ts={message.ts}");
            gameEntries.Clear();
            _mockGameThreadTs = message.ts;
        }

        public void AddGameEntry(GameEntry gameEntry)
        {
            Entities.GameEntry newGameEntry = new Entities.GameEntry();
            newGameEntry.GameId = gameEntry.GameId;
            newGameEntry.LinkCount = gameEntry.LinkCount;
            newGameEntry.RawText = gameEntry.RawText;
            newGameEntry.UserId = gameEntry.User;
            newGameEntry.UserName = gameEntry.UserName;

            gameEntries.Add(newGameEntry);
        }

        public DateTime GetThreadTs(int gameId)
        {
            return _mockGameThreadTs;
        }

        public GameStatistics GetGameStatistics(int gameId)
        {
            GameStatistics gameStatistics = new GameStatistics();

            var bestEntry = gameEntries.OrderBy(x => x.LinkCount)
                                       .FirstOrDefault();

            var worstEntry = gameEntries.OrderByDescending(x => x.LinkCount)
                                        .FirstOrDefault();

            gameStatistics.BestEntry = bestEntry.LinkCount;
            gameStatistics.WorstEntry = worstEntry.LinkCount;
            gameStatistics.BestEntryMessage = bestEntry.RawText;
            gameStatistics.WorstEntryMessage = worstEntry.RawText;
            gameStatistics.CurrentWinner = bestEntry.UserName;

            return gameStatistics;
        }

        public void AddUserIfFirstTimePlaying(string UserId, string UserName)
        {
            // Not needed in mockup
        }

        public void IncrementPlayerWinCount(string UserId)
        {
            // Not needed in mockup
        }

        public void IncrementPlayerEntryCount(string UserId)
        {
            // Not needed in mockup
        }

        public Entities.GameEntry GetWinningEntry(int gameId)
        {
            return gameEntries.Where(x => x.GameId == gameId).OrderBy(x => x.LinkCount).FirstOrDefault();
        }
    }
}
