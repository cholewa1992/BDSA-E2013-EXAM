using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System.Collections.Generic;

namespace UtilUnitTest
{
    [TestClass]
    public class EncoderUnitTest
    {
        [TestMethod]
        public void Test_Encoder_EncodingAndDecoding()
        {
            //A simple test to check that the encoder actually encodes and decodes to the same string value.

            String inputString = "Die Hard: Mega Hard";

            byte[] encodedString = Encoder.Encode(inputString);

            Assert.AreEqual(inputString, Encoder.Decode(encodedString));
        }
    }
}
