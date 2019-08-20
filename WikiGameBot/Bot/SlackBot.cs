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

        public SlackBot(string Token,ServiceProvider services)
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                throw new ArgumentNullException();
            }
            _client = new SlackSocketClient(Token);
            _gameReaderWriter = services.GetService<IGameReaderWriter>();
            _dBServerInfoLoader = services.GetService<IDBServerInfoLoader>();
        }

        public void Connect()
        {
            Console.WriteLine(_dBServerInfoLoader.GetConnectionString());

            MessageProcessor processor = new MessageProcessor(_gameReaderWriter);
            Console.WriteLine("RTM Client Connecting...");
            ManualResetEventSlim clientReady = new ManualResetEventSlim(false);
            
            _client.Connect((connected) => {
                // This is called once the client has emitted the RTM start command
                clientReady.Set();
            }, () => {
                // This is called once the RTM client has connected to the end point
            });
            _client.OnMessageReceived += (message) =>
            {
                var res = processor.ProcessMessage(message);
                if (res != null)
                {
                    var chan = _client.Channels.Find(x => x.name.Equals("wikigame"));
                    var thread_ts = res.ThreadTs.Value.ToProperTimeStamp();
                    _client.PostMessage(x => Console.WriteLine(res), chan.id, res.MessageText, thread_ts: thread_ts);
                }
            };
            
            clientReady.Wait();

            // Send heartbeat
            var c = _client.Channels.Find(x => x.name.Equals("wikigame"));
            _client.PostMessage(x => Console.WriteLine(x.error), c.id, "Hello! Enter in two wikipedia links to get started!\n" +
                "Afterwards, reply to that post in the formal \"Start -> Click 1 -> Click 2 -> ... -> End\"\n" +
                "Type \"wiki-bot: stats\" for records on the current game\n\n" +
                "Note: This is the proof of concept version; Starting a new game will reset the client");


            while (true) { };
        }

        


    }
}
