using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Message
{
    /// <summary>
    /// Message for sending a view
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    class ChangeViewMessage
    {
        public ContentControl view;

        public ISearchItem SearchItem;
    }
}
