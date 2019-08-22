using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SlackAPI;
using WikiGameBot.Core;
using WikiGameBot.Core.LeaderBoard;
using WikiGameBot.Data.Loaders;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Bot
{
    public class SlackBot
    {
        public SlackSocketClient _client { get; set; }

        private IDBServerInfoLoader _dBServerInfoLoader;
        private IGameReaderWriter _gameReaderWriter;
        private readonly LeaderBoardGenerator _leaderBoardGenerator;
        List<WikiMessage> _wikiMessages = new List<WikiMessage>();

        public SlackBot(IGameReaderWriter gameReaderWriter, IDBServerInfoLoader dBServerInfoLoader)
        {
            _gameReaderWriter = gameReaderWriter;
            _dBServerInfoLoader = dBServerInfoLoader;
            _leaderBoardGenerator = new LeaderBoardGenerator(_gameReaderWriter);
        }

        public async Task Connect()
        {
            Console.WriteLine("Initializing Client");
            string token = Environment.GetEnvironmentVariable("WIKI_BOT_USER_OATH_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException();
            }
            _client = new SlackSocketClient(token);

            MessageProcessor processor = new MessageProcessor(_gameReaderWriter);
            Console.WriteLine("RTM Client Connecting...");
            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);

            _gameReaderWriter.TestDatabaseConnection();

            _client.Connect((connected) => {
                // This is called once the client has emitted the RTM start command
                clientReady.Set();
            }, () => {
                // This is called once the RTM client has connected to the end point
            });
            _client.OnMessageReceived += async (message) =>
            {
                // Ignore Bots
                if (message.user != null)
                {
                    // Overwrite username with poster's real name
                    User poster = new User();
                    _client.UserLookup.TryGetValue(message.user, out poster);
                    Console.WriteLine(poster.real_name);
                    message.username = poster.real_name;

                    var res = await processor.ProcessMessage(message);
                    if (res != null)
                    {
                        var chan = _client.Channels.Find(x => x.name.Equals("wiki-game-test-chan"));
                        if (res.ThreadTs.HasValue)
                        {
                            var thread_ts = res.ThreadTs.Value.ToProperTimeStamp();
                            _client.PostMessage(x => Console.WriteLine(res), chan.id, res.MessageText, thread_ts: thread_ts);
                        }
                        else
                        {
                            _client.PostMessage(x => Console.WriteLine(res), chan.id, res.MessageText);
                        }
                    }
                }

            };
            
            clientReady.Wait();

            // Send heartbeat
            var c = _client.Channels.Find(x => x.name.Equals("wiki-game-test-chan"));
            _client.PostMessage(x => Console.WriteLine(x.error), c.id, "Hello! Enter in two wikipedia links to get started!\n" +
                "Afterwards, reply to that post in the formal \"Starting Page -> Click 1 -> Click 2 -> ... -> Ending Page");

            // Generate Leadboard
            var leaderboardString = _leaderBoardGenerator.GenerateLeaderBoardString();
            if (string.IsNullOrEmpty(leaderboardString) == false) { 
                _client.PostMessage(x => Console.WriteLine(x.error), c.id, $"{leaderboardString}\nType \"wiki-bot: leaderboard\" to bring back the leader board at any time.");
            }

            while (true) { };
        }
    }
}
