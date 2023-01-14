using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL.Exceptions
{
    internal class UserAlreadyInBattleException : Exception
    {
        public UserAlreadyInBattleException(string message) : base(message)
        {
        }
    }
}
