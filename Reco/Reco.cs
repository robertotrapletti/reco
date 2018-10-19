using System;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using PrivateReco;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Resources;

namespace RecoLibrary
{
    public class Reco
    {
        private static Reco instance;
        private List<Record> records;

        private Reco() {
            this.Load();
        }

        /// <summary>
        /// Public Methods
        /// </summary>
        /// <returns></returns>
        public static Reco GetInstance() {
            if (instance == null)
            {
                instance = new Reco();
                return instance;
            }
            else { return instance; }
        }
       
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

        //API LIBRERIA
        public bool AddImage(String path, String name) {
            Record record;
            try
            {
                record = Record.CreateFromImage(path, name);
                return true;
            }
            catch (Exception e) {
                return false;
            }
            
        }

        public int getNumberOfRecords() {
            return records.Count;
        }
      
    }
}
