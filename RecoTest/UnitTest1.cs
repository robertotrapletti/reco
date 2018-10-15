using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Reco;


namespace RecoTest
{
    [TestClass]
    public class RecordTest
    {
        [TestMethod]
        public void createFromImage()
        {
            Assert.IsFalse(Reco.Reco.addImage("test", "ciao"));
            Assert.IsTrue("a".init()); 
        }
    }
}
