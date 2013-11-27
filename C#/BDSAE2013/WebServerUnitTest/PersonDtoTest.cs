using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;

namespace WebServerUnitTest
{
    [TestClass]
    class PersonDtoTest
    {
        [TestMethod]
        public void TestNameInputValidationValidName_Test()
        {
            var person = new PersonDto();
            person.Name = "John";
            person.Name = "John Doe";
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNameInputValidationInvalidNameEmpty_Test()
        {
            var person = new PersonDto();
            person.Name = "";
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNameInputValidationInvalidNameSpaceChar_Test()
        {
            var person = new PersonDto();
            person.Name = " ";
        }
    }
}
