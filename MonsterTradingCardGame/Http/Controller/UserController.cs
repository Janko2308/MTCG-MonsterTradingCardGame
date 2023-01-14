using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class UserController : IController
    {
        private UserLogic userLogic = new();
        public void Run(HttpRequest request, HttpResponse response)
        {
            if(request.Method == "POST")
            {
                Register(request, response);
            }
            else if(request.Method == "GET")
            {
                Get(request, response);
            }
            else if(request.Method == "PUT")
            {
                Update(request, response);
            }
        }

        private void Register(HttpRequest request, HttpResponse response)
        {
            try
            {
                User user = JsonSerializer.Deserialize<User>(request.Content);
                if (user == null)
                {
                    //TODO: Entsprechende Response schreiben 
                    throw new ArgumentNullException("json", "json is null");
                }
                userLogic.RegisterUser(user.Username, user.Password);
                response = ResponseUtils.SetResponseData(response, 201, "Created", "");
            }
            catch (ConflictException e)
            {
                response = ResponseUtils.SetResponseData(response, 409, "Conflict", e.Message);
            }
        }
        
        private void Get(HttpRequest request, HttpResponse response)
        {
            try
            {
                string name = request.Path.Split("/").Last();
                
                //admin is allowed everything && name != "admin"
                UserInfo user = userLogic.GetUser(name);
                if (user.Name == null)
                {
                    throw new UserNotFoundException("user not found");
                }
                
                if (!userLogic.CheckIfSession(name, AuthenticationToken.ReturnAuthenticationToken(request)))
                {
                    throw new AuthenticateTokenException("Access Token missing or invalid");
                }
                ResponseUtils.SetResponseData(response, 200, "OK", JsonSerializer.Serialize(user));

            }
            catch (UserNotFoundException)
            {
                ResponseUtils.SetResponseData(response, 404, "Not Found", "");
            }
            catch(AuthenticateTokenException)
            {
                ResponseUtils.SetResponseData(response, 401, "Access token is missing or invalid", "");
            }
        }

        private void Update(HttpRequest request, HttpResponse response)
        {
            try
            {
                string name = request.Path.Split("/").Last();
                UserInfo user = userLogic.GetUser(name);
                if (user.Name == null)
                {
                    throw new UserNotFoundException("user not found");
                }
                if (!userLogic.CheckIfSession(name, AuthenticationToken.ReturnAuthenticationToken(request)))
                {
                    throw new AuthenticateTokenException("Not valid");
                }
                else
                {
                    userLogic.UpdateUserInfo(name, request.Content);
                    ResponseUtils.SetResponseData(response, 200, "OK", "");
                }
                //TODO: if token missing or invalid
                //TODO: user not found
                
            }
            catch (UserNotFoundException e)
            {
                ResponseUtils.SetResponseData(response, 404, "Not Found", "");
            }
            catch(ArgumentNullException e)
            {
                ResponseUtils.SetResponseData(response, 404, "Not Found", "");
            }
            catch (AuthenticateTokenException e)
            {
                ResponseUtils.SetResponseData(response, 401, "Access token is missing or invalid", "");
            }
        }
    }
}
