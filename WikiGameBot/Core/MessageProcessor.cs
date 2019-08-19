﻿using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using System.Text;
using WikiGameBot.Bot;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Core
{
    

    public class MessageProcessor
    {
        private readonly IGameReaderWriter _gameReaderWriter;
        private readonly PageListExtractor _pageListExtractor;
        private readonly WikipediaLinkExtractor _wikipediaLinkExtractor;
        public MessageProcessor(IGameReaderWriter gameReaderWriter)
        {
            _gameReaderWriter = gameReaderWriter;
            _wikipediaLinkExtractor = new WikipediaLinkExtractor();
            _pageListExtractor = new PageListExtractor();
        }

        public PrintMessage ProcessMessage(NewMessage message)
        {
            if (CheckIfMessageIsANewGame(message))
            {
                Console.WriteLine("New Game Started");
                _gameReaderWriter.CreateNewGame(message);
            }
            else
            {
                var gameId = _gameReaderWriter.FindGameId(message);
                if (gameId != -1)
                {
                    if (message.text.ToLower().Trim() == "wiki-bot: stats")
                    {
                        var stats = _gameReaderWriter.GetGameStatistics(gameId);
                        return PrintStats(stats, gameId);
                    }
                    else
                    {
                        int linkCount = GenerateLinkCount(message);
                        // TODO: Username is showing as null occasionally
                        _gameReaderWriter.AddGameEntry(new GameEntry
                        {
                            LinkCount = linkCount,
                            User = message.user,
                            UserName = message.username,
                            GameId = gameId
                        });
                    }
                }

            }
            return null;
        }

        private PrintMessage PrintStats(GameStatistics stats, int gameId)
        {
            return new PrintMessage
            {
                IsReply = true,
                MessageText = $"{stats.CurrentWinner} is currently winning with a score of {stats.BestEntry}.\n" +
                    $">{stats.BestEntryMessage}",
                ThreadTs = _gameReaderWriter.GetThreadTs(gameId)
            };
        }

        private int GenerateLinkCount(NewMessage message)
        {
            List<string> pageList = _pageListExtractor.GeneratePageList(message.text);
            Console.WriteLine($"User: {message.user} #Links: {pageList.Count}");
            return pageList.Count;
        }

        private bool CheckIfMessageIsANewGame(NewMessage message)
        {
            List<string> wikipediaLinks = _wikipediaLinkExtractor.ExtractWikipediaLinks(message.text);
            return wikipediaLinks.Count == 2;
        }

    }
}