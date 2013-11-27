using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;

namespace WebServerUnitTest
{   
    [TestClass]
    public class MovieDtoTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidNull()
        {
            var movie = new MovieDto();
            movie.Year = null;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidEmpty()
        {
            var movie = new MovieDto();
            movie.Year = "";
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidSpaceChar()
        {
            var movie = new MovieDto();
                movie.Year = " ";
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidChar()
        {
            var movie = new MovieDto();
                movie.Year = "qwertyuiopasdfghjklzxcvbnm";
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidSpecialChar()
        {
            var movie = new MovieDto();
                movie.Year = "!#¤%&/()=?+-*^¨";
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidNegativeYear()
        {
            var movie = new MovieDto();
                movie.Year = "-1989";
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidCharInFront()
        {
            var movie = new MovieDto();
                movie.Year = "year2012";
        }

        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidFiveDigit()
        {
            var movie = new MovieDto();
                movie.Year = "20012";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestYearInputInValidThreeDigit()
        {
            var movie = new MovieDto();
            movie.Year = "209";
        }

        [TestMethod]
        public void TestYearInputValid()
        {
            var movie = new MovieDto();
            movie.Year = "1992";
            movie.Year = "2014";
            movie.Year = "0000";
            movie.Year = "9999";
        }


    }
}
