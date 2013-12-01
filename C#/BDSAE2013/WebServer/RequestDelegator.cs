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
    /// @invariant _storage != null
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
            _requestControllers.Add(new MovieInfoRequestController());
            _requestControllers.Add(new PeopleInfoRequestController());

            //Invariant Check
            if (_storage == null)
                throw new StorageNullException("Storage was null when trying to process request");

        }

        /// <summary>
        /// Method to process incoming requests.
        /// The method will use the underlying controllers to process the request.
        /// </summary>
        /// <param name="request"> The request to process </param>
        public Request ProcessRequest(Request request)
        {
            //Check if the incoming request is null
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
                Console.WriteLine("No valid controller was found");

                //This response code is returned when no controllers can process the incoming request
                request.ResponseStatusCode = Request.StatusCode.BadRequest;
                return request;
            }

            Func<IStorageConnectionBridge, object> storageDelegate;

            //Process the request using the defined controller.
            try
            {
                storageDelegate = controller.ProcessRequest(request);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("An error occured while processing request. Error: "+e.Message);

                //This response code is returned if the request was not a restful method or some vital input was null.
                request.ResponseStatusCode = Request.StatusCode.BadRequest;
                return request;
            }

            //Invoke the delegate received from the controller using the storage module given through the constructor.
            try
            {
                object returnValue = storageDelegate.Invoke(_storage);

                /*
                if(returnValue == typeof(string))
                    //Set response string
                else
                    //Set response object
                */

                request.ResponseStatusCode = Request.StatusCode.Ok;
                return request;
            }
            catch (ArgumentException e)
            {
                //This response code is returned if the request could not process the request in the database
                request.ResponseStatusCode = Request.StatusCode.NotFound;
                return request;
            }
        }

        /// <summary>
        /// Method to define whether there is a controller in the list of controllers that can handle the incoming request.
        /// If there is, the method returns the controller.
        /// @pre _requestControllers != null
        /// @pre _requestControllers.Count > 0
        /// </summary>
        /// <param name="method"> The method part of the incoming request </param>
        /// <returns> The controller to be used to determine the work that has to be done on the storage module </returns>
        private IRequestController DefineController(string method)
        {
            //pre condition checks
            if (_requestControllers == null)
                throw new RequestControllerListException("List of request controllers cannot be null when invoking DefineController method");
            
            if (!(_requestControllers.Count > 0))
                throw new RequestControllerListException("List of request controllers must contain at least 1 controller when invoking DefineController method");

            //Check if the incoming method is null
            if (method == null)
                throw new ArgumentNullException("Method string must not be null");

            if (method.Split(' ').Length != 2)
                throw new ArgumentException("Incoming request method has bad syntax, must be [Method]' '[URL]");

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
