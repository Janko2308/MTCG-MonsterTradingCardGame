using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    internal class BattleUtils
    {
        public ManualResetEventSlim Mres { get; private set; } = new ManualResetEventSlim(false);
        public string BattleLog { get; set; } = "";

        public string Player1 { get; set; } = "";
        public string Player2 { get; set; } = "";


        public BattleUtils()
        {
            
        }
    }
    
}
