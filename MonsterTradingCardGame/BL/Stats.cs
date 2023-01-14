using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    public class Stats
    {
        public string? Name { get; set; }
        public int? Elo { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
        public Stats(string? name, int? elo, int? wins, int? losses)
        {
            this.Name = name;
            this.Elo = elo;
            this.Wins = wins;
            this.Losses = losses;
        }
    }
}
