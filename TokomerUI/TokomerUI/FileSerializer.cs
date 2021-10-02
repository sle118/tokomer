using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Windows.Forms;
using System.ComponentModel;
using MahApps.Metro.Controls;

namespace MyUtilities
{
  
    public static class FileSerializer
    {
        public static void Serialize( string filename, object objectToSerialize )
        {
            if (objectToSerialize == null)
                throw new ArgumentNullException("objectToSerialize cannot be null");
            Stream stream = null;
            try
            {
                stream = File.Open(filename, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }


        public static T Deserialize<T>( string filename, BackgroundWorker worker = null)
        {
            T objectToSerialize = default(T);
            Stream stream = null;
            try
            {
                stream = File.Open(filename, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (T)bFormatter.Deserialize(stream);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                worker.ReportProgress(50);
            }
            return objectToSerialize;
        }
    }

}
