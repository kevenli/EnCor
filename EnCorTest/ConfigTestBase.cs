using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;

namespace EnCorTest
{
    public class ConfigTestBase
    {
        private XmlReader LoadXmlData()
        {
            string reourceFileName = string.Format("{0}.xml", this.GetType().FullName);
            Stream fs = Assembly.GetExecutingAssembly().GetManifestResourceStream(reourceFileName);
            return new XmlTextReader(fs);
        }

        protected XmlReader GetXmlSection(string sectionName)
        {
            XmlReader xml = LoadXmlData();
            xml.ReadStartElement("test");
            while (!xml.EOF)
            {
                if (xml.IsStartElement(sectionName))
                {
                    xml.ReadStartElement();
                    break;
                }
                else
                {
                    xml.ReadInnerXml();
                }
            }
            xml.Read();


            return xml;
        }
    }
}
