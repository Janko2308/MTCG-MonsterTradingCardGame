using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class BattleController : IController
    {
        private BL.BattleLogic battleLogic = new();
        public void Run(Http.HttpRequest request, Http.HttpResponse response)
        {
            if (request.Method == "POST")
            {
                try
                {
                    StartBattle(request, response);
                }
                catch (AuthenticateTokenException)
                {
                    ResponseUtils.SetResponseData(response, 401, "Access token is missing or invalid", "");
                }
                catch (UserAlreadyInBattleException)
                {
                    ResponseUtils.SetResponseData(response, 400, "User already in battle", "");
                }
                catch (InvalidDeckException)
                {
                    ResponseUtils.SetResponseData(response, 400, "Invalid Deck", "");
                }
                
            }
            else
            {
                throw new NoSuchMethodException("Only POST requests in Battle");
            }
        }
        public void StartBattle(Http.HttpRequest request, Http.HttpResponse response)
        {
            BattleUtils battleutils = battleLogic.StartBattle(request, response);
            battleutils.Mres.Wait();
            ResponseUtils.SetResponseData(response, 200, "OK", battleutils.BattleLog);
        }
    }
}
