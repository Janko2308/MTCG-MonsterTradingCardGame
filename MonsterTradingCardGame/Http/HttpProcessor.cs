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
            //TODO mehr Endpunkte
        }

        public void run()
        {
            var reader = new StreamReader(clientSocket.GetStream());
            var request = new HttpRequest(reader);
            request.Parse();
            var response = new HttpResponse(new StreamWriter(clientSocket.GetStream()));
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
                        controller.Run(request, response);
                    }
                    catch(JsonException e)
                    {
                        ResponseUtils.SetResponseData(response, 400, "Bad Request", "");
                    }
                    catch(NoSuchMethodException e)
                    {
                        ResponseUtils.SetResponseData(response, 400, "Bad Request no such request", "");
                    }
                    
                }
            }
            catch (AuthenticateTokenException e)
            {
                ResponseUtils.SetResponseData(response, 401, "Access token is missing or invalid", "");
            }
            catch (NoSuchMethodException e)
            {
                ResponseUtils.SetResponseData(response, 400, "Bad Request no such request", "");
            }

            response.Process();
        }
    }
}