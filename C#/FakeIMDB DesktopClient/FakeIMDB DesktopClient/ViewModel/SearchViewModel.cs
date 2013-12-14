using System.Collections.Generic;
using System.Threading;
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
    /// This class contains properties that the SearchView can bind to
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class SearchViewModel : ViewModelBase
    {

        // Services to use
        private readonly ISearchService _searchService;


        // Model containing connection information for the services to use
        private ConnectionModel _connectionModel;


        // Cancellationtoken sent with async services
        private CancellationToken _searchServiceCancellationToken;


        // Property names
        public const string SearchResultPropertyName = "SearchResult";


        
        private List<ISearchItem> _searchResults;
        /// <summary>
        /// List of searchresults from an injected service
        /// </summary>
        public List<ISearchItem> SearchResult
        {
            get { return _searchResults; }
            set
            {
                if (_searchResults == value)
                {
                    return;
                }

                _searchResults = value;
                RaisePropertyChanged(SearchResultPropertyName);
            }
        }


        // Commands
        public RelayCommand<ISearchItem> SelectionCommand { get; set; }


        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="searchService">A service setting a list of searchresults by callback</param>
        public SearchViewModel(ISearchService searchService)
        {
            // Set the injected service
            _searchService = searchService;


            _searchServiceCancellationToken = new CancellationToken();


            // Register to receive ConnectionModelMessages
            // When received, the local _connectionModel will be set accordingly
            Messenger.Default.Register<ConnectionModelMessage>(this,
                (connectionMessage) =>
                {
                    _connectionModel = connectionMessage.ConnectionModel;
                });


            // Register to receive SearchTermMessages
            // When received the set service will be used to set the local searchResult
            Messenger.Default.Register<SearchTermMessage>(this, msg => _searchService.SearchAsync(
                (item, error) =>
                {
                    if (error != null)
                    {
                        MessageBox.Show(error.Message);
                        return;
                    }

                    SearchResult = item;
                }, msg.searchTerm, _connectionModel, _searchServiceCancellationToken
                ));


            // Set SelectionCommand. The ISearchItem command parameter's type is checked and
            // an appropriate view is sent in a message
            SelectionCommand = new RelayCommand<ISearchItem>(searchItem
                =>
            {
                switch (searchItem.Type)
                {
                    case ItemType.Movie:
                        Messenger.Default.Send(new ChangeViewMessage
                        {
                            view = new MovieItemView(),
                            SearchItem = searchItem
                        });
                        break;

                    case ItemType.Person:
                        Messenger.Default.Send(new ChangeViewMessage
                        {
                            view = new PersonItemView(),
                            SearchItem = searchItem
                        });
                        break;

                        
                }
            });
        }
    }
}