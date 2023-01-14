using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL.Exceptions
{
    internal class NotEnoughRightsException : Exception
    {
        public NotEnoughRightsException(string message) : base(message)
        {
        }
    }
}
