using Microsoft.Extensions.DependencyInjection;
using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using WikiGameBot.Bot;
using WikiGameBot.Core;
using WikiGameBot.Data.Loaders;
using WikiGameBot.Data.Loaders.Interfaces;
using Xunit;

namespace Tests
{
    public class MessageProcessorTests
    {
        public string _messageText { get; set; }
        public NewMessage _newMessage { get; set; }
        private readonly MessageProcessor _messageProcessor;

        public MessageProcessorTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IGameReaderWriter, MockGameReaderWriter>()
                .BuildServiceProvider();

            _messageProcessor = new MessageProcessor(serviceProvider.GetService<IGameReaderWriter>());
        }
        List<string> _links = new List<string>();


        private void WhenTheMessageIsProcessed()
        {
            _messageProcessor.ProcessMessage(_newMessage);
        }

        private void GivenTheDefaultGame()
        {
            GivenTheMessageText("Today's Challenge: https://en.wikipedia.org/wiki/Buddy_Guy -> https://en.wikipedia.org/wiki/Ramen");
            _messageProcessor.ProcessMessage(_newMessage);
        }

        private void GivenAMessageWithText(string messageText)
        {
            _newMessage = new NewMessage { text = messageText };
        }

        private void GivenTheMessageText(string messageText)
        {
            _messageText = messageText;
        }


        private void ThenTheLinksLookLike(List<string> links)
        {
            Assert.Equal(links, _links);
        }
    }
}
