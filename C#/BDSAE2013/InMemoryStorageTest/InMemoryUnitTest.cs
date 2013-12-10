using System.Linq;
using EntityFrameworkStorage;
using InMemoryStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage;

namespace InMemoryStorageTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddTest()
        {
            using (var ef = new InMemoryStorageConnectionFactory().CreateConnection())
            {
                var user = new UserAcc { Email = "jbec@itu.dk", Password = "1234" };
                Assert.IsFalse(ef.Get<UserAcc>().Any(t => t.Email == user.Email));

                ef.Add(user);
                Assert.IsFalse(ef.Get<UserAcc>().Any(t => t.Email == user.Email));

                Assert.IsTrue(ef.SaveChanges());
                Assert.IsTrue(ef.Get<UserAcc>().Any(t => t.Email == user.Email));
            }
        }

        [TestMethod]
        public void UpdateEntityTest()
        {
            using (var ef = new InMemoryStorageConnectionFactory().CreateConnection())
            {
                var user = new UserAcc { Email = "jbec@itu.dk", Password = "1234"};
                ef.Add(user);
                Assert.IsFalse(ef.Get<UserAcc>().Any(t => t.Id == user.Id && t.Email == "jbec@itu.dk"));
                ef.SaveChanges();
                Assert.IsTrue(ef.Get<UserAcc>().Any(t => t.Id == user.Id && t.Email == "jbec@itu.dk"));


                ef.Update(new UserAcc { Id = 1, Email = "jbec1@itu.dk", Password = "1234" });
                Assert.IsTrue(ef.Get<UserAcc>().Any(t => t.Id == user.Id && t.Email == "jbec@itu.dk"));
                Assert.IsFalse(ef.Get<UserAcc>().Any(t => t.Id == user.Id && t.Email == "jbec1@itu.dk"));

                ef.SaveChanges();
                Assert.IsFalse(ef.Get<UserAcc>().Any(t => t.Id == user.Id && t.Email == "jbec@itu.dk"));
                Assert.IsTrue(ef.Get<UserAcc>().Any(t => t.Id == user.Id && t.Email == "jbec1@itu.dk"));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InternalDbException))]
        public void UpdateEntityNotInContextTest()
        {
            using (var ef = new InMemoryStorageConnectionFactory().CreateConnection())
            {
                var user = new UserAcc { Email = "jbec@itu.dk", Password = "1234" };
                user.Email = "jbec1@itu.dk";
                Assert.IsFalse(ef.Get<UserAcc>().Any(t => t.Id == user.Id && t.Email == user.Email));
                ef.Update(user);
                ef.SaveChanges();
                Assert.IsTrue(ef.Get<UserAcc>().Any(t => t.Id == user.Id && t.Email == "jbec1@itu.dk"));
            }
        }

        [TestMethod]
        public void DeleteEntityTest()
        {
            using (var ef = new InMemoryStorageConnectionFactory().CreateConnection())
            {
                var user = new UserAcc { Email = "jbec@itu.dk", Password = "1234" };
                ef.Add(user);
                Assert.AreEqual(false, ef.Get<UserAcc>().Any(t => t.Id == user.Id));

                ef.SaveChanges();
                Assert.AreEqual(true, ef.Get<UserAcc>().Any(t => t.Id == user.Id));

                ef.Delete(user);
                Assert.AreEqual(true, ef.Get<UserAcc>().Any(t => t.Id == user.Id));

                ef.SaveChanges();
                Assert.AreEqual(false, ef.Get<UserAcc>().Any(t => t.Id == user.Id));

            }
        }
        [TestMethod]
        [ExpectedException(typeof(InternalDbException))]
        public void DeleteEntityNotInContextTest()
        {
            using (var ef = new InMemoryStorageConnectionFactory().CreateConnection())
            {
                var user = new UserAcc { Email = "jbec@itu.dk", Password = "1234" };
                Assert.IsFalse(ef.Get<UserAcc>().Any(t => t.Id == user.Id));
                ef.Delete(user);
                ef.SaveChanges();
                Assert.IsFalse(ef.Get<UserAcc>().Any(t => t.Id == user.Id));
            }
        }

        [TestInitialize]
        public void Init()
        {
            InMemoryStorageSet<UserAcc>.Clear();
        }
    }
}
