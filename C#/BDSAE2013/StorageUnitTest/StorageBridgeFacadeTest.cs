using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage;

namespace StorageUnitTest
{
    [TestClass]
    public class StorageBridgeFacadeTest
    {
        private UserAcc _user1;
        private UserAcc _user2;
        private UserAcc _user3;
        private Mock<IStorageConnection> _mock;
        private Mock<IStorageConnectionFactory> _factoryMock;

        [TestMethod]
        public void AddTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            Assert.IsTrue(sud.Add(new UserAcc()));
        }
        
        [TestMethod]
        public void AddFailedTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(false);
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            Assert.IsFalse(sud.Add(new UserAcc()));
        }

        [TestMethod]
        public void UpdateTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            Assert.IsTrue(sud.Update(new UserAcc()));
        }

        [TestMethod]
        public void UpdateFailedTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(false);
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            Assert.IsFalse(sud.Update(new UserAcc()));
        }

        [TestMethod]
        public void DeleteTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            Assert.IsTrue(sud.Delete(new UserAcc()));
        }

        [TestMethod]
        public void DeleteFailedTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(false);
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            Assert.IsFalse(sud.Delete(new UserAcc()));
        }

        [TestMethod]
        public void GetByIdTest()
        {
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            Assert.AreEqual(_user1,sud.Get<UserAcc>(1));
            Assert.AreEqual(_user2, sud.Get<UserAcc>(2));
            Assert.AreEqual(_user3, sud.Get<UserAcc>(3));
        }

        [TestMethod]
        public void GetByIdWrongIdTest()
        {
            //TODO
        }

        [TestMethod]
        public void GetAllTest()
        {
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            var e = sud.Get<UserAcc>();
            Assert.AreEqual(_user1,e.Single(t => t.Id == 1));
            Assert.AreEqual(_user2, e.Single(t => t.Id == 2));
            Assert.AreEqual(_user3, e.Single(t => t.Id == 3));
        }

        [TestMethod]
        public void GetAllEmptyStorageTest()
        {
            _mock.Setup(foo => foo.Get<UserAcc>()).Returns(new List<UserAcc>().AsQueryable);
            var sud = new StorageBridgeFacade(_factoryMock.Object);
            Assert.AreEqual(0, sud.Get<UserAcc>().Count());
        }

        [TestInitialize]
        public void Setup()
        {
            _user1 = new UserAcc
            {
                Email = "jbec@itu.dk",
                Firstname = "Jacob",
                Lastname = "Cholewa",
                Id = 1,
                Password = "1",
                Username = "U1"
            };

            _user2 = new UserAcc
            {
                Email = "bech@itu.dk",
                Firstname = "Benjamin",
                Lastname = "Cholewa",
                Id = 2,
                Password = "2",
                Username = "U2"
            };

            _user3 = new UserAcc
            {
                Email = "mart@itu.dk",
                Firstname = "Martin",
                Lastname = "T",
                Id = 3,
                Password = "3",
                Username = "U3"
            };

            _mock = new Mock<IStorageConnection>();
            var users = new HashSet<UserAcc> {_user1, _user2, _user3};
            _mock.Setup(foo => foo.Get<UserAcc>()).Returns(users.AsQueryable);
            _mock.Setup(foo => foo.Add(_user1));
            _mock.Setup(foo => foo.Add(_user2));
            _mock.Setup(foo => foo.Add(_user3));
            _mock.Setup(foo => foo.Delete(_user1));
            _mock.Setup(foo => foo.Delete(_user2));
            _mock.Setup(foo => foo.Delete(_user3));
            _mock.Setup(foo => foo.Update(_user1));
            _mock.Setup(foo => foo.Update(_user2));
            _mock.Setup(foo => foo.Update(_user3));

            _factoryMock = new Mock<IStorageConnectionFactory>();
            _factoryMock.Setup(foo => foo.GetConnection()).Returns(_mock.Object);
        }
    }
}
