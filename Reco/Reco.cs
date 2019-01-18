using System;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using PrivateRecord;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Resources;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.CvEnum;
using TensorFlow;
using System.Net;
using System.IO.Compression;
using Newtonsoft.Json;

namespace RecoLibrary
{
    /// <summary>
    /// Main library class used to expose the API
    /// </summary>
    public class Reco
    {
        private static Reco instance;
        private List<Record> records;
        static string dir, modelFile, labelsFile;

        private Reco() {
            records = new List<Record>();
            if (dir == null)
            {
                dir = "/tmp";
                //Error ("Must specify a directory with -m to store the training data");
            }
            ModelFiles(dir);
            this.Load();
        }

        
        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns>Return the instance of the Reco class</returns>
        public static Reco GetInstance() {
            if (instance == null)
            {
                instance = new Reco();
                return instance;
            }
            else { return instance; }
        }
        
        public void purgeRecoInstance() {
            instance = null;
        }

        
        //API------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Load the repository with the saved Records
        /// </summary>
        /// <param name="path">Path of the repository</param>
        /// <returns>Return true if the repository is correctly loaded, else create a new list of Records and return false</returns>
        public bool Load(String path= "repository.bin") {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                records = (List<Record>)formatter.Deserialize(stream);
                stream.Close();
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        /// <summary>
        /// Save the current list of Records to a repository file
        /// </summary>
        /// <param name="path">Path where to save the repository</param>
        /// <returns>Return true if the repository is successfully saved, else false</returns>
        public bool Save(String path = "repository.bin") {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, records);
                stream.Close();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Create a new Record Object from an image and adds it to the repository
        /// </summary>
        /// <param name="imagePath">Image location</param>
        /// <param name="name">Associated name</param>
        /// <returns>Return true if the Record is created and successfully added to the repository, else false</returns>
        public bool AddImage(String imagePath, String name) {
            Record record;
            try
            {
                record = Record.CreateFromImage(imagePath, name);
                records.Add(record);
                return true;
            }
            catch (Exception e) {
                return false;
            }
            
        }

        /// <summary>
        /// Remove a Record Object from the repository
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Return true if the Record is found and correctly removed from the repository, else false</returns>
        public bool RemoveImage(String name) {
            return records.Remove(records.Find(r=>r.Name.Equals(name)));
        }

        /// <summary>
        /// Get the number of Records into the repository
        /// </summary>
        /// <returns>Return the number of Records into the repository</returns>
        public int GetNumberOfRecords() {
            return records.Count;
        }

        /// <summary>
        /// Compares the given image with all Records inside the repository and return 
        /// the associated name of the most similar image.
        /// </summary>
        /// <param name="imagePath">Path of the image to compare</param>
        /// <returns>Return name if a match is found, else an empty String</returns>
        private List<KeyValuePair<String, int>> GetNameList(String imagePath)
        {
            Record processingRecord = Record.CreateFromImage(imagePath, "");
            var resultList = new List<KeyValuePair<String, int>>();
           
            records.ForEach(e => {
                VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
                int k = 2;
                double uniquenessThreshold = 0.8;
                Mat mask = new Mat();
                //TODO da approfondire questa sintassi
                using (Emgu.CV.Flann.LinearIndexParams ip = new Emgu.CV.Flann.LinearIndexParams())
                using (Emgu.CV.Flann.SearchParams sp = new SearchParams())
                using (DescriptorMatcher matcher = new FlannBasedMatcher(ip, sp))
                {
                    matcher.Add(e.Descriptors);

                    matcher.KnnMatch(processingRecord.Descriptors, matches, k, null);
                    mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                    mask.SetTo(new MCvScalar(255));
                    Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

                    // Calcluate the score based on matches size
                    int score = 0;
                    for (int i = 0; i < matches.Size; i++)
                    {
                        if (mask.GetData(i)[0] == 0) continue;
                        foreach (var item in matches[i].ToArray())
                            ++score;
                    }

                    // Add score and record's name to the map
                    resultList.Add(new KeyValuePair<string, int>(e.Name, score));
                }
            });

            Utility.sortList(resultList);
            return resultList;
        }

        /// <summary>
        /// Compares the given image with all Records inside the repository and return 
        /// the associated name of the most similar image.
        /// </summary>
        /// <param name="imagePath">Path of the image to compare</param>
        /// <returns>Return name if a match is found, else an empty String</returns>
        public String GetName(String imagePath) {
            var resultList = GetNameList(imagePath);
            return resultList[0].Key;
        }

        /// <summary>
        /// Get the firsts n associated names from the comparation of the given image with 
        /// all Records inside the repository, sorted from the most similar
        /// </summary>
        /// <param name="imagePath">Path of the image to compare</param>
        /// <param name="nResults">Expected number of names to return</param>
        /// <returns>Return list of names or an empty list</returns>
        public List<String> GetNNames(String imagePath, int nResults) {
            var resultList = GetNameList(imagePath).GetRange(0,nResults);
            var resultStringList = new List<String>();
            resultList.ForEach(r=>resultStringList.Add(r.Key));
            return resultStringList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public Boolean isMonitor(String imagePath) {
            var graph = new TFGraph();
            // Load the serialized GraphDef from a file.
            var model = File.ReadAllBytes(modelFile);

            graph.Import(model, "");

            var tensor = ImageUtil.CreateTensorFromImageFile(imagePath, TFDataType.Float);
            var session = new TFSession(graph);
            
                var labels = File.ReadAllLines(labelsFile);
                var runner = session.GetRunner();
                runner.AddInput(graph["input"][0], tensor);
                runner.Fetch(graph["MobilenetV2/Predictions/Reshape_1"][0]);

                var output = runner.Run();

                // Fetch the results from output:
                TFTensor result = output[0];


                // You can get the data in two ways, as a multi-dimensional array, or arrays of arrays, 
                // code can be nicer to read with one or the other, pick it based on how you want to process
                // it
                bool jagged = true;

                var bestIdx = 0;
                float p = 0, best = 0;

                if (jagged)
                {
                    var probabilities = ((float[][])result.GetValue(jagged: true))[0];
                    for (int i = 0; i < probabilities.Length; i++)
                    {
                        if (probabilities[i] > best)
                        {
                            bestIdx = i;
                            best = probabilities[i];
                        }
                    }

                }
                else
                {
                    var val = (float[,])result.GetValue(jagged: false);

                    // Result is [1,N], flatten array
                    for (int i = 0; i < val.GetLength(1); i++)
                    {
                        if (val[0, i] > best)
                        {
                            bestIdx = i;
                            best = val[0, i];
                        }
                    }
                }

                Console.WriteLine($"best match: [{bestIdx}] {best * 100.0}% {labels[bestIdx]}");


            return isMonitorOrSimilarLabel(labels[bestIdx]);

        }

        private bool isMonitorOrSimilarLabel(string v)
        {
            return v.Equals("Monitor", StringComparison.OrdinalIgnoreCase) ||
                v.Equals("Screen", StringComparison.OrdinalIgnoreCase) ||
                v.Equals("television", StringComparison.OrdinalIgnoreCase) ||
                v.Equals("website", StringComparison.OrdinalIgnoreCase);
        }

        static void ModelFiles(string dir)
        {
            modelFile = "Resources/model/mobilenet_v2_1.0_224_frozen.pb";
            labelsFile = "Resources/model/labels.txt";
            if (File.Exists(modelFile) && File.Exists(labelsFile))
                return;
        }
    }
}
