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

    }
}
