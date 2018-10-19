using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RecoLibrary;


namespace RecoTest
{
    [TestClass]
    public class RecoTest
    {
        [TestMethod]
        public void GetInstanceAndLoadTest()
        {
            //Check consistency of Reco instance and records field without input file
            Assert.IsNotNull(Reco.GetInstance());
            Assert.AreEqual(0,Reco.GetInstance().getNumberOfRecords());

            //load a repository.bin test file
            Assert.IsTrue(Reco.GetInstance().Load("repository.bin"));

            //Check consistency of Reco instance and records field with input test file
            Assert.AreEqual(4, Reco.GetInstance().getNumberOfRecords());

        }

        [TestMethod]
        public void AddImageAndSaveTest()
        {
            Assert.IsFalse(Reco.GetInstance().AddImage("notFound.png","notFoundImage"));
            Assert.IsTrue(Reco.GetInstance().AddImage("Resources/testImage1.png","testImage"));
            Assert.IsTrue(Reco.GetInstance().Save());
        }
    }
}
