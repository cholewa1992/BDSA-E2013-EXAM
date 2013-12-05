using System.Linq;
using EntityFrameworkStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage;

namespace EntityFrameworkStorageUnitTest
{
    [TestClass]
    public class EFStorageTest
    {
        [TestMethod]
        public void AddToContextTest()
        {
            
            using (var ef = new StorageConnectionBridgeFacade(new EFConnectionFactory<FakeContext>()))
            {
                ef.Add(new UserAcc{Email = "jbec@itu.dk", Password = "1234"});
                Assert.AreEqual(1, ef.Get<UserAcc>().Count());
            }
        }

        [TestMethod]
        public void AddToContextWithOutSaveTest()
        {

        }
    }
}
