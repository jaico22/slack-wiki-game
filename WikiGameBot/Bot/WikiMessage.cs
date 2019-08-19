using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Bot
{
    public class WikiMessage
    {

        public int MessageId { get; set; }

        public string MessageType { get; set; }
        /// <summary>
        /// Identifier of Slack Thread
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// Date time related to thread
        /// </summary>
        /// 
        public DateTime ThreadTs { get; set; }
        /// <summary>
        /// Slack Identifier for user
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Message text
        /// </summary>
        public string MessageText { get; set; }
    }
}
