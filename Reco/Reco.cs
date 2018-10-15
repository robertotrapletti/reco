using System;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using PrivateReco;

namespace Reco
{
    public static class Reco
    {
        //API LIBRERIA
        public static bool init(this String i) {
            return true;
        }

        public static bool addImage(String path, String name) {
            Record record = Record.createFromImage(path, name);
            return true;
        }
    }
}
