﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL.Exceptions
{
    public class NoSuchMethodException : Exception
    {
        public NoSuchMethodException(string message) : base(message)
        {
        }
    }
}
