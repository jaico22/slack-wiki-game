using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using WikiGameBot.Bot;
using Xunit;

namespace Tests
{
    public class MessageProcessorTests
    {
        public string _messageText { get; set; }
        public NewMessage _newMessage { get; set; }
        private readonly MessageProcessor _messageProcessor = new MessageProcessor();
        List<string> _links = new List<string>();

        [Fact]
        public void NewGameCanBeDetected()
        {
            GivenAMessageWithText("Today's Challenge: https://en.wikipedia.org/wiki/Buddy_Guy -> https://en.wikipedia.org/wiki/Ramen");
            ThenTheMessageProcessorReturnNewGame();
        }

        [Fact]
        public void WikipediaLinksCanBeDetected()
        {
            GivenTheMessageText("Today's Challenge: https://en.wikipedia.org/wiki/Buddy_Guy -> https://en.wikipedia.org/wiki/Ramen");
            WhenLinksAreGeneratedOnMessageText();
            ThenTheLinksLookLike(new List<string>() { "https://en.wikipedia.org/wiki/Buddy_Guy", "https://en.wikipedia.org/wiki/Ramen" });
        }

        private void ThenTheMessageProcessorReturnNewGame()
        {
            Assert.True(_messageProcessor.CheckIfMessageIsANewGame(_newMessage));
        }

        private void GivenAMessageWithText(string messageText)
        {
            _newMessage = new NewMessage { text = messageText };
        }

        private void GivenTheMessageText(string messageText)
        {
            _messageText = messageText;
        }

        private void WhenLinksAreGeneratedOnMessageText()
        {
            _links = _messageProcessor.ExtractWikipediaLinks(_messageText);

        }
        private void ThenTheLinksLookLike(List<string> links)
        {
            Assert.Equal(links, _links);
        }
    }
}
