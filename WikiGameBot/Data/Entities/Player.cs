using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiGameBot.Data.Entities
{
    public class Player
    {
        /// <summary>
        /// Unique identifier in slack for player
        /// </summary>
        [Key]
        public string SlackUserId { get; set; }

        /// <summary>
        /// Users Username
        /// </summary>
        public string SlackUserName { get; set; }

        /// <summary>
        /// Number of times user has won the challenge
        /// </summary>
        public int NumberOfWins { get; set; }

        /// <summary>
        /// Number of times user has played
        /// </summary>
        public int NumberOfEntries { get; set; }

    }
}
