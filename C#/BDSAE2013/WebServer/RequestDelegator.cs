using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;

namespace WebServer
{
    public class RequestDelegator
    {
        List<IRequestController> requestControllers;
        IStorageConnectionBridge storage;

        public RequestDelegator()
        {
            storage = new RefinedStorageConnectionBridge(new EFConnectionFactory());

            requestControllers = new List<IRequestController>();
            requestControllers.Add(new MovieRequestController());
            requestControllers.Add(new UserRequestController());
            requestControllers.Add(new PersonRequestController());
            requestControllers.Add(new FavouriteRequestController());
        }

        public void ProcessRequest(Request request)
        {
            Func<IStorageConnectionBridge, object> funcDelegate = DefineController(request.Method).ProcessRequest(request);

            funcDelegate.Invoke(storage);

            //Implement return values
        }

        private IRequestController DefineController(string method)
        {
            string url = method.Split(' ')[1];
            string[] urlKeywords = url.Split('/');

            for (int i = 0; i < urlKeywords.Length; i++)
            {
                foreach (IRequestController controller in requestControllers)
                {
                    if (urlKeywords[i] == controller.Keyword)
                    {
                        return controller;
                    }
                }
            }

            throw new ArgumentException("The url did not match any controllers");
        }

    }
}
