using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core
{
    public class GameStartData
    {
        /// <summary>
        /// Indicates if GameStartData is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Link that each path should start with
        /// </summary>
        public string StartingUrl { get; set; }

        /// <summary>
        /// Link that each path should end with
        /// </summary>
        public string EndingUrl { get; set; }

        /// <summary>
        /// Timestamp of Thread Associated with the game
        /// </summary>
        public DateTime ThreadTs { get; set; }

    }
}
