using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RecoLibrary;


namespace RecoTest
{
    [TestClass]
    public class RecoTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Reco.GetInstance().purgeRecoInstance();
        }

        [TestMethod]
        public void AddImageAndSaveTest()
        {
            Assert.IsFalse(Reco.GetInstance().AddImage("notFound.png", "notFoundImage"));
            Assert.IsTrue(Reco.GetInstance().AddImage("Resources/testImage1.png", "testImage1"));
            Reco.GetInstance().AddImage("Resources/testImage2.png", "testImage2");
            Reco.GetInstance().AddImage("Resources/testImage3.png", "testImage3");
            Reco.GetInstance().AddImage("Resources/testImage4.png", "testImage4");
            Assert.AreEqual(4, Reco.GetInstance().GetNumberOfRecords());
            Assert.IsTrue(Reco.GetInstance().Save("Resources/sampleRepository.bin"));
        }
        [TestMethod]
        public void GetInstanceAndLoadTest()
        {
            //Check consistency of Reco instance and records field without input file
            Assert.IsNotNull(Reco.GetInstance());
            Assert.AreEqual(0,Reco.GetInstance().GetNumberOfRecords());

            //load a repository.bin test file
            Assert.IsTrue(Reco.GetInstance().Load("Resources/sampleRepository.bin"));

            //Check consistency of Reco instance and records field with input test file
            Assert.AreEqual(4, Reco.GetInstance().GetNumberOfRecords());

        }

        
    }
}
