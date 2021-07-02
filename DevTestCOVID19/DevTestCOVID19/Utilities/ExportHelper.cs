using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DevTestCOVID19.Utilities
{
    public abstract class ExportHelper
    {
        protected byte[] GetArrayByteToExportCSV(List<object> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string[] customer = (string[])list[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    sb.Append(customer[j] + ',');
                }
                sb.Append("\r\n");
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        protected byte[] GetArryByteToExportXML<TData>(string rootName, string itemName, List<TData> model)
        {
            Type type = typeof(TData);
            var properties = type.GetProperties();

            using (MemoryStream stream = new MemoryStream())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.ASCII);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 4;

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(rootName);

                foreach (var item in model)
                {
                    xmlWriter.WriteStartElement(itemName);
                    foreach (PropertyInfo info in properties)
                    {
                        xmlWriter.WriteElementString(info.Name, info.GetValue(item).ToString());
                    }
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
                byte[] byteArray = stream.ToArray();
                xmlWriter.Close();

                return byteArray;
            }
        }
    }
}
