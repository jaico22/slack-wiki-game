using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core
{
    public class PageListExtractor
    {
        /// <summary>
        /// Split message text and parse out list of pages
        /// </summary>
        /// <param name="message"></param>
        /// <returns>List of page titles</returns>
        public List<string> GeneratePageList(string message)
        {
            Console.WriteLine(message);
            // Maintains consistency for UTs and any other unseen cases where '>' isn't formatted as '&gt;'
            message = message.Replace(">", "&gt;");
            List<string> pageListOut = new List<string>();
            var paragraphs = message.Trim().Split("\n");
            foreach (var paragraph in paragraphs)
            {
                List<string> pageList = new List<string>();
                var splitMessageText = paragraph.Split("-&gt;");
                foreach (var word in splitMessageText)
                {
                    var trimmedWord = word.Trim();
                    pageList.Add(word.Trim());
                }
                if (pageList.Count > pageListOut.Count)
                {
                    pageListOut = pageList;
                }
            }
            return pageListOut;
        }

    }
}
