using System;
using System.IO;

namespace ItemPriceCharts.XmReaderWriter.XmlActions
{
    public class XmlCreateFile
    {
        public static readonly string XML_FILE_PATH = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"\ItemPriceCharts\myXmlFile.xml");

        public static bool EnsureXmlFileExists()
        {
            if (!File.Exists(XML_FILE_PATH))
            {
                File.Create(XML_FILE_PATH).Close();
            }

            return true;
        }
    }
}
