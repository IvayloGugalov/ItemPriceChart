using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace ItemPriceCharts.XmReaderWriter.XmlActions
{
    public static class XmlWriteData
    {
        public static XmlWriter CreateWriter(string filePath)
        {
            return XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true, NewLineOnAttributes = true  });
        }

        public static void WriteTo(this XmlWriter writer, string localName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.WriteElementString(localName, value);
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

        public static void WriteToAttribute(this XmlWriter writer, string localName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                writer.WriteAttributeString(localName, value);
            }
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

            var userNode = xmlDoc.SelectSingleNode(node);
            userNode.InnerText = value;

            xmlDoc.Save(filePath);
        }

        public static void UpdateElementsValue(string filePath, IDictionary<string, string> nodesValues)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            foreach (var nodeValue in nodesValues)
            {
                var userNode = xmlDoc.SelectSingleNode(nodeValue.Key);
                userNode.InnerText = nodeValue.Value;
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
    }

    public static class XmlClearData
    {
        public static XDocument CreateXmlDocument(string filePath)
        {
            return XDocument.Load(filePath);
        }

        public static void RemoveElementsFromDocument(this XDocument document, params string[] elements)
        {
            foreach (var element in elements)
            {
                document.Descendants(element).Remove();
            }
        }
    }
}
