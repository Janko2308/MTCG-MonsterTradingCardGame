using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class DeckController : IController
    {
        private DeckLogic deckLogic = new();
        public void Run(HttpRequest request, HttpResponse response)
        {
            if (request.Method == "GET")
            {
                Get(request, response);
            }
            else if (request.Method == "PUT")
            {
                ConfigureDeck(request, response);
            }
            else
            {
                throw new NoSuchMethodException("");
            }
        }

        public void Get(HttpRequest request, HttpResponse response)
        {
            try
            {
                deckLogic.GetDeck(request, response);
                //SetResponse where ? 
                //ResponseUtils.SetResponseData(response, 200, "The deck has cards, the response contains these","");
            }
            catch (NoItemAvaiableException)
            {
                ResponseUtils.SetResponseData(response, 204, "The request was fine but the deck doesnn´t habe any cards", "");
            }
        }
        public void ConfigureDeck(HttpRequest request, HttpResponse response)
        {
            try
            {
                deckLogic.ConfigureDeck(request, response);
                
                ResponseUtils.SetResponseData(response, 200, "The deck has been successfully configured","");

            }
            catch (NotRequiredAmountOfCards)
            {
                ResponseUtils.SetResponseData(response, 400, "The provided deck did not include the required amount of cards", "");
            }
            catch (NoItemAvaiableException)
            {
                ResponseUtils.SetResponseData(response, 403, "At least one of the provided cards does not belong to the user or is not avaiable", "");
            }
        }
    }
    
    
}
