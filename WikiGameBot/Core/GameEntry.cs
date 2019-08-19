using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core
{
    public class GameEntry
    {
        public int LinkCount { get; set; }
        public int GameId { get; set; }
        public string User { get; set; }
        public string UserName { get; set; }
        public string RawText { get; set; }
    }
}
