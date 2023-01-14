using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame.Http.Controller
{
    internal class PackageController : IController
    {
        private PackageLogic packageLogic = new();
        public void Run(HttpRequest request, HttpResponse response)
        {
            if (request.Method == "POST")
            {
                try
                {
                    packageLogic.AddPackage(request, response);
                    ResponseUtils.SetResponseData(response, 201, "Package was successfully created", "");
                }
                catch(NotEnoughRightsException e)
                {
                    ResponseUtils.SetResponseData(response, 403, "Provided user is not admin", "");
                }
                catch (NotUniqueCardException e)
                {
                    ResponseUtils.SetResponseData(response, 409, "At least one card in the packages already exists", "");
                }
                
                
            }
            else 
            {
                throw new NoSuchMethodException("Is not a POST Request in Package");
            }
        }
    }
}
