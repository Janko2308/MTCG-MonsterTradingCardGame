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
    internal class StatsLogic
    {
        private StatsRepository statsRepository = new();
        private UserLogic userLogic = new();
        private SessionLogic sessionLogic = new();
        public void GetStats(Http.HttpRequest request, Http.HttpResponse response)
        {
            var authHeader = request.headers["Authorization"].ToString();
            authHeader = authHeader.Split(" ")[1];
            authHeader = authHeader.ToLower();
            string userName = userLogic.GetUserNameFromToken(authHeader);
            if (!sessionLogic.CheckIfSession(userName, authHeader))
            {
                throw new AuthenticateTokenException("Access token is missing or invalid");
            }
            Stats userStats = statsRepository.GetStats(userName);



            ResponseUtils.SetResponseData(response, 200, "OK", JsonSerializer.Serialize(userStats));
        }
    }
}
