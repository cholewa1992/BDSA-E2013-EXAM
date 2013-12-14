using System;
using FakeIMDB_DesktopClient.Services.Stubs;
using FakeIMDB_DesktopClient.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesktepClientUnitTest
{
    [TestClass]
    public class SearchViewModelTestClass
    {
        [TestMethod]
        public void SearchResultEmptyTest()
        {
            var model = new SearchViewModel(new SearchServiceStub());

            
        }
    }
}
