using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL.Exceptions
{
    internal class NotUniqueCardException : Exception
    {
        public NotUniqueCardException(string message) : base(message)
        {
        }
    }
}
