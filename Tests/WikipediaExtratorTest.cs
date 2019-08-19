using System;
using System.Collections.Generic;
using System.Text;
using WikiGameBot.Core;
using Xunit;

namespace Tests
{
    public class WikipediaExtratorTest
    {
        public string _messageText;
        private readonly WikipediaLinkExtractor _wikipediaLinkExtractor;
        public WikipediaExtratorTest()
        {
            _wikipediaLinkExtractor = new WikipediaLinkExtractor();
        }
        [Fact]
        public void WikipediaLinksCanBeExtracted()
        {
            GivenAMessageWithText("Challenge: https://en.wikipedia.org/wiki/Buddy_Guy -> https://en.wikipedia.org/wiki/Ramen");
            ThenThereAreThisManyLinksExtracted(2);
        }

        private void ThenThereAreThisManyLinksExtracted(int v)
        {
            Assert.Equal(v, _wikipediaLinkExtractor.ExtractWikipediaLinks(_messageText).Count);
        }

        private void GivenAMessageWithText(string messageText)
        {
            _messageText = messageText;
        }
    }
}
