using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace NolekSymbols.Helpers
{
    public static class SerializeHelper
    {
        /// <summary>
        ///     Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject">The object to serialize</param>
        /// <param name="fileName">The name of the file where the serialized object should be saved to</param>
        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) return;
            try
            {
                var xmlDocument = new XmlDocument();
                var serializer = new XmlSerializer(serializableObject.GetType());
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"EXCEPTION: " + ex.Message);
            }
        }

        /// <summary>
        ///     Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject">The object to serialize</param>
        /// <param name="fileName">The name of the file where the serialized object should be saved to</param>
        public static void SerializeObjectWithSubClasses<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) return;

            try
            {
                var types = AbstractClassHelper.GetAllTypesInNameSpace("NolekSymbols.Model.BE");
                var enumerable = types as Type[] ?? types.ToArray();
                var typeArray = new Type[enumerable.Length];

                var i = 0;
                foreach (var t in enumerable)
                {
                    typeArray[i] = t;
                    i++;
                }
                var xmlDocument = new XmlDocument();
                var serializer = new XmlSerializer(serializableObject.GetType(), typeArray);
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"EXCEPTION: " + ex.Message);
            }
        }


        /// <summary>
        ///     Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">The file to desrialize</param>
        /// <returns>The deserialized object</returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return default(T);

            var objectOut = default(T);
            try
            {
                var types = AbstractClassHelper.GetAllTypesInNameSpace("NolekSymbols.Model.BE");
                var enumerable = types as Type[] ?? types.ToArray();
                var typeArray = new Type[enumerable.Length];

                var i = 0;
                foreach (var t in enumerable)
                {
                    typeArray[i] = t;
                    i++;
                }
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                var xmlString = xmlDocument.OuterXml;
                using (var read = new StringReader(xmlString))
                {
                    var outType = typeof(T);
                    var serializer = new XmlSerializer(outType, typeArray);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T) serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"EXCEPTION: " + ex.Message);
            }
            return objectOut;
        }
    }
}