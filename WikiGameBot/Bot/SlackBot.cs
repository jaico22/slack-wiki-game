using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SlackAPI;
using WikiGameBot.Core;
using WikiGameBot.Data.Loaders;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot.Bot
{
    public class SlackBot
    {
        public SlackSocketClient _client { get; set; }

        private IDBServerInfoLoader _dBServerInfoLoader;
        private IGameReaderWriter _gameReaderWriter;

        List<WikiMessage> _wikiMessages = new List<WikiMessage>();

        public SlackBot(IGameReaderWriter gameReaderWriter, IDBServerInfoLoader dBServerInfoLoader)
        {
            _gameReaderWriter = gameReaderWriter;
            _dBServerInfoLoader = dBServerInfoLoader;
        }

        public void Connect()
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
            _client.OnMessageReceived += (message) =>
            {
                // Ignore Bots
                if (message.user != null)
                {
                    // Overwrite username with poster's real name
                    User poster = new User();
                    _client.UserLookup.TryGetValue(message.user, out poster);
                    Console.WriteLine(poster.real_name);
                    message.username = poster.real_name;

                    var res = processor.ProcessMessage(message);
                    if (res != null)
                    {
                        var chan = _client.Channels.Find(x => x.name.Equals("wikigame"));
                        var thread_ts = res.ThreadTs.Value.ToProperTimeStamp();
                        _client.PostMessage(x => Console.WriteLine(res), chan.id, res.MessageText, thread_ts: thread_ts);
                    }
                }

            };
            
            clientReady.Wait();

            // Send heartbeat
            var c = _client.Channels.Find(x => x.name.Equals("wikigame"));
            _client.PostMessage(x => Console.WriteLine(x.error), c.id, "Hello! Enter in two wikipedia links to get started!\n" +
                "Afterwards, reply to that post in the formal \"Starting Page -> Click 1 -> Click 2 -> ... -> Ending Page\"\n\n" +
                "What's new since last time:\n" +
                "   - Stats have been implemented: Type \"wiki-bot stats\" to view whos winning the current game\n" +
                "   - More feedback, so you'll know when the bot is working" );


            while (true) { };
        }

        


    }
}
