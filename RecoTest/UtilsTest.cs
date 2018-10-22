using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecoLibrary;

namespace RecoTest
{
    [TestClass]
    public class UtilsTes
    {
        [TestMethod]
        public void SortDictionaryTest() {
            List<KeyValuePair<string, int>> unsorted = new List<KeyValuePair<string, int>>();
            unsorted.Add(new KeyValuePair<string, int>("secondo", 2));
            unsorted.Add(new KeyValuePair<string, int>("terzo", 3));
            unsorted.Add(new KeyValuePair<string, int>("primo", 1));

            Utility.sortList(unsorted);

            Assert.IsTrue(unsorted[0].Value == 1);
            Assert.IsTrue(unsorted[1].Value == 2);
            Assert.IsTrue(unsorted[2].Value == 3);
        }
    }
}
