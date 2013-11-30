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
        [TestMethod]
        public void AddToContextTest()
        {
            using (var ef = new EFConnectionFactory().GetConnection<FakeContext>())
            {
                ef.Add(new UserAcc());
                Assert.AreEqual(0, ef.Get<UserAcc>().Count());
                ef.SaveChanges();
                Assert.AreEqual(1, ef.Get<UserAcc>().Count());
            }
        }

        [TestMethod]
        public void AddToContextWithOutSaveTest()
        {
            using (var ef = new EFConnectionFactory().GetConnection<FakeContext>())
            {
                ef.Add(new UserAcc());
            }
        }
    }
}
