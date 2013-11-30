using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using CommunicationFramework;
using Storage;

namespace WebServer
{
    class PeopleInfoController : AbstractRequestController
    {
        public PeopleInfoController()
        {
            Keyword = "PeopleInfo";
        }
    }
}
