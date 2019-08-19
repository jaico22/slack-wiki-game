using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core
{
    public class GameStatistics
    {
        public int BestEntry { get; set; }

        public string BestEntryMessage { get; set; }

        public int WorstEntry { get; set; }

        public string WorstEntryMessage { get; set; }

        public string CurrentWinner { get; set; }
    }
}
