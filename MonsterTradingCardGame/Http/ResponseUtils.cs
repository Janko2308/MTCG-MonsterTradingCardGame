using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http
{
    internal class ResponseUtils
    {
        public static HttpResponse SetResponseData(HttpResponse response, int code, string text, string content)
        {
            response.ResponseCode = code;
            response.ResponseText = text;
            response.ResponseContent = content;

            return response;
        }
    }
}
