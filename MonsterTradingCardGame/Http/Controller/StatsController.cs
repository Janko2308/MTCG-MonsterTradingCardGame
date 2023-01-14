using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class StatsController : IController
    {
        private StatsLogic statsLogic = new();
        public void Run(Http.HttpRequest request, Http.HttpResponse response)
        {
            if (request.Method == "GET")
            {
                try
                {
                    GetStats(request, response);
                }
                catch(AuthenticateTokenException)
                {
                    ResponseUtils.SetResponseData(response, 401, "Access token is missing or invalid", "");
                }
                catch (UserNotFoundException)
                {
                    ResponseUtils.SetResponseData(response, 404, "User not found", "");
                }
            }
            else
            {
                throw new NoSuchMethodException("Only GET requests in Stats");
            }
        }
        public void GetStats(HttpRequest request, HttpResponse response)
        {
            statsLogic.GetStats(request, response);
        }
    }
    
    
}
