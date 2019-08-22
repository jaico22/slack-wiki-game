using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiGameBot.Data.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Timestamp associated with the thread related to the game
        /// </summary>
        public DateTime ThreadTimeStamp { get; set; }

        /// <summary>
        /// Indication if game is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// URL Players Start At
        /// </summary>
        public string StartingUrl { get; set; }

        /// <summary>
        /// URL Players End At
        /// </summary>
        public string EndingUrl { get; set; }
    }
}
