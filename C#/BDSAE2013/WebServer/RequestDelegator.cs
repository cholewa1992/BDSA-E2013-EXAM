using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{
    public class RequestDelegator
    {
        IRequestController[] requestControllerArray;
        public void ProcessRequest(Request request)
        {
            DefineController(request.Method).ProcessRequest(request);
        }

        private IRequestController DefineController(string method)
        {
            string url = method.Split(' ')[1];
            string[] urlKeywords = url.Split('/');

            for (int i = 0; i < urlKeywords.Length; i++)
            {
                for (int j = 0; j < requestControllerArray.Length; j++)
                {
                    if( urlKeywords[i] == requestControllerArray[j].Keyword )
                    {
                        return requestControllerArray[j];
                    }
                }
            }

            throw new ArgumentException("The url did not match any controllers");
        }

    }
}
