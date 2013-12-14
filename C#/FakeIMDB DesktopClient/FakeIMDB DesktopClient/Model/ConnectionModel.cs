using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace FakeIMDB_DesktopClient.Model
{
    /// <summary>
    /// Model with properties describing a Connection
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class ConnectionModel
    {
        public string Address { get; set; }
        public Protocols Protocol { get; set; }
        public int Timeout { get; set; }
    }
}
