using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage;

namespace StorageTest
{
    [TestClass]
    public class StorageBridgeFacadeTest
    {
        private UserAcc _addUser1;
        private UserAcc _user1;
        private UserAcc _user2;
        private UserAcc _user3;
        private Mock<IStorageConnection> _mock;
        private Mock<IStorageConnectionFactory> _factoryMock;

        [TestMethod]
        public void GetByIdEquals1Test()
        {
            const int id = 1;
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Get<UserAcc>(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetByIdBelow1Test()
        {
            const int id = -1;
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Get<UserAcc>(id);
        }

        [TestMethod]
        public void GetByIdIntMaxTest()
        {
            const int id = int.MaxValue;
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Get<UserAcc>(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetByIdAbowIntMaxTest()
        {
            var id = int.MaxValue;
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Get<UserAcc>(id + 1);
        }


        [TestMethod]
        public void UpdateIdEquals1Test()
        {
            var user = new UserAcc();
            const int id = 1;
            user.Id = id;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(user);
        }

        [TestMethod]
        [ExpectedException(typeof(InternalDbException))]
        public void UpdateIdBelow1Test()
        {
            var user = new UserAcc();
            const int id = -1;
            user.Id = id;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(user);
        }

        [TestMethod]
        public void UpdateIdIntMaxTest()
        {
            var user = new UserAcc();
            const int id = int.MaxValue;
            user.Id = id;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(user);
        }

        [TestMethod]
        [ExpectedException(typeof(InternalDbException))]
        public void UpdateIdAbowIntMaxTest()
        {
            var user = new UserAcc();
            var id = int.MaxValue;
            user.Id = id + 1;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(user);
        }


        [TestMethod]
        public void DeleteIdEquals1Test()
        {
            var user = new UserAcc();
            const int id = 1;
            user.Id = id;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete(user);
        }

        [TestMethod]
        [ExpectedException(typeof(InternalDbException))]
        public void DeleteIdBelow1Test()
        {
            var user = new UserAcc();
            const int id = -1;
            user.Id = id;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(user);
        }

        [TestMethod]
        public void DeleteIdIntMaxTest()
        {
            var user = new UserAcc();
            const int id = int.MaxValue;
            user.Id = id;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(user);
        }

        [TestMethod]
        [ExpectedException(typeof(InternalDbException))]
        public void DeleteIdAbowIntMaxTest()
        {
            var user = new UserAcc();
            var id = int.MaxValue;
            user.Id = id + 1;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(user);
        }

        [TestMethod]
        public void DeleteByIdEquals1Test()
        {
            const int id = 1;
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete<UserAcc>(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteByIdBelow1Test()
        {
            const int id = -1;

            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete<UserAcc>(id);
        }

        [TestMethod]
        public void DeleteByIdIntMaxTest()
        {
            const int id = int.MaxValue;
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete<UserAcc>(id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteByIdAbowIntMaxTest()
        {
            var id = int.MaxValue;
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete<UserAcc>(id + 1);
        }



        [TestMethod]
        public void AddTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Add(_addUser1);
        }

        [TestMethod]
        [ExpectedException(typeof(ChangesWasNotSavedException))]
        public void AddFailedTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(false);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Add(_addUser1);
        }

        [TestMethod]
        public void UpdateTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(_user1);
        }

        [TestMethod]
        [ExpectedException(typeof(ChangesWasNotSavedException))]
        public void UpdateFailedTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(false);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Update(_user1);
        }

        [TestMethod]
        public void DeleteTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete(_user1);
        }

        [TestMethod]
        [ExpectedException(typeof(ChangesWasNotSavedException))]
        public void DeleteFailedTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(false);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete(_user1);
        }

        [TestMethod]
        public void DeleteByIdTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete<UserAcc>(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ChangesWasNotSavedException))]
        public void DeleteByIdFailedTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(false);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete<UserAcc>(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteByIdNotFoundFailedTest()
        {
            _mock.Setup(foo => foo.SaveChanges()).Returns(false);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Delete<UserAcc>(4);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            Assert.AreEqual(_user1, sud.Get<UserAcc>(1));
            Assert.AreEqual(_user2, sud.Get<UserAcc>(2));
            Assert.AreEqual(_user3, sud.Get<UserAcc>(int.MaxValue));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetByIdWrongIdTest()
        {
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            sud.Get<UserAcc>(4);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            var e = sud.Get<UserAcc>();
            Assert.AreEqual(_user1, e.Single(t => t.Id == 1));
            Assert.AreEqual(_user2, e.Single(t => t.Id == 2));
            Assert.AreEqual(_user3, e.Single(t => t.Id == int.MaxValue));
        }

        [TestMethod]
        public void GetAllEmptyStorageTest()
        {
            _mock.Setup(foo => foo.Get<UserAcc>()).Returns(new List<UserAcc>().AsQueryable);
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            Assert.AreEqual(0, sud.Get<UserAcc>().Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNull()
        {
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            sud.Add((UserAcc) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateNull()
        {
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            sud.Update((UserAcc)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNull()
        {
            var sud = new StorageConnectionBridgeFacade(_factoryMock.Object);
            _mock.Setup(foo => foo.SaveChanges()).Returns(true);
            sud.Delete((UserAcc)null);
        }

        [TestInitialize]
        public void Setup()
        {
            _addUser1 = new UserAcc
            {
                Email = "jbec@itu.dk",
                Firstname = "Jacob",
                Lastname = "Cholewa",
                Password = "1",
                Username = "U1"
            };

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
                Id = int.MaxValue,
                Password = "3",
                Username = "U3"
            };

            _mock = new Mock<IStorageConnection>();
            var users = new HashSet<UserAcc> { _user1, _user2, _user3 };
            _mock.Setup(foo => foo.Get<UserAcc>()).Returns(users.AsQueryable);

            //_mock.Setup(foo => foo.Add(It.IsAny<UserAcc>()));
            _mock.Setup(foo => foo.Add(_addUser1)).Callback(() => _addUser1.Id = 3);

            _mock.Setup(foo => foo.Delete(_user1));
            _mock.Setup(foo => foo.Delete(_user2));
            _mock.Setup(foo => foo.Delete(_user3));

            _mock.Setup(foo => foo.Update(_user1));
            _mock.Setup(foo => foo.Update(_user2));
            _mock.Setup(foo => foo.Update(_user3));

            _factoryMock = new Mock<IStorageConnectionFactory>();
            _factoryMock.Setup(foo => foo.CreateConnection()).Returns(_mock.Object);
        }
    }
}