using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Message
{
    class ChangeViewMessage
    {
        public ContentControl view;

        public ISearchItem SearchItem;
    }
}
