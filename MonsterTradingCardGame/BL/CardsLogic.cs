using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.DAL;
using MonsterTradingCardGame.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    internal class CardsLogic
    {
        private CardRepository cardRepository = new();
        private SessionLogic sessionLogic = new();
        private UserLogic userLogic = new();
        public void GetCards(HttpRequest request, HttpResponse response)
        {
            var authHeader = request.headers["Authorization"].ToString();
            authHeader = authHeader.Split(" ")[1];
            authHeader = authHeader.ToLower();
            string userName = userLogic.GetUserNameFromToken(authHeader);
            if (!sessionLogic.CheckIfSession(userName, authHeader))
            {
                throw new AuthenticateTokenException("Access token is missing or invalid");
            }
            
            List<CardInfo> cards = cardRepository.GetCards(userName);
            
            if (cards.Count == 0)
            {
                throw new NoItemAvaiableException("No Cards Available");
            }
            ResponseUtils.SetResponseData(response, 200, "OK", JsonSerializer.Serialize(cards));
            
        }
    }
}
