using System.Linq;
using EntityFrameworkStorage;
using EntityFrameworkStorageUnitTest.EFTestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage;

namespace EntityFrameworkStorageUnitTest
{
    [TestClass]
    public class EFStorageTest
    {
        private IStorageConnection _ef;

        [TestMethod]
        public void AddToContextTest()
        {
            _ef = new EFConnectionFactory().GetConnection<FakeContext>();
            _ef.Add(new UserAcc());
            _ef.SaveChanges();
            Assert.AreEqual(1, _ef.Get<UserAcc>().Count());
            _ef.Dispose();
        }

        [TestCleanup]
        public void CleanUp()
        {

        }
    }
}
