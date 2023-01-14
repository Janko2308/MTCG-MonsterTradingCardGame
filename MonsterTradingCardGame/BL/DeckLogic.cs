using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.DAL;
using MonsterTradingCardGame.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    internal class DeckLogic
    {
        private DeckRepository deckRepository = new();
        private SessionLogic sessionLogic = new();
        private UserLogic userLogic = new();
        public void GetDeck(HttpRequest request, HttpResponse response)
        {
            var authHeader = request.headers["Authorization"].ToString();
            authHeader = authHeader.Split(" ")[1];
            authHeader = authHeader.ToLower();
            string userName = userLogic.GetUserNameFromToken(authHeader);
            if (!sessionLogic.CheckIfSession(userName, authHeader))
            {
                throw new AuthenticateTokenException("Access token is missing or invalid");
            }
            
            List<CardInfo> deck = deckRepository.GetDeck(userName);

            if (deck.Count == 0)
            {
                throw new NoItemAvaiableException("No Cards Available In Deck");
            }

            string lastPathSegment = request.Path.Split('/').Last(); 
            if(lastPathSegment == "deck?format=plain")
            {
                string plainDeck = "";
                int counter = 1;
                foreach (CardInfo card in deck)
                {
                    plainDeck += counter + ": Name: " + card.Name + "\nType: " + card.Type + "\nElement: " + card.Element + "\nDamage: " + card.Damage + "\n\n";
                    counter++;
                }
                ResponseUtils.SetResponseData(response, 200, "OK", plainDeck);
            }
            else
            {
                ResponseUtils.SetResponseData(response, 200, "OK", JsonSerializer.Serialize(deck));
            }

        }

        public void ConfigureDeck(HttpRequest request, HttpResponse response)
        {
            var authHeader = request.headers["Authorization"].ToString();
            authHeader = authHeader.Split(" ")[1];
            authHeader = authHeader.ToLower();
            string userName = userLogic.GetUserNameFromToken(authHeader);
            if (!sessionLogic.CheckIfSession(userName, authHeader))
            {
                throw new AuthenticateTokenException("Access token is missing or invalid");
            }
            List<string> cardIds = JsonSerializer.Deserialize<List<string>>(request.Content);
            if (cardIds.Count != 4)
            {
                throw new NotRequiredAmountOfCards("Not 4 cards for the deck");
            }
            deckRepository.ConfigureDeck(userName, cardIds);
            ResponseUtils.SetResponseData(response, 200, "The Deck has cards, the response contains these ", "");

        }

    }
}
