using MonsterTradingCardGame.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class SessionController : IController
    {
        private SessionLogic sessionLogic = new();
        public void Run(HttpRequest request, HttpResponse response)
        {
            if (request.Method == "POST")
            {
                Login(request, response);
            }
            else
            {
                //throw something
            }
        }

        private void Login(HttpRequest request, HttpResponse response)
        {
            try
            {
                User user = JsonSerializer.Deserialize<User>(request.Content);
                if (user == null)
                {
                    //TODO: Entsprechende Response schreiben 
                    throw new ArgumentNullException("json", "json is null");
                }
                string token = sessionLogic.Login(user.Username, user.Password);
                response = ResponseUtils.SetResponseData(response, 200, "Login successfull", token);
            }
            catch (AuthenticationException e)
            {
                response = ResponseUtils.SetResponseData(response, 401, "Invalid username/password provided", e.Message);
            }
        }
        public bool CheckIfSession(string username, string authorizationToken)
        {
            return sessionLogic.CheckIfSession(username, authorizationToken);
        }
    }
}
