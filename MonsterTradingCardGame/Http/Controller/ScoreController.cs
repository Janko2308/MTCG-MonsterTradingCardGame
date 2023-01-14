using MonsterTradingCardGame.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class ScoreController : IController
    {
        private BL.ScoreLogic scoreLogic = new();
        public void Run(Http.HttpRequest request, Http.HttpResponse response)
        {
            if (request.Method == "GET")
            {
                try
                {
                    GetScoreBoard(request, response);
                }
                catch (AuthenticateTokenException)
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
                throw new NoSuchMethodException("Only GET requests in Score");
            }
        }
        public void GetScoreBoard(Http.HttpRequest request, Http.HttpResponse response)
        {
            scoreLogic.GetScore(request, response);
        }
    }
}
