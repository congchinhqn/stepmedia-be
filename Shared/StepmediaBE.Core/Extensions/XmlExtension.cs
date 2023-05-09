using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Metatrade.Core.Extensions
{
    public static class XmlExtension
    {
        public static string SerializeXML<T>(this T value) where T : class
        {
            if (value == null) return string.Empty;

            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, value);
                    return stringWriter.ToString();
                }
            }
        }
    }
}