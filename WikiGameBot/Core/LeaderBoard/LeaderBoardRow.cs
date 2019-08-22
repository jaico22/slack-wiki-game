using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core.LeaderBoard
{
    public class LeaderBoardRow
    {
        public int Position { get; set; }

        public string PlayerName { get; set; }

        public int NumberOfWins { get; set; }

        public int NumerOfEntries { get; set; }

        public decimal WinPercentage { get; set; }

    }
}
