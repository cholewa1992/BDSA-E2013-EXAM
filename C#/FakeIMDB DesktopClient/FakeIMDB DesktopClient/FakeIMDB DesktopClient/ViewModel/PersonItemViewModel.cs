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

        private readonly IPersonExtendedInformationService _informationService;
        private readonly IPutPersonDataService _moviePutService;

        private ConnectionModel _connectionModel;


        private CancellationToken _extendedInfoCancellationToken;
        private CancellationToken _putInfoCancellationToken;


        public const string PersonSearchItemPropertyName = "PersonItem";



        private PersonSearchItem _personItem;
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

        public RelayCommand<MovieSearchItem> SelectionCommand { get; set; }
        public RelayCommand PutCommand { get; set; } 

        public PersonItemViewModel(IPersonExtendedInformationService informationService, IPutPersonDataService moviePutService)
        {

            _informationService = informationService;
            _moviePutService = moviePutService;


            Messenger.Default.Register<ConnectionModelMessage>(this,
                (connectionMessage) =>
                {
                    _connectionModel = connectionMessage.ConnectionModel;
                });

            
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


            SelectionCommand = new RelayCommand<MovieSearchItem>((selectedItem) =>
                Messenger.Default.Send(new ChangeViewMessage()
                        {
                            view = new MovieItemView(),
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

                }, PersonItem, _connectionModel, _putInfoCancellationToken));

        }
    }
}
