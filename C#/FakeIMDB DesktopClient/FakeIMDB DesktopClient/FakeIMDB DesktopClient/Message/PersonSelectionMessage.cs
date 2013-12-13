using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Message
{
    class PersonSelectionMessage
    {
        public PersonSearchItem SearchItem { get; set; }
    }
}
