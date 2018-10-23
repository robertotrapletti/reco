using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.XFeatures2D;

namespace PrivateRecord
{
    /// <summary>
    /// Object to save the result of a processed image. It saves the features detected, their descriptors and the realtive associated name.
    /// </summary>
    [Serializable()]
    public class Record
    {
        ///////////////////////////////////////////////////////////
        ///Fields
        //////////////////////////////////////////////////////////

        private readonly String name;
        private readonly VectorOfKeyPoint keyPoint;
        private readonly Mat descriptors;

        public VectorOfKeyPoint KeyPoints { get { return keyPoint; } }
        public String Name { get { return name; } }
        public Mat Descriptors { get { return descriptors; } }

        ///////////////////////////////////////////////////////////
        /// Constructors & Factory pattern
        //////////////////////////////////////////////////////////

        // Private constructor
        private Record(String name) {
            this.name = name;
            this.keyPoint = new VectorOfKeyPoint();
            this.descriptors = new Mat();
        }

        /// <summary>
        /// Create a new Record Object from an image
        /// </summary>
        /// <param name="path">The image path</param>
        /// <param name="name">The desired associated name</param>
        /// <returns>Return a Record Object</returns>
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
