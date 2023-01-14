using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    internal class TransactionsLogic
    {
        private TransactionsRepository transactionsRepository = new();
        private UserLogic userlogic = new();
        public void BuyPackage(Http.HttpRequest request)
        {
            var authHeader = request.headers["Authorization"].ToString();
            authHeader = authHeader.Split(" ")[1];
            authHeader = authHeader.ToLower();
            string username = transactionsRepository.GetUserNameFromToken(authHeader);
            if (userlogic.CheckIfUserExists(username))
            {
                transactionsRepository.BuyPackage(authHeader); 
            }
            else
            {
                throw new AuthenticateTokenException("Token is not valid");
            }
        }
    }
}
