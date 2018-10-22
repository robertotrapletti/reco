using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecoLibrary
{
    public class Utility
    {
        public static void sortList(List<KeyValuePair<string, int>> list) {
            list.Sort(
                delegate(KeyValuePair<string,int> pair1,
                KeyValuePair<string,int> pair2) {
                    return pair1.Value.CompareTo(pair2.Value);
                }
            );
          
        }
    }
}
