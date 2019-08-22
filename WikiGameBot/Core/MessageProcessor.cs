using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiGameBot.Bot;
using WikiGameBot.Core.LeaderBoard;
using WikiGameBot.Core.PathValidation;
using WikiGameBot.Data.Entities;
using WikiGameBot.Data.Loaders;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Core
{
    

    public class MessageProcessor
    {
        private readonly IGameReaderWriter _gameReaderWriter;
        private readonly PageListExtractor _pageListExtractor;
        private readonly WikipediaLinkExtractor _wikipediaLinkExtractor;
        private readonly PathValidator _pathValidator;
        private readonly LeaderBoardGenerator _leaderBoardGenerator;

        public MessageProcessor(IGameReaderWriter gameReaderWriter)
        {
            _gameReaderWriter = gameReaderWriter;
            _wikipediaLinkExtractor = new WikipediaLinkExtractor();
            _pageListExtractor = new PageListExtractor();
            _leaderBoardGenerator = new LeaderBoardGenerator(_gameReaderWriter);
            _pathValidator = new PathValidator();
        }

        public async Task<PrintMessage> ProcessMessage(NewMessage message)
        {
            // Check Global Commands
            switch (message.text.ToLower().Trim())
            {
                case "wiki-bot: leaders":
                case "wiki-bot leaders":
                case "wiki-bot: leaderboard":
                case "wiki-bot leaderboard":
                case "wiki-bot leader board":
                case "wiki-bot: leader board":
                    var leaderboardString = _leaderBoardGenerator.GenerateLeaderBoardString();
                    if (string.IsNullOrEmpty(leaderboardString))
                    {
                        return new PrintMessage { IsReply = false, MessageText = "No players are playering; Cannot generate leaderboard" };
                    }
                    else
                    {
                        return new PrintMessage { IsReply = false, MessageText = leaderboardString };
                    }
            }

            // Check if a new game has started
            GameStartData gameStartData = GetGameStartData(message);
            if (gameStartData.IsValid)
            {
                Console.WriteLine("New Game Started");
                _gameReaderWriter.CreateNewGame(gameStartData);
                return new PrintMessage { IsReply = true, MessageText = $"New Game Started! To end game, type \n> wiki-bot: end game", ThreadTs = message.ts };
            }
            else
            {
                var game = _gameReaderWriter.GetGame(message);

                if (game != null)
                {
                    switch (message.text.ToLower().Trim())
                    {
                        case "wiki-bot stats":
                        case "wiki-bot: stats":
                            var stats = _gameReaderWriter.GetGameStatistics(game.Id);
                            return PrintStats(stats, game.Id);
                        case "wiki-bot end":
                        case "wiki-bot end game":
                        case "wiki-bot endgame":
                        case "wiki-bot: end":
                        case "wiki-bot: end game":
                        case "wiki-bot: endgame":
                            return _gameReaderWriter.EndGame(game.Id);
                        default:
                            break;
                    }

                    var pathValidationOutput = await FindAndProcessPath(game, message);

                    // Only process valid messages as determined by validator output
                    if (pathValidationOutput!=null && pathValidationOutput.IsValid)
                    {
                        var responce = _gameReaderWriter.AddGameEntry(new GameEntry
                        {
                            LinkCount = pathValidationOutput.PathLength,
                            User = message.user,
                            RawText = message.text,
                            UserName = message.username,
                            GameId = game.Id
                        });

                        if (responce == LoaderResponse.Success)
                        {
                            return new PrintMessage { IsReply = true, MessageText = $"{message.username}'s Entry Received! Number of clicks: {pathValidationOutput.PathLength}", ThreadTs = message.thread_ts };
                        }
                        else
                        {
                            return new PrintMessage { IsReply = true, MessageText = $"You have already played this round. You only get one chance per game", ThreadTs = message.thread_ts };
                        }
                    }
                    else
                    {
                        if (pathValidationOutput != null)
                        {
                            return new PrintMessage { IsReply = true, MessageText = pathValidationOutput.ValidationMessage, ThreadTs = message.thread_ts };
                        }
                    }

                }
            }
            return null;
        }

        private async Task<ValidationData> FindAndProcessPath(Game game, NewMessage message)
        {
            var path = _pageListExtractor.GeneratePageList(message.text);
            return await _pathValidator.Validate(game, path);
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

        /// <summary>
        /// Processes message and determine if a new game should be started. 
        /// Metadata related to the game is stored in the 'GameStartData' Object
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private GameStartData GetGameStartData(NewMessage message)
        {
            List<string> wikipediaLinks = _wikipediaLinkExtractor.ExtractWikipediaLinks(message.text);
            if (wikipediaLinks.Count == 2)
            {
                return new GameStartData { IsValid = true, StartingUrl = wikipediaLinks[0], EndingUrl = wikipediaLinks[1],  ThreadTs = message.ts};
            }
            return new GameStartData { IsValid = false };
        }

    }
}
