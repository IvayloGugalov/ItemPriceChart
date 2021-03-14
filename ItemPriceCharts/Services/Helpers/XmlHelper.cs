using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ItemPriceCharts.Services.Helpers
{
    public static class XmlHelper
    {
        #region XmlWriter extensions
        public static XmlWriter CreateWriter(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true
            };

            return XmlWriter.Create(filePath, writerSettings);
        }

        public static XmlWriter CreateNewer(string filePath, out FileStream stream)
        {
            FileStream s = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineOnAttributes = true
            };

            stream = s;

            return XmlWriter.Create(s, writerSettings);
        }

        public static void WriteTo(this XmlWriter writer, string localName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.WriteElementString(localName, value);
            }
        }

        public static void WriteToAttribute(this XmlWriter writer, string localName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.WriteAttributeString(localName, value);
            }
        }

        public static void WriteTo(this XmlWriter writer, string localName, int value)
        {
            writer.WriteElementString(localName, value.ToString());
        }

        public static void WriteTo(this XmlWriter writer, string localName, bool value)
        {
            writer.WriteElementString(localName, value.ToString());
        }

        /// <summary>
        /// Updates the xml element(node) with the specified value.
        /// node = "${/nameof(parent)}/{nameof(element)}"
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="node"></param>
        /// <param name="value"></param>
        public static void UpdateElementValue(string filePath, string node, string value)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            var userNodes = xmlDoc.SelectNodes(node);
            foreach (XmlNode userNode in userNodes)
            {
                userNode.InnerText = value;
            }
            xmlDoc.Save(filePath);
        }

        public static IDisposable WriteElementBody(this XmlWriter writer, string elementName)
        {
            return new EndElementWriter(writer, elementName);
        }

        private class EndElementWriter : IDisposable
        {
            private readonly XmlWriter xmlWriter;

            public EndElementWriter(XmlWriter xmlWriter, string elementName)
            {
                this.xmlWriter = xmlWriter;
                this.xmlWriter.WriteStartElement(elementName);
            }

            public void Dispose()
            {
                this.xmlWriter.WriteEndDocument();
                this.xmlWriter.Close();
                this.xmlWriter.Dispose();
            }
        }
        #endregion

        #region XmlReader extensions
        public static XmlReader CreateReader(string filePath)
        {
            if (File.Exists(filePath))
            {
                return XmlReader.Create(filePath);
            }

            throw new NullReferenceException($"File path is not existing {filePath}");
        }

        public static void Read(this XmlReader reader, IDictionary<string, Action> tagReaders)
        {
            var depth = reader.Depth;

            if (reader.IsEmptyElement)
            {
                reader.Read();
                return;
            }

            while (reader.Depth > depth - 1)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Depth < depth + 2)
                {
                    if (tagReaders.TryGetValue(reader.LocalName, out var tagReader))
                    {
                        tagReader();
                        continue;
                    }
                    else if (!reader.Read())
                    {
                        break;
                    }    
                }
                else if (!reader.Read())
                {
                    break;
                }
            }

            reader.Close();
            reader.Dispose();
        }
        #endregion
    }
}
