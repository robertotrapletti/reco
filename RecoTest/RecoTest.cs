using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RecoLibrary;
using System.Collections.Generic;

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
            Assert.IsTrue(Reco.GetInstance().Save("Resources/sampleRepo.json"));
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

        [TestMethod]
        public void GetNameTest()
        {
           Reco.GetInstance().Load("Resources/sampleRepository.bin");
           string name=Reco.GetInstance().GetName("Resources/imageToFind.jpeg");
           Assert.AreEqual(name, "testImage1");
        }

        [TestMethod]
        public void GetNNameTest()
        {
            Reco.GetInstance().Load("Resources/sampleRepository.bin");
            List<string> names = Reco.GetInstance().GetNNames("Resources/imageToFind.jpeg",2);
            Assert.AreEqual(names[0], "testImage1");
            Assert.AreEqual(names[1], "testImage2");
        }

        [TestMethod]
        public void RemoveTest()
        {
            Reco.GetInstance().Load("Resources/sampleRepository.bin");
            Assert.IsTrue(Reco.GetInstance().RemoveImage("testImage1"));
        }

        [TestMethod]
        public void IsMonitorTest()
        {
            Assert.IsTrue(Reco.GetInstance().isMonitor("Resources/monitor.jpg"));
            Assert.IsFalse(Reco.GetInstance().isMonitor("Resources/random.jpg"));
        }
        
    }
}
