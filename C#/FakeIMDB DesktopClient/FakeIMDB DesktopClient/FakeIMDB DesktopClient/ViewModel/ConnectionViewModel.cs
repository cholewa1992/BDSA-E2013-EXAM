using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FakeIMDB_DesktopClient.Message;
using FakeIMDB_DesktopClient.Model;
using FakeIMDB_DesktopClient.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace FakeIMDB_DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the ConnectionView can bind to
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class ConnectionViewModel : ViewModelBase
    {

        // Commands
        public RelayCommand<string> ConnectionCommand { get; set; }


        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        public ConnectionViewModel() 
        {
            
            // Set command which first sends a ConnectionModelMessage with the address received through Command parameter
            // Then sends a ChangeViewMessage with a SearchView
            ConnectionCommand = new RelayCommand<string>((address)
                =>
            {
                Messenger.Default.Send(new ConnectionModelMessage()
                {
                    ConnectionModel = new ConnectionModel()
                    {
                        Address = address
                    }
                });

                Messenger.Default.Send(new ChangeViewMessage()
                {
                    view = new SearchView()
                });
            }
            );

        }

    }
}
