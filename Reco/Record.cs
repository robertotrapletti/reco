using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.XFeatures2D;

namespace PrivateReco
{
    [Serializable]
    class Record
    {
        ///////////////////////////////////////////////////////////
        ///Fields
        //////////////////////////////////////////////////////////

        private readonly String name;
        private readonly VectorOfKeyPoint keyPoint;
        private readonly Mat descriptors;

        ///////////////////////////////////////////////////////////
        /// Constructors & Factory pattern
        //////////////////////////////////////////////////////////

        // Private constructor
        private Record(String name) {
            this.name = name;
            this.keyPoint = new VectorOfKeyPoint();
            this.descriptors = new Mat();
        }

        public static Record CreateFromImage(String path, String name) {
            // new Record
            Record newRecord = new Record(name);

            //Preprocessing of the image
            Mat image = CvInvoke.Imread(path, ImreadModes.Color);
            UMat uImage = image.GetUMat(AccessType.Read);
            SURF surf = new SURF(400);
            surf.DetectAndCompute(uImage, null, newRecord.keyPoint, newRecord.descriptors, false);

            return newRecord;
        }

        ///////////////////////////////////////////////////////////
        /// Getter & Setter
        //////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////
        /// Methods
        //////////////////////////////////////////////////////////

    }
}
