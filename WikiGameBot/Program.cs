using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WikiGameBot.Bot;

namespace WikiGameBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = Environment.GetEnvironmentVariable("WIKI_BOT_USER_OATH_TOKEN");
            SlackBot bot = new SlackBot(token);
            bot.Connect();
        }
    }
}
