using MonsterTradingCardGame.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    internal class SessionLogic
    {
        private SessionRepository sessionRepository = new();
        //private UserLogic userLogic = new();
        public string Login(string username, string password)
        {
            password = PasswordHasher.HashPassword(password);
            if (sessionRepository.CheckIfPasswordIsCorrect(username, password))
            {
                Console.WriteLine("User logged in" + "########");
                return sessionRepository.CreateSession(username);
            }
            else
            {
                Console.WriteLine("False Password" + "########");
                throw new AuthenticationException("Wrong Credentials");
                //throw new UnauthorizedException("Password is incorrect");
            }
        }

        //throw new NotFoundException("User does not exist");

        public bool CheckIfSession(string Username, string AuthorizationToken)
        {
            return sessionRepository.CheckIfSession(Username, AuthorizationToken);
        }
    }
}
