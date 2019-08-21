using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using System.Text;
using WikiGameBot.Bot;
using WikiGameBot.Data.Loaders;
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
                return new PrintMessage { IsReply = true, MessageText = $"New Game Started! To end game, type \n> wiki-bot: end game", ThreadTs = message.ts };
            }
            else
            {
                var gameId = _gameReaderWriter.FindGameId(message);
                if (gameId != -1)
                {
                    switch (message.text.ToLower().Trim())
                    {
                        case "wiki-bot stats":
                        case "wiki-bot: stats":
                            var stats = _gameReaderWriter.GetGameStatistics(gameId);
                            return PrintStats(stats, gameId);
                        case "wiki-bot end":
                        case "wiki-bot end game":
                        case "wiki-bot endgame":
                        case "wiki-bot: end":
                        case "wiki-bot: end game":
                        case "wiki-bot: endgame":
                            return _gameReaderWriter.EndGame(gameId);
                        default:
                            break;
                    }

                    int linkCount = GenerateLinkCount(message);

                    // Only process valid messages (e.g. ignore a normal conversation)
                    if (linkCount > 0)
                    {
                        var responce = _gameReaderWriter.AddGameEntry(new GameEntry
                        {
                            LinkCount = linkCount,
                            User = message.user,
                            RawText = message.text,
                            UserName = message.username,
                            GameId = gameId
                        });

                        if (responce == LoaderResponse.Success)
                        {
                            return new PrintMessage { IsReply = true, MessageText = $"{message.username}'s Entry Received! Number of clicks: {linkCount}", ThreadTs = message.thread_ts };
                        }
                        else
                        {
                            return new PrintMessage { IsReply = true, MessageText = $"You have already played this round. You only get one chance per game", ThreadTs = message.thread_ts };
                        }

                    }

                }

            }
            return null;
        }

        private PrintMessage PrintStats(GameStatistics stats, int gameId)
        {
            if (stats != null)
            {
                return new PrintMessage
                {
                    IsReply = true,
                    MessageText = $"{stats.CurrentWinner} is currently winning with a score of {stats.BestEntry}.\n" +
                        $">{stats.BestEntryMessage}",
                    ThreadTs = _gameReaderWriter.GetThreadTs(gameId)
                };
            }
            else
            {
                return new PrintMessage
                {
                    IsReply = true,
                    MessageText = "No one has played yet, stats can't get generated",
                    ThreadTs = _gameReaderWriter.GetThreadTs(gameId)
                };
            }

        }

        private int GenerateLinkCount(NewMessage message)
        {
            List<string> pageList = _pageListExtractor.GeneratePageList(message.text);
            Console.WriteLine($"User: {message.user} #Links: {pageList.Count}");
            return pageList.Count-1;
        }

        private bool CheckIfMessageIsANewGame(NewMessage message)
        {
            List<string> wikipediaLinks = _wikipediaLinkExtractor.ExtractWikipediaLinks(message.text);
            return wikipediaLinks.Count == 2;
        }

    }
}
