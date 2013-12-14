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
    /// This class contains properties that the PersonItemView can bind to
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class PersonItemViewModel : ViewModelBase
    {
        // Services to use
        private readonly IPersonExtendedInformationService _informationService;
        private readonly IPutPersonDataService _moviePutService;

        // Model containing connection information for the services to use
        private ConnectionModel _connectionModel;

        // Cancellationtoken sent with async services
        private CancellationToken _extendedInfoCancellationToken;
        private CancellationToken _putInfoCancellationToken;


        // Property names
        public const string PersonSearchItemPropertyName = "PersonItem";



        private PersonSearchItem _personItem;
        /// <summary>
        /// Property containing the PersonItem to be showed
        /// </summary>
        public PersonSearchItem PersonItem
        {
            get { return _personItem; }
            set
            {
                if (_personItem == value)
                    return;

                _personItem = value;
                RaisePropertyChanged(PersonSearchItemPropertyName);
            }
        }

        // Commands
        public RelayCommand<MovieSearchItem> SelectionCommand { get; set; }
        public RelayCommand PutCommand { get; set; } 


        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="informationService"></param>
        /// <param name="moviePutService"></param>
        public PersonItemViewModel(IPersonExtendedInformationService informationService, IPutPersonDataService moviePutService)
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

            
            // Register to receive PersonSelectionMessages
            // When received, the set informationservice will be used to fetch extended data and the local item will be set
            Messenger.Default.Register<PersonSelectionMessage>(this,
                (selectionMessage) => _informationService.GetDataAsync(
                    (item, error) =>
                    {

                        if (error != null)
                        {
                            MessageBox.Show(error.Message);
                            return;
                        }

                        PersonItem = item;
                    }, selectionMessage.SearchItem, _connectionModel, _extendedInfoCancellationToken));


            // Command sending a ChangeViewMessage with a selected item and View accordingly
            SelectionCommand = new RelayCommand<MovieSearchItem>((selectedItem) =>
            {
                PersonItem = null;
                Messenger.Default.Send(new ChangeViewMessage()
                {
                    view = new MovieItemView(),
                    SearchItem = selectedItem
                });
            }
                );

            // Command using the set putservice will be given the local PersonItem to update offshore
            PutCommand = new RelayCommand(() => _moviePutService.PutDataAsync(
                (msg, error) =>
                {
                    if (error != null)
                    {
                        MessageBox.Show(error.Message);
                        return;
                    }

                    MessageBox.Show(msg);

                }, PersonItem, _connectionModel, _putInfoCancellationToken));

        }
    }
}
