using MonsterTradingCardGame.DAL;
using MonsterTradingCardGame.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    public class SessionLogic
    {
        private SessionRepository sessionRepository = new();
        public string Login(string username, string password)
        {
            password = PasswordHasher.HashPassword(password);
            if (sessionRepository.CheckIfPasswordIsCorrect(username, password))
            {
                return sessionRepository.CreateSession(username);
            }
            else
            {
                throw new AuthenticationException("Wrong Credentials");
            }
        }

        public bool CheckIfSession(string Username, string AuthorizationToken)
        {
            return sessionRepository.CheckIfSession(Username, AuthorizationToken);
        }

        public string CreateSession(string username)
        {
            return sessionRepository.CreateSession(username);
        }

        public void RemoveSession(HttpRequest request, HttpResponse response)
        {
            string token = AuthenticationToken.ReturnAuthenticationToken(request);
            sessionRepository.RemoveSession(token);
        }
    }
}
