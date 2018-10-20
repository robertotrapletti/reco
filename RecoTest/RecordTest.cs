using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrivateRecord;

namespace RecoTest
{
    [TestClass]
    public class RecordTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void createFromNotFoundTest() {
            Record emptyRecord = Record.CreateFromImage("Resources/notFoundImage.png","notFoundImage"); 
        }

        [TestMethod]
        public void createFromImageTest(){
            Record record = Record.CreateFromImage("Resources/testImage1.png", "testImage1");
            Assert.IsNotNull(record);
        }
    }
}
