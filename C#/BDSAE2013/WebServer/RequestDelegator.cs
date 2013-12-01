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
    /// <summary>
    /// A class to handle the interactions between a communcation protocol and a storage module.
    /// The class contains a list of control objects that can process different types of request when invoked. This results in delegates that can be used to access the storage module.
    /// </summary>
    public class RequestDelegator
    {
        private readonly List<IRequestController> _requestControllers;        //A list of the available controllers used by the class
        private readonly IStorageConnectionBridge _storage;                   //The storage module linked to the RequestDelegator, this can be injected on instantiation

        /// <summary>
        /// The base constructor of the class.
        /// This constructor calls the general constructor with the most commonly used storage module.
        /// </summary>
        public RequestDelegator()
            : this(new StorageBridgeFacade(new EFConnectionFactory()))
        {
        }

        /// <summary>
        /// The main constructor of the class.
        /// This constructor sets up the storage module used by the class give the constructor parameters.
        /// The constructor also sets up the list of controllers.
        /// </summary>
        /// <param name="storage"> The storage for the class to use </param>
        public RequestDelegator(IStorageConnectionBridge storage)
        {
            //Check if the storage is null
            if (storage == null)
                throw new ArgumentNullException("Storage cannot be null");

            _storage = storage;

            //Initialize the list of controllers as well as adding each controller
            _requestControllers = new List<IRequestController>();
            _requestControllers.Add(new MovieRequestController());
            _requestControllers.Add(new UserRequestController());
            _requestControllers.Add(new PersonRequestController());
            _requestControllers.Add(new FavouriteRequestController());
            _requestControllers.Add(new MovieInfoController());
            _requestControllers.Add(new PersonInfoController());
        }

        /// <summary>
        /// Method to process incoming requests.
        /// The method will use the underlying controllers to process the request.
        /// </summary>
        /// <param name="request"> The request to process </param>
        public void ProcessRequest(Request request)
        {
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            IRequestController controller;

            //A call that defines the controller to process the request
            try
            {
                controller = DefineController(request.Method);
            }
            catch (ArgumentException e)
            {
                //TODO: compute a return message
                Console.WriteLine("No valid controller was found, or argument was null");
                return;
            }

            Func<IStorageConnectionBridge, object> storageDelegate;

            //Process the request using the defined controller.
            try
            {
                storageDelegate = controller.ProcessRequest(request);
            }
            catch (ArgumentException e)
            {
                //TODO: compute a return message
                Console.WriteLine("An exception was thrown durng request processing: " + e.Message);
                return;
            }

            //Invoke the delegate received from the controller using the storage module given through the constructor.
            storageDelegate.Invoke(_storage);

            //TODO: Implement return values
        }

        /// <summary>
        /// Method to define whether there is a controller in the list of controllers that can handle the incoming request.
        /// If there is, the method returns the controller.
        /// </summary>
        /// <param name="method"> The method part of the incoming request </param>
        /// <returns> The controller to be used to determine the work that has to be done on the storage module </returns>
        private IRequestController DefineController(string method)
        {
            if (method == null)
                throw new ArgumentNullException("Method string must not be null");

            //Split the method by the 'space' character. Take the second string from the resulting array. This is the url of the received request
            string url = method.Split(' ')[1];

            //Split the url by the '/' character. This will give us each "keyword" in the received url.
            string[] urlKeywords = url.Split('/');

            //Iterate through all keywords and all controllers.
            foreach(string keyword in urlKeywords)
            {
                foreach (IRequestController controller in _requestControllers)
                {
                    //If any keyword in the url matches the keyword recognized by one of the controllers we return the controller
                    if (keyword == controller.Keyword)
                    {
                        return controller;
                    }
                }
            }

            //If no keyword matched any controller the program throws an exception due to bad input.
            throw new ArgumentException("The url did not match any controllers");
        }

    }
}
