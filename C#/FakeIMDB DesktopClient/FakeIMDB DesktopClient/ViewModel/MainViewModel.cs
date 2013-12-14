using System.Windows;
using System.Windows.Controls;
using CommunicationFramework;
using FakeIMDB_DesktopClient.Message;
using FakeIMDB_DesktopClient.Model;
using FakeIMDB_DesktopClient.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace FakeIMDB_DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the MainWindow can bind to
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class MainViewModel : ViewModelBase
    {

        // Model containing connection information for the services to use
        private readonly ConnectionModel _connectionModel;


        // Property Names
        public const string CurrentViewPropertyName = "CurrentView";
        public const string ProgramBannerPropertyName = "ProgramBanner";
        public const string BusyIndicatorPropertyName = "BusyIndicator";
        public const string SearchProgressPropertyName = "SearchProgress";
        public const string SearchProgressValuePropertyName = "SearchProgressValue";
        public const string SearchProgressVisibilityPropertyName = "SearchProgressVisibility";
        public const string IsAddressSetPropertyName = "IsAddressSet";



        private ContentControl _currentView;
        /// <summary>
        /// Property with the current ContentControl to be displayed
        /// </summary>
        public ContentControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView == value)
                    return;

                _currentView = value;
                RaisePropertyChanged(CurrentViewPropertyName);
            }
        }


        private ContentControl _busyIndicator;
        /// <summary>
        /// Property with a ContentControl
        /// </summary>
        public ContentControl BusyIndicator
        {
            get { return _busyIndicator; }
            set
            {
                if (_busyIndicator == value)
                    return;

                _busyIndicator = value;
                RaisePropertyChanged(BusyIndicatorPropertyName);
            }
        }


        private SearchProgressBar _searchProgress;
        /// <summary>
        /// Property with a SearchProgressBar object
        /// </summary>
        public SearchProgressBar SearchProgress
        {
            get { return _searchProgress; }
            set
            {
                if (_searchProgress == value)
                    return;

                _searchProgress = value;
                RaisePropertyChanged(SearchProgressPropertyName);
            }
        }


        private double _searchProgressValue;
        /// <summary>
        /// Property with the value of search progress
        /// </summary>
        public double SearchProgressValue
        {
            get { return _searchProgressValue; }
            set
            {
                if (_searchProgressValue == value)
                    return;

                _searchProgressValue = value;
                RaisePropertyChanged(SearchProgressValuePropertyName);
            }
        }


        private Visibility _searchProgressVisibility;
        /// <summary>
        /// Property describing the visibility of a Progress Bar
        /// </summary>
        public Visibility SearchProgressVisibility
        {
            get { return _searchProgressVisibility; }
            set
            {
                if (_searchProgressVisibility == value)
                    return;

                _searchProgressVisibility = value;
                RaisePropertyChanged(SearchProgressVisibilityPropertyName);
            }
        }


        private string _programBanner = string.Empty;
        /// <summary>
        /// Property containing the program banner to be displayed
        /// </summary>
        public string ProgramBanner
        {
            get { return _programBanner; }
            set
            {
                if (_programBanner == value)
                    return;

                _programBanner = value;
                RaisePropertyChanged(ProgramBannerPropertyName);
            }
        }


        private bool _isAddressSet = false;
        /// <summary>
        /// Property describing if the address had been set
        /// </summary>
        public bool IsAddressSet
        {
            get { return _isAddressSet; }
            set
            {
                if (_isAddressSet == value)
                    return;

                _isAddressSet = value;
                RaisePropertyChanged(IsAddressSetPropertyName);
            }
        }


        // Commands
        public RelayCommand<string> SearchCommand { get; set; }
        public RelayCommand ConnectionModelCommand { get; set; }
        public RelayCommand OptionsViewCommand { get; set; }



        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        public MainViewModel()
        {
            ProgramBanner = "FakeIMDB";

            CurrentView = new ConnectionView();

            //BusyIndicator = new BusyIndicator2();

            SearchProgressVisibility = Visibility.Hidden;

            //Default ConnectionModel
            _connectionModel = new ConnectionModel
            {
                Address = "http://localhost:1337/",
                Protocol = Protocols.Http,
                Timeout = 10000
            };


            // Initialize Views so they register to receive messages
            new SearchView();
            new MovieItemView();
            new PersonItemView();


            // Set default address in viewModels
            Messenger.Default.Send(new ConnectionModelMessage { ConnectionModel = _connectionModel });

            
            IsAddressSet = true;


            // Set command to change to the connection view
            OptionsViewCommand = new RelayCommand(() 
                => CurrentView = new ConnectionView()
            );


            // Set command which sends a ConnectionModelMessage
            ConnectionModelCommand =
                new RelayCommand(
                    () => Messenger.Default.Send(new ConnectionModelMessage {ConnectionModel = _connectionModel}));

            // Set command for what happend when searching.
            // The current view will be changed and a message will be sent with the Command parameter
            SearchCommand = new RelayCommand<string>(searchTerm =>
            {
                //SearchProgressVisibility = Visibility.Visible;

                CurrentView = new SearchView();

                Messenger.Default.Send(new SearchTermMessage
                {
                    searchTerm = searchTerm
                });
            });


            // Register to receive ChangeViewMessage messages
            // The view will be set accordingly, and the SearchItem type will be checked, casted
            // and sent to the set view
            Messenger.Default.Register<ChangeViewMessage>(this, changeViewMessage =>
            {
                CurrentView = changeViewMessage.view;

                if (changeViewMessage.SearchItem != null)
                {
                    if (changeViewMessage.SearchItem.Type == ItemType.Movie)
                    {
                        Messenger.Default.Send(new MovieSelectionMessage
                        {
                            SearchItem = (MovieSearchItem) changeViewMessage.SearchItem
                        });
                    }
                    if (changeViewMessage.SearchItem.Type == ItemType.Person)
                    {
                        Messenger.Default.Send(new PersonSelectionMessage
                        {
                            SearchItem = (PersonSearchItem) changeViewMessage.SearchItem
                        });
                    }
                }
            });

            // Register to receive ChangeToConnectionViewMessage messages
            Messenger.Default.Register<ChangeToConnectionViewMessage>(this, (a) => CurrentView = new MovieItemView());
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}