using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL.Exceptions
{
    public class AuthenticateTokenException : Exception
    {
        public AuthenticateTokenException(string message) : base(message)
        {
        }
    }
}
