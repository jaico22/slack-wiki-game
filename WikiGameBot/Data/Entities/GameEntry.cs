using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiGameBot.Data.Entities
{
    public class GameEntry
    {
        [Key]
        public int Id { get; set; }
        public int LinkCount { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RawText { get; set; }

    }
}
