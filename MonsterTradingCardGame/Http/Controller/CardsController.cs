using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class CardsController : IController
    {
        private CardsLogic cardsLogic = new();
        public void Run(Http.HttpRequest request, Http.HttpResponse response)
        {
            if (request.Method == "GET")
            {
                try
                {
                    if (AuthenticationToken.ReturnAuthenticationToken(request) == "" )                   
                    {
                        throw new AuthenticateTokenException("Not Authorized");
                    }
                    
                    cardsLogic.GetCards(request, response);
                    
                }
                catch (NoItemAvaiableException e)
                {
                    ResponseUtils.SetResponseData(response, 204, "The request was fine, but the user doesn´t have any cards", "");
                }
                catch (AuthenticateTokenException e)
                {
                    ResponseUtils.SetResponseData(response, 401, "Access token is missing or invalid", "");
                }
            }
            else
            {
                throw new NoSuchMethodException("Only GET requests in Cards");
            }
            
        }
    }
}
