using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.DAL;
using MonsterTradingCardGame.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    internal class PackageLogic
    {
        private PackageRepository packageRepository = new();
        private UserLogic userLogic = new();
        public void AddPackage(HttpRequest request, HttpResponse response)
        {
            var authHeader = request.headers["Authorization"].ToString();
            authHeader = authHeader.Split(" ")[1];
            authHeader = authHeader.ToLower();
            if (!CheckIfAdmin(authHeader))
            {
                throw new NotEnoughRightsException("User is not admin");
            }
            else
            {
                var packageCards = JsonNode.Parse(request.Content);
                if(packageCards == null){
                    throw new NullReferenceException();
                }
                List<Card> cardList = new();
                for (int i = 0; i < 5; i++)
                {
                    string tempDamage = packageCards[i]["Damage"].ToString();
                    int damage = int.Parse(tempDamage);
                    Card tempCard = new(packageCards[i]["Id"].ToString(), packageCards[i]["Name"].ToString(), packageCards[i]["Type"].ToString(), packageCards[i]["Element"].ToString(), damage);
                    cardList.Add(tempCard);
                }
                if(cardList.Count == 5)
                {
                    if (!packageRepository.CheckIfCardExists(cardList))
                    {
                        packageRepository.AddPackage(cardList);
                    }
                    else
                    {
                        throw new NotUniqueCardException("At least one card already exists");
                    }
                }
                else
                {
                    throw new NotRequiredAmountOfCardsException("Not enough cards");
                }

 
            }
        }
        public bool CheckIfAdmin(string token)
        {
            return userLogic.CheckIfAdmin(token);
        }
    }
}
