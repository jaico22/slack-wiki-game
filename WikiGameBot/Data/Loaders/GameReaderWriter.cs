using System;
using System.Collections.Generic;
using System.Text;
using SlackAPI.WebSocketMessages;
using WikiGameBot.Core;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Data.Loaders
{
    public class GameReaderWriter : IGameReaderWriter
    {
        public void AddGameEntry(GameEntry gameEntry)
        {
            throw new NotImplementedException();
        }

        public void CreateNewGame(NewMessage message)
        {
            throw new NotImplementedException();
        }

        public int FindGameId(NewMessage message)
        {
            throw new NotImplementedException();
        }

        public GameStatistics GetGameStatistics(int gameId)
        {
            throw new NotImplementedException();
        }

        public DateTime GetThreadTs(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}
