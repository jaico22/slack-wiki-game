using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Bot
{
    public class PrintMessage
    {
        /// <summary>
        /// Body of message to be sent
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Indication if message is a reply to a thread
        /// </summary>
        public bool IsReply { get; set; }

        /// <summary>
        /// Thread timestamp used to identify what thread to send the reply to
        /// </summary>
        public DateTime? ThreadTs { get; set; }
    }
}
