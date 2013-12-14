using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FakeIMDB_DesktopClient.Message;
using FakeIMDB_DesktopClient.Model;
using FakeIMDB_DesktopClient.Services;
using FakeIMDB_DesktopClient.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace FakeIMDB_DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the MovieItemView can bind to
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class MovieItemViewModel : ViewModelBase
    {
        // Services to use
        private readonly IMovieExtendedInformationService _informationService;
        private readonly IPutMovieDataService _moviePutService;

        // Model containing connection information for the services to use
        private ConnectionModel _connectionModel;

        // Cancellationtoken sent with async services
        private CancellationToken _extendedInfoCancellationToken;
        private CancellationToken _putInfoCancellationToken;


        // Property Names
        public const string MovieItemPropertyName = "MovieItem";



        private MovieSearchItem _movieItem;
        /// <summary>
        /// Property containing the MovieItem to be showed
        /// </summary>
        public MovieSearchItem MovieItem
        {
            get { return _movieItem; }
            set
            {
                if (_movieItem == value)
                    return;

                _movieItem = value;
                RaisePropertyChanged(MovieItemPropertyName);
            }
        }


        // Commands
        public RelayCommand<PersonSearchItem> SelectionCommand { get; set; } 
        public RelayCommand PutCommand { get; set; }


        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="informationService">A service setting a MovieItem with extended information by callback</param>
        /// <param name="moviePutService">A service updating the MovieItem properties offshore</param>
        public MovieItemViewModel(IMovieExtendedInformationService informationService, IPutMovieDataService moviePutService)
        {

            _informationService = informationService;
            _moviePutService = moviePutService;


            // Register to receive ConnectionModelMessages
            // When received, the local _connectionModel will be set accordingly
            Messenger.Default.Register<ConnectionModelMessage>(this,
                (connectionMessage) =>
                {
                    _connectionModel = connectionMessage.ConnectionModel;
                });

            // Register to messages
            Messenger.Default.Register<MovieSelectionMessage>(this, 
                (selectionMessage) => _informationService.GetDataAsync(
                    (item, error) =>
                    {
                        if (error != null)
                        {
                            MessageBox.Show(error.Message);
                            return;
                        }

                        
                        MovieItem = item;
                    }, selectionMessage.SearchItem, _connectionModel, _extendedInfoCancellationToken));


            // Commands
            SelectionCommand = new RelayCommand<PersonSearchItem>((selectedItem) =>
            {
                MovieItem = null;
                Messenger.Default.Send(new ChangeViewMessage()
                {
                    view = new PersonItemView(),
                    SearchItem = selectedItem
                });
            }
                );


            PutCommand = new RelayCommand(() => _moviePutService.PutDataAsync(
                (msg, error) =>
                {
                    if (error != null)
                    {
                        MessageBox.Show(error.Message);
                        return;
                    }

                    MessageBox.Show(msg);

                }, MovieItem, _connectionModel, _putInfoCancellationToken));
                
        }
    }
}
