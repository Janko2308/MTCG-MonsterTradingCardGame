// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using MonsterTradingCardGame;
using MonsterTradingCardGame.Http;

/*
Rest.init();

while (true)
{
    IAsyncResult result = Rest.listener.BeginGetContext(new AsyncCallback(Rest.ListenerCallback), Rest.listener);
    Console.WriteLine("Waiting for request to be processed asyncronously.");
    result.AsyncWaitHandle.WaitOne();
    Console.WriteLine("Request processed asyncronously.");
    //Thread.Sleep(10000);
}
*/
DataBaseInit.init();
new HttpServer(IPAddress.Any, 10001).run();
Console.WriteLine("Hello, World!");
