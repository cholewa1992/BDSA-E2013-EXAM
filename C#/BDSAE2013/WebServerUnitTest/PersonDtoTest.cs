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
        public void TestNameInputValidationInvalidName_Test()
        {
            var person = new PersonDto();
            person.Name = "";
        }
    }
}
