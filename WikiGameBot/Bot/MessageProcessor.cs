using SlackAPI.WebSocketMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Bot
{
    public class MessageProcessor
    {
        
        public void ProcessMessage(NewMessage message)
        {
            if (CheckIfMessageIsANewGame(message))
            {
                Console.WriteLine("New Game Started");
                // TODO: Add a new game
            }
            else
            {
                // TODO: Check if reply matches existing game
            }
        }

        /// <summary>
        /// Returns true if message is a new game
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CheckIfMessageIsANewGame(NewMessage message)
        {
            List<string> wikipediaLinks = ExtractWikipediaLinks(message.text);
            return wikipediaLinks.Count == 2;
        }

        /// <summary>
        /// Detects and extracts wikipedia links
        /// </summary>
        /// <param name="messageText"></param>
        /// <returns></returns>
        public List<string> ExtractWikipediaLinks(string messageText)
        {
            List<string> wikipediaLinks = new List<string>();
            var words = messageText.Split(' ');
            foreach (var word in words)
            {
                var lowerWord = word.ToLower();
                if (lowerWord.StartsWith("https://en.wikipedia.org") ||
                    lowerWord.StartsWith("en.wikipedia.org") ||
                    lowerWord.StartsWith("http://en.wikipedia.org"))
                {
                    wikipediaLinks.Add(word);
                }
            }
            return wikipediaLinks;
        }


    }
}
