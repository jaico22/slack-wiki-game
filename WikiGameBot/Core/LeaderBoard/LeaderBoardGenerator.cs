using System;
using System.Collections.Generic;
using System.Text;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Core.LeaderBoard
{
    public class LeaderBoardGenerator
    {
        private readonly IGameReaderWriter _gameReaderWriter;

        public LeaderBoardGenerator(IGameReaderWriter gameReaderWriter)
        {
            _gameReaderWriter = gameReaderWriter;
        }

        public string GenerateLeaderBoardString()
        {
            string leaderBoardString = $"Here are the current top players\n";

            var leaderBoard = GetLeaderBoard();

            if (leaderBoard == null)
            {
                return null;
            }

            foreach (var row in leaderBoard)
            {
                leaderBoardString += $"*#{row.Position}: {row.PlayerName}* _Wins: {row.NumberOfWins} Entries: {row.NumerOfEntries} Win Percentage: {row.WinPercentage*100}%_\n";
            }

            return leaderBoardString;
        }

        public List<LeaderBoardRow> GetLeaderBoard()
        {
            List<LeaderBoardRow> leaderBoard = new List<LeaderBoardRow>();
            var leaders = _gameReaderWriter.GetLeaderBoard(10);
            int position = 1;
            if (leaders != null)
            {
                foreach (var leader in leaders)
                {
                    if(leader.NumberOfEntries != 0)
                    {
                        leaderBoard.Add(new LeaderBoardRow
                        {
                            Position = position++,
                            PlayerName = leader.SlackUserName,
                            NumberOfWins = leader.NumberOfWins,
                            NumerOfEntries = leader.NumberOfEntries,
                            WinPercentage = (decimal)leader.NumberOfWins / (decimal)leader.NumberOfEntries
                        });
                    }
                }
                return leaderBoard;
            }
            else
            {
                return null;
            }
        }
    }
}
