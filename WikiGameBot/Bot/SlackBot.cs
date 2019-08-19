using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SlackAPI;


namespace WikiGameBot.Bot
{
    public class SlackBot
    {
        public SlackSocketClient _client { get; set; }

        List<WikiMessage> _wikiMessages = new List<WikiMessage>();

        public SlackBot(string Token)
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                throw new ArgumentNullException();
            }
            _client = new SlackSocketClient(Token);
        }

        public void Connect()
        {
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
                Console.WriteLine($"Msg Id: {message.id} To: {message.reply_to} Type: {message.type} SubType: {message.subtype} " +
                    $"Text: {message.text} TS: {message.ts} Thread TS: {message.thread_ts}");
                Console.WriteLine(message.ToString());
                // Handle each message as you receive them
                _wikiMessages.Add(new WikiMessage
                {
                    MessageId = message.id,
                    MessageType = message.type,
                    ThreadId = message.reply_to,
                    ThreadTs = message.thread_ts,
                    UserId = message.username,
                    MessageText = message.text,
                });
            };
            
            clientReady.Wait();

            _client.GetChannelList((clr) => { Console.WriteLine("got channels"); });
            var c = _client.Channels.Find(x => x.name.Equals("wikigame"));
            _client.PostMessage(x => Console.WriteLine(x.error),c.id,"Hello!");

            while (true) { };
        }


    }
}
