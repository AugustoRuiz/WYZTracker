using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace WYZTracker
{
    public class SerializationUtils
    {
        /// <summary>
        /// Serializes and object to string
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">The object to serialize</param>
        /// <returns>The string representing the object.</returns>
        public static string Serialize<T>(T obj)
        {
            StringWriter stream = new StringWriter();
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            formatter.Serialize(stream, obj);
            return stream.ToString();
        }

        /// <summary>
        /// Serializes an object over the specified stream
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">The object to serialize</param>
        /// <param name="stream">The stream the object will be serialized into.</param>
        public static void Serialize<T>(T obj, Stream stream)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            formatter.Serialize(stream, obj);
        }

        /// <summary>
        /// Deserializes an object from the specified stream.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="stream">The stream with the serialized object.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(Stream stream)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            T result = (T) formatter.Deserialize(stream);
            return result;
        }

        /// <summary>
        /// Deserializes an object from the specified string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="value">The string with the serialized object.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(string value)
        {
            StringReader reader = new StringReader(value);
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            T result = (T)formatter.Deserialize(reader);
            return result;
        }

        public static T Clone<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            Serialize<T>(obj, stream);
            stream.Seek(0, SeekOrigin.Begin);
            T cloned = Deserialize<T>(stream);
            stream.Dispose();
            return cloned;
        }
    }
}
