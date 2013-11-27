<<<<<<< HEAD
﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.EntityDto;
using WebServer;

namespace WebServerUnitTest
{
    [TestClass]
    public class UserDtoTest
    {
        [TestMethod]
        public void TestEmailValidationValidInput_Test()
        {
            var user = new UserDto();
            user.Email = "jbec@itu.dk";
            user.Email = "JBEC@ITU.dk";
            user.Email = "jbec01@iTu.dk";
            user.Email = "jbec01@iTu.com";
            user.Email = "jbec01@iTu.COM";
=======
﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;

namespace WebServerUnitTest
{
    [TestClass]
    public class UserDtoTest
    {
        [TestMethod]
        public void TestEmailValidationValidInput_Test()
        {
            var user = new UserDto();
            user.Email = "jbec@itu.dk";
            user.Email = "JBEC@ITU.dk";
            user.Email = "jbec01@iTu.dk";
            user.Email = "jbec01@iTu.com";
            user.Email = "jbec01@iTu.COM";
>>>>>>> bf2ab07a84a297679ad69f4431b881d5e113be77
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmailValidationNull_Test()
        {
            var user = new UserDto();
            user.Email = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmailValidationNoAtOrDot_Test()
        {
            var user = new UserDto();
            user.Email = "jacob";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmailValidationNoAt_Test()
        {
            var user = new UserDto();
            user.Email = "jacobcholewa.dk";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmailValidationNoDot_Test()
        {
            var user = new UserDto();
            user.Email = "jacob@itu";
        }   
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmailValidationNumbersInNamespace_Test()
        {
            var user = new UserDto();
            user.Email = "jacob@itu.123";
        }
    }
}
