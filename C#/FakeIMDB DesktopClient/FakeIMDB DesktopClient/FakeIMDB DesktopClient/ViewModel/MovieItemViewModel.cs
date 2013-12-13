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
        private readonly IMovieExtendedInformationService _informationService;
        private readonly IPutMovieDataService _moviePutService;

        private ConnectionModel _connectionModel;


        private CancellationToken _extendedInfoCancellationToken;
        private CancellationToken _putInfoCancellationToken;

        // Property Names
        public const string WelcomeTitlePropertyName = "WelcomeTitle";
        public const string CurrentViewPropertyName = "CurrentView";
        public const string ProgramBannerPropertyName = "ProgramBanner";


        private MovieSearchItem _movieItem;
        public MovieSearchItem MovieItem
        {
            get { return _movieItem; }
            set
            {
                if (_movieItem == value)
                    return;

                _movieItem = value;
                RaisePropertyChanged("MovieItem");
            }
        }


        public RelayCommand<PersonSearchItem> SelectionCommand { get; set; } 
        public RelayCommand PutCommand { get; set; }


        public MovieItemViewModel(IMovieExtendedInformationService informationService, IPutMovieDataService moviePutService)
        {

            _informationService = informationService;
            _moviePutService = moviePutService;


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
                Messenger.Default.Send(new ChangeViewMessage()
                {
                    view = new PersonItemView(),
                    SearchItem = selectedItem
                })
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
