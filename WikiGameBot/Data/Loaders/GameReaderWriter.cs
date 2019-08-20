﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlackAPI.WebSocketMessages;
using WikiGameBot.Core;
using WikiGameBot.Data.Entities;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Data.Loaders
{
    public class GameReaderWriter : IGameReaderWriter
    {
        private readonly WikiBotDataDbContext _context;

        public GameReaderWriter(WikiBotDataDbContext context)
        {
            _context = context;
        }

        public void AddGameEntry(Core.GameEntry gameEntry)
        {
            // TODO Add method of feedback if user has allready entered
            if (CheckIfUserHasPlayed(gameEntry.User, gameEntry.GameId) == false)
            {
                AddUserIfFirstTimePlaying(gameEntry.User, gameEntry.UserName);
                Console.WriteLine($"User has not played; Adding entry...");
                Console.WriteLine($"{gameEntry.ToString()}");
                // Prepare Data for writing
                Entities.GameEntry newGameEntry = new Entities.GameEntry();
                newGameEntry.GameId = gameEntry.GameId;
                newGameEntry.LinkCount = gameEntry.LinkCount;
                newGameEntry.RawText = gameEntry.RawText;
                newGameEntry.UserId = gameEntry.User;
                newGameEntry.UserName = gameEntry.UserName;

                // Write changes to database
                _context.GameEntries.Add(newGameEntry);
                _context.SaveChanges();
            }

        }
     
        public void AddUserIfFirstTimePlaying(string UserId, string UserName)
        {
            var matchingPlayer = _context.Players.Where(x => x.SlackUserId == UserId);
            if(matchingPlayer==null || matchingPlayer.Count() == 0)
            {
                Player newPlayer = new Player();
                newPlayer.SlackUserId = UserId;
                newPlayer.SlackUserName = UserName;
                newPlayer.NumberOfEntries = 0;
                newPlayer.NumberOfWins = 0;

                _context.Players.Add(newPlayer);
                _context.SaveChanges();
            }
        }

        private bool CheckIfUserHasPlayed(string UserId, int GameId)
        {
            Console.WriteLine($"Checking if {UserId} has played on game {GameId}");
            var matchingEntries = _context.GameEntries.Where(x => x.GameId == GameId && x.UserId == UserId);
            if (matchingEntries != null && matchingEntries.Count()>0)
            {
                return true;
            }
            return false;
        }
        public void CreateNewGame(NewMessage message)
        {
            // Prepare data for writing
            Console.WriteLine("Preparing new game data...");
            Game newGame = new Game();
            newGame.ThreadTimeStamp = message.ts;
            newGame.IsActive = true;

            // Write changes to database
            Console.WriteLine("Saving data...");
            _context.Games.Add(newGame);
            _context.SaveChanges();
            Console.WriteLine("Game Written and started.");
        }

        public int FindGameId(NewMessage message)
        {
            Console.WriteLine($"Checking if game exists on current thread (thread_ts={message.thread_ts})");
            var game = _context.Games.Where(x => x.ThreadTimeStamp == message.thread_ts)
                                     .FirstOrDefault();
            if (game != null)
            {
                return game.Id;
            }
            else
            {
                Console.WriteLine($"Game does not exist; A value of -1 will be returned.");
                return -1;
            }
        }

        public GameStatistics GetGameStatistics(int gameId)
        {
            throw new NotImplementedException();
        }

        public DateTime GetThreadTs(int gameId)
        {
            return _context.Games.Where(x => x.Id == gameId).Select(x => x.ThreadTimeStamp).FirstOrDefault();
        }
    }
}
