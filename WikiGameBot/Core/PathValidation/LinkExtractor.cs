using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WikiGameBot.Core.PathValidation
{
    public class LinkExtractor
    {
        public HttpClient _httpClient { get; set; }

        public LinkExtractor()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<WikiLink>> GeneratePageLinks(string url)
        {
            List<WikiLink> wikiLinks = new List<WikiLink>();

            var response = await _httpClient.GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();

            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var links = pageDocument.DocumentNode.SelectNodes("//body//a");

            foreach(var link in links)
            {
                var newWikiLink = new WikiLink();
                newWikiLink.LinkText = link.InnerText;
                newWikiLink.Url = link.GetAttributeValue("href", null);
                if (string.IsNullOrEmpty(newWikiLink.Url) == false) {
                    switch (newWikiLink.Url[0])
                    {
                        case '/':
                            newWikiLink.Url = $"https://en.wikipedia.org{newWikiLink.Url}";
                            break;
                        case '#':
                            newWikiLink.Url = $"{url}{newWikiLink.Url}";
                            break;
                    }
                }
                newWikiLink.PageTitle = link.GetAttributeValue("title", null);
                wikiLinks.Add(newWikiLink);
            }
            return wikiLinks;
        }
    }
}
