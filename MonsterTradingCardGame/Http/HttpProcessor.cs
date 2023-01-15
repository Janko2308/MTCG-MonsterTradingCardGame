using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.Http.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http
{
    public class HttpProcessor
    {
        private TcpClient clientSocket;
        private readonly Dictionary<string, IController> controllerMap = new();

        public HttpProcessor(TcpClient clientSocket)
        {
            this.clientSocket = clientSocket;
            controllerMap.TryAdd("/users", new UserController());
            controllerMap.TryAdd("/sessions", new SessionController());
            controllerMap.TryAdd("/packages", new PackageController());
            controllerMap.TryAdd("/transactions/packages", new TransactionsController());
            controllerMap.TryAdd("/cards", new CardsController());
            controllerMap.TryAdd("/deck", new DeckController());
            controllerMap.TryAdd("/deck?format=plain", new DeckController());
            controllerMap.TryAdd("/stats", new StatsController());
            controllerMap.TryAdd("/score", new ScoreController());
            controllerMap.TryAdd("/battles", new BattleController());
            controllerMap.TryAdd("/users/delete", new UserController());
            controllerMap.TryAdd("/sessions/logout", new SessionController());
        }

        public void run()
        {
            var reader = new StreamReader(clientSocket.GetStream());
            var request = new HttpRequest(reader);
            var response = new HttpResponse(new StreamWriter(clientSocket.GetStream()));
            try
            {
                request.Parse();               
            }
            catch (Exception e)
            {
                ResponseUtils.SetResponseData(response, 400, "Bad Request", e.Message);
                return;
            }
            string name = request.Path.Split("/").Last();
            var tempController = new SessionController();
            try
            {
                controllerMap.TryAdd("/users/" + name, new UserController());
                
                if (!controllerMap.ContainsKey(request.Path))
                {
                    ResponseUtils.SetResponseData(response, 404, "Not Found", "");                    
                }
                else
                {
                    var controller = controllerMap[request.Path];
                    try
                    {
                        Console.WriteLine(request.Content);
                        controller.Run(request, response);
                    }
                    catch(JsonException)
                    {
                        ResponseUtils.SetResponseData(response, 400, "Bad Request", "");
                    }
                    catch(NoSuchMethodException)
                    {
                        ResponseUtils.SetResponseData(response, 400, "Bad Request no such request", "");
                    }
                    
                }
            }
            catch (AuthenticateTokenException)
            {
                ResponseUtils.SetResponseData(response, 401, "Access token is missing or invalid", "");
            }
            catch (NoSuchMethodException)
            {
                ResponseUtils.SetResponseData(response, 400, "Bad Request no such request", "");
            }

            response.Process();
        }
    }
}