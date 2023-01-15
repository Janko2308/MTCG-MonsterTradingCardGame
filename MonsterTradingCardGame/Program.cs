// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using MonsterTradingCardGame;
using MonsterTradingCardGame.Http;

new HttpServer(IPAddress.Any, 10001).run();

