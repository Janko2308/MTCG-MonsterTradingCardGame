using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal interface IController
    {
        public void Run(HttpRequest request, HttpResponse response);
    }
}
