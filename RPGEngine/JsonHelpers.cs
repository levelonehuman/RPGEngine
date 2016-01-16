using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine
{
    /// <summary>
    /// Methods for reading/writing json data for the game
    /// </summary>
    public static class JsonHelpers
    {        
        private const string dataPath = @"../../..\RPGEngine\Data\";

        public static List<T> ReadFileToList<T>(this string fileName)
        {
            string text = File.ReadAllText(dataPath + fileName);
            Dictionary<string, List<T>> data = JsonConvert.DeserializeObject<Dictionary<string, List<T>>>(text);

            return data[GetKeyFromFileName(fileName)];            
        }
        public static Dictionary<string, ObservableCollection<T>> ReadFileToCollection<T>(this string fileName)
        {
            string text = File.ReadAllText(dataPath + fileName);                        

            Dictionary<string, ObservableCollection<T>> data = JsonConvert.DeserializeObject<Dictionary<string, ObservableCollection<T>>>(text);            

            return data;
        }

        //public static ObservableCollection<T> GetCollectionFromFile<T>(string fileName)
        //{
        //    Dictionary<string, ObservableCollection<T>> data = ReadFile<T>(fileName);
        //    ObservableCollection<T> collection = new ObservableCollection<T>(data[GetKeyFromFileName(fileName)]);

        //    return collection;
        //}

        public static void SaveFile(string fileName, object value)
        {
            string text = JsonConvert.SerializeObject(value);

            File.WriteAllText(dataPath + fileName, text);
        }

        private static string GetKeyFromFileName(string fileName)
        {
            return fileName.Split('.')[0];
        }
    }
}
