using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Message
{
    /// <summary>
    /// Message for sending a selected Person Search Item
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    class PersonSelectionMessage
    {
        public PersonSearchItem SearchItem { get; set; }
    }
}
