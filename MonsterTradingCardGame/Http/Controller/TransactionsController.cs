using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class TransactionsController : IController
    {
        private TransactionsLogic transactionsLogic = new();
        public void Run(HttpRequest request, HttpResponse response)
        {
            if (request.Method == "POST")
            {
                try
                {
                    if (AuthenticationToken.ReturnAuthenticationToken(request) == "")
                    {
                        throw new AuthenticateTokenException("Not Authorized");
                    }
                    BuyPackage(request);
                    ResponseUtils.SetResponseData(response, 200, "A package has been successfully bought", "");
                }
                catch(AuthenticateTokenException e)
                {
                    ResponseUtils.SetResponseData(response, 401, "Access token is missing or invalid", "");
                }
                catch (NoItemAvaiableException e)
                {
                    ResponseUtils.SetResponseData(response, 404, "No card package avaiable", "");
                }
                catch (NotEnoughMoneyException e)
                {
                    ResponseUtils.SetResponseData(response, 403, "Not enough money for buying a card package", "");
                }
            }
            else
            {
                throw new NoSuchMethodException("Only POST requests in Transaction");
            }
        }
        public void BuyPackage(HttpRequest request)
        {
            transactionsLogic.BuyPackage(request);
        }
    }
}
