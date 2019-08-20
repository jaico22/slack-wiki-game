using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WikiGameBot.Bot;
using WikiGameBot.Data;
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
                .AddDbContext<WikiBotDataDbContext>()
                .AddTransient<IGameReaderWriter, GameReaderWriter>()
                .AddTransient<IDBServerInfoLoader, AWSRDSInfoLoader>()
                .AddTransient<SlackBot>()
                .BuildServiceProvider();
            serviceProvider.GetService<SlackBot>().Connect();
        }
    }
}
