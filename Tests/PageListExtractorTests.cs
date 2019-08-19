using System;
using System.Collections.Generic;
using System.Text;
using WikiGameBot.Core;
using Xunit;

namespace Tests
{
    public class PageListExtractorTests
    {
        private readonly PageListExtractor _pageListExtractor;
        public string _messageText { get; set; }
        public PageListExtractorTests()
        {
            _pageListExtractor = new PageListExtractor();
        }

        [Fact]
        public void PageListsWithNoSpacesCanBeParsed()
        {
            GivenTheMessageText("a->b->c->d");
            ThenThePageCountIs(4);
        }

        [Fact]
        public void PageListsWithSpacesCanBeParsed()
        {
            GivenTheMessageText("a -> b -> c->d");
            ThenThePageCountIs(4);
        }

        [Fact]
        public void MessagesWithNewLinesAreParsedCorrectly()
        {
            GivenTheMessageText("a -> b -> c->d\nThis is a new line -> This won't count as two");
            ThenThePageCountIs(4);
        }

        private void ThenThePageCountIs(int v)
        {
            Assert.Equal(v, _pageListExtractor.GeneratePageList(_messageText).Count);
        }

        private void GivenTheMessageText(string messageText)
        {
            _messageText = messageText;
        }
    }
}
