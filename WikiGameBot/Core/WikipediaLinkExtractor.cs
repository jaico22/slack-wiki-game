using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core
{
    public class WikipediaLinkExtractor
    {
        /// <summary>
        /// Detects and extracts wikipedia links
        /// </summary>
        /// <param name="messageText"></param>
        /// <returns></returns>
        public List<string> ExtractWikipediaLinks(string messageText)
        {
            // Clean Message
            var messageTextClean = messageText;
            messageTextClean = messageTextClean.Replace("\n", " ");
            messageTextClean = messageTextClean.Replace("<", "");
            messageTextClean = messageTextClean.Replace(">", "");

            // Parse Cleaned Message
            List<string> wikipediaLinks = new List<string>();
            var words = messageTextClean.Split(' ');
            foreach (var word in words)
            {
                var lowerWord = word.ToLower();
                if (lowerWord.Contains("https://en.wikipedia.org") ||
                    lowerWord.Contains("en.wikipedia.org") ||
                    lowerWord.Contains("http://en.wikipedia.org")
                    )
                {
                    wikipediaLinks.Add(word);
                }
            }
            return wikipediaLinks;
        }
    }
}
