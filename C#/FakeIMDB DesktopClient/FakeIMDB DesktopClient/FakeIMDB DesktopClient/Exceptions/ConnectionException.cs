using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeIMDB_DesktopClient.Exceptions
{
    /// <summary>
    /// ConnectionException
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    class ConnectionException: Exception
    {
        public ConnectionException()
    {
    }

    public ConnectionException(string message)
        : base(message)
    {
    }

    public ConnectionException(string message, Exception inner)
        : base(message, inner)
    {
    }
    }
}
