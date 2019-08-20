using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WikiGameBot.Bot;
using WikiGameBot.Data.Loaders;
using WikiGameBot.Data.Loaders.Interfaces;

namespace WikiGameBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Depenecy Injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IGameReaderWriter, MockGameReaderWriter>()
                .AddSingleton<IDBServerInfoLoader, AWSRDSInfoLoader>()
                .BuildServiceProvider();

            // Game
            string token = Environment.GetEnvironmentVariable("WIKI_BOT_USER_OATH_TOKEN");
            SlackBot bot = new SlackBot(token,serviceProvider);
            bot.Connect();
        }
    }
}
