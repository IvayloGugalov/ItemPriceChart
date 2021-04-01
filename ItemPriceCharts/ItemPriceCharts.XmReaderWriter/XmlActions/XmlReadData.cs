using System;
using System.Collections.Generic;
using System.Xml;

using NLog;

namespace ItemPriceCharts.XmReaderWriter.XmlActions
{
    public static class XmlReadData
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(XmlReadData));

        public static void GetXmlElementValues(ref Dictionary<string, string> elementAndOutputValuePairs)
        {
            var reassignedDictionary = elementAndOutputValuePairs;

            using (var reader = XmlReader.Create(XmlCreateFile.XML_FILE_PATH))
            {
                var xmlTagReaders = new Dictionary<string, Action>();
                foreach (var item in elementAndOutputValuePairs)
                {
                    xmlTagReaders.Add(item.Key, () => reassignedDictionary[item.Key] = reader.ReadElementContentAsString());
                }

                try
                {
                    reader.ReadXmlFile(xmlTagReaders);
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        public static void ReadXmlFile(this XmlReader reader, IDictionary<string, Action> tagReaders)
        {
            try
            {
                var depth = reader.Depth;

                if (reader.IsEmptyElement)
                {
                    reader.Read();
                    return;
                }

                while (reader.Depth >= depth)
                {
                    if (reader.NodeType == XmlNodeType.Element)
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
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }
    }
}
