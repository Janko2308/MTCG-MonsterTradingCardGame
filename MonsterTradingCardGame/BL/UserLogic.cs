using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.DAL;
using MonsterTradingCardGame.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    public class UserLogic
    {
        private UserRepository userRepository = new();
        private SessionLogic sessionLogic = new();
        public void RegisterUser(string username, string password)
        {
            if (userRepository.CheckIfUserExists(username))
            {
                throw new ConflictException("Username already exists");
            }
            else
            {
                if (username == "")
                {
                    throw new ArgumentNullException();
                }
                password = PasswordHasher.HashPassword(password);
                userRepository.RegisterUser(username, password);
                Console.WriteLine("User registered");
            }
        }
        
        public void DeleteUser(string username, string password)
        {
            
            if (userRepository.CheckIfUserExists(username))
            {
                password = PasswordHasher.HashPassword(password);
                userRepository.DeleteUser(username, password);
                Console.WriteLine("User deleted");
            }
            else
            {
                throw new NotFoundException("User not found");
            }
            
        }

        public bool CheckIfUserExists(string username)
        {
            return userRepository.CheckIfUserExists(username);
        }

        public UserInfo GetUser(string username)
        {
            return userRepository.GetUser(username);
        }

        public bool CheckIfSession(string Username, string AuthorizationToken)
        {
            return sessionLogic.CheckIfSession(Username, AuthorizationToken);
        }
        
        public void UpdateUserInfo(string name, string content)
        {
            if (!userRepository.CheckIfUserExists(name))
            {
                throw new UserNotFoundException("user not found");
            }
            userRepository.UpdateUserInfo(name, content);
        }

        public bool CheckIfAdmin(string token)
        {
            return userRepository.CheckIfAdmin(token);
        }
        
        public string GetUserNameFromToken(string token)
        {
            return userRepository.GetUserNameFromToken(token);
        }


    }
}
