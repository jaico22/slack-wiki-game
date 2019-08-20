using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiGameBot.Data.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        public string SlackUserId { get; set; }

        public string SlackUserName { get; set; }

        public int NumberOfWins { get; set; }

        public int NumberOfEntries { get; set; }

    }
}
