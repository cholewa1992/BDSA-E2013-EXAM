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
        private readonly ISearchService _searchService;

        private ConnectionModel _connectionModel;

        // Property names
        public const string SearchResultPropertyName = "SearchResult";


        private CancellationToken _searchServiceCancellationToken;


        private List<ISearchItem> _searchResults;

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


        public RelayCommand<ISearchItem> SelectionCommand { get; set; }


        /// <summary>
        ///     Initializes a new instance of the SearchViewModel class.
        /// </summary>
        public SearchViewModel(ISearchService searchService)
        {
            _searchService = searchService;


            _searchServiceCancellationToken = new CancellationToken();


            Messenger.Default.Register<ConnectionModelMessage>(this,
                (connectionMessage) =>
                {
                    _connectionModel = connectionMessage.ConnectionModel;
                });


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


            // Setup command actions
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

                        /*
                    case ItemType.Person:
                        Messenger.Default.Send(new ChangeViewMessage()
                        {
                            view = new Person();
                            viewAction = () => Messenger.Default.Send(new SelectionMessage(
                            {
                                SelectedItem = searchItem
                            });
                        });
                        break;*/
                }
            });
        }
    }
}