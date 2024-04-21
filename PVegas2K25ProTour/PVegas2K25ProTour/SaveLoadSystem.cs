using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace PVegas2K25ProTour
{
    public static class SaveLoadSystem
    {
        public const string FILEPATH = "PlayerStats.xml";
        // public const string FILEPATH = "C:\\Life Plans\\UW Platteville\\UWP Classes Spring 2024\\Intermediate SE\\compu-force\\PVegas2K25ProTour\\PVegas2K25ProTour\\bin\\PlayerStats.xml";
        public static bool NewLineOnAttributes { get; set; }
        /// <summary>
        /// Serializes an object to an XML string, using the specified namespaces.
        /// </summary>
        /// 

        public static void Save(PlayerRecord playerRecord)
        {
            try
            {
                SaveLoadSystem.ToXmlFile(playerRecord, FILEPATH);
                Debug.WriteLine("XML File successfully saved!");
            }
            catch 
            {
                Debug.WriteLine("Failed to save to the xml file.");
            }
        }

        public static T Load<T>()
        {
            try
            {
                // Load the object from the XML file using the specified type T
                T playerRecord = FromXmlFile<T>(FILEPATH);

                Debug.WriteLine("File successfully loaded");

                return playerRecord;
            }
            catch (FileNotFoundException)
            {
                // If the file doesn't exist, create a new instance of T and return it
                Debug.WriteLine("File not found. Creating a new instance.");
                return Activator.CreateInstance<T>();
            }
            catch (Exception ex)
            {
                // If there's any other error, handle it appropriately (e.g., log, throw, etc.)
                Debug.WriteLine($"Error loading file: {ex.Message}");
                // For now, returning a new instance of T
                return Activator.CreateInstance<T>();
            }
        }

        //***********************************SPECIAL NOTE!*************************************
        // All code below this point was created by user: "Robert Harvey" On Stack Overflow
        // This code was posted on 08-18-14 at 17:42
        // Here is a link to that resource: https://stackoverflow.com/questions/25368083/using-xml-files-to-store-data-in-c-sharp
        // ************************************************************************************
        public static string ToXml(object obj, XmlSerializerNamespaces ns)
        {
            Type T = obj.GetType();

            var xs = new XmlSerializer(T);
            var ws = new XmlWriterSettings { Indent = true, NewLineOnAttributes = NewLineOnAttributes, OmitXmlDeclaration = true };

            var sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, ws))
            {
                xs.Serialize(writer, obj, ns);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Serializes an object to an XML string.
        /// </summary>
        public static string ToXml(object obj)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            return ToXml(obj, ns);
        }

        /// <summary>
        /// Deserializes an object from an XML string.
        /// </summary>
        public static T FromXml<T>(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xml))
            {
                return (T)xs.Deserialize(sr);
            }
        }

        /// <summary>
        /// Deserializes an object from an XML string, using the specified type name.
        /// </summary>
        public static object FromXml(string xml, string typeName)
        {
            Type T = Type.GetType(typeName);
            XmlSerializer xs = new XmlSerializer(T);
            using (StringReader sr = new StringReader(xml))
            {
                return xs.Deserialize(sr);
            }
        }

        /// <summary>
        /// Serializes an object to an XML file.
        /// </summary>
        public static void ToXmlFile(object obj, string filePath)
        {
            var xs = new XmlSerializer(obj.GetType());
            var ns = new XmlSerializerNamespaces();
            var ws = new XmlWriterSettings { Indent = true, NewLineOnAttributes = NewLineOnAttributes, OmitXmlDeclaration = true };
            ns.Add("", "");

            using (XmlWriter writer = XmlWriter.Create(filePath, ws))
            {
                xs.Serialize(writer, obj, ns);
            }
        }

        /// <summary>
        /// Deserializes an object from an XML file.
        /// </summary>
        public static T FromXmlFile<T>(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            try
            {
                var result = FromXml<T>(sr.ReadToEnd());
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("There was an error attempting to read the file " + filePath + "\n\n" + e.InnerException.Message);
            }
            finally
            {
                sr.Close();
            }
        }
    }
}
