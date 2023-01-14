using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.BL
{
    public class AuthenticationToken
    {
        public static string ReturnAuthenticationToken(Http.HttpRequest request)
        {
            if (!request.headers.ContainsKey("Authorization")){
                return "";
            }
            string authenticationHeader = request.headers["Authorization"];
            string authenticationToken = "";
            if (authenticationHeader != null)
            {
                authenticationToken = authenticationHeader.Split(' ')[1];
            }
            return authenticationToken;
        }
    }
}
