using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using WikiGameBot.Core.PathValidation;
using Xunit;

namespace Tests
{
    public class LinkExtractionTests
    {
        public string _url { get; set; }
        public List<WikiLink> _linkList { get; set; }

        private readonly LinkExtractorProcessor _linkExtractorProcessor;

        private readonly LinkMatcher _linkMatcher;

        public LinkExtractionTests()
        {
            _linkExtractorProcessor = new LinkExtractorProcessor();
            _linkMatcher = new LinkMatcher();
        }

        [Fact]
        public async Task LinkCanBeExtracted()
        {
            GivenTheUrl("https://en.wikipedia.org/wiki/Operation_Goodwood_(naval)");
            await WhenTheLinksAreExtracted();
            ThenThisLinkCanBeFound(new WikiLink { LinkText = "German battleship Tirpitz",
                                                  Url = "https://en.wikipedia.org/wiki/German_battleship_Tirpitz",
                                                  PageTitle = "German battleship Tirpitz"
                                                });
        }

        [Fact]
        public async Task ExactStringCanBeFound()
        {
            GivenTheUrl("https://en.wikipedia.org/wiki/Operation_Goodwood_(naval)");
            await WhenTheLinksAreExtracted();
            ThenALinkWithThisTitleCanBeFound("German battleship Tirpitz");
        }

        [Fact]
        public async Task ApproximateStringCanBeFound()
        {
            GivenTheUrl("https://en.wikipedia.org/wiki/Operation_Goodwood_(naval)");
            await WhenTheLinksAreExtracted();
            ThenALinkWithThisTitleCanBeFound("German battleship Triptz");
        }

        [Fact]
        public async Task IncompleteStringCannotBeFound()
        {
            GivenTheUrl("https://en.wikipedia.org/wiki/Operation_Goodwood_(naval)");
            await WhenTheLinksAreExtracted();
            ThenALinkWithThisTitleCannotBeFound("German battleship");
        }

        private void ThenALinkWithThisTitleCannotBeFound(string v)
        {
            _linkMatcher.wikiLinks = _linkList;
            var match = _linkMatcher.FindLink(v);
            Assert.Null(match);
        }

        private void ThenALinkWithThisTitleCanBeFound(string v)
        {
            _linkMatcher.wikiLinks = _linkList;
            var match = _linkMatcher.FindLink(v);
            Assert.NotNull(match);
        }

        private async Task WhenTheLinksAreExtracted()
        {
            _linkList = await _linkExtractorProcessor.GeneratePageLinks(_url);
        }

        private void ThenThisLinkCanBeFound(WikiLink wikiLink)
        {
            Assert.True(_linkList.Where(x => x.LinkText == wikiLink.LinkText && x.PageTitle == wikiLink.PageTitle).Count() >= 1);
        }

        private void GivenTheUrl(string url)
        {
            _url = url;
        }
    }
}
