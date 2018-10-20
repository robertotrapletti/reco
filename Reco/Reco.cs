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

namespace RecoLibrary
{
    /// <summary>
    /// Main library class used to expose the API
    /// </summary>
    public class Reco
    {
        private static Reco instance;
        private List<Record> records;

        private Reco() {
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
                records = new List<Record>();
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
            return false;
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
        public String GetName(String imagePath) {
            return null;
        }

        /// <summary>
        /// Get the firsts n associated names from the comparation of the given image with 
        /// all Records inside the repository, sorted from the most similar
        /// </summary>
        /// <param name="imagePath">Path of the image to compare</param>
        /// <param name="nResults">Expected number of names to return</param>
        /// <returns>Return list of names or an empty list</returns>
        public List<String> GetNNames(String imagePath, int nResults) {
            return null;
        }
      
    }
}
