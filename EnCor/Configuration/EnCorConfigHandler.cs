using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.IO;

namespace EnCor.Configuration
{
    public class EnCorConfigHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            string content = null;

            StringWriter sw = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = true });
            section.WriteTo(writer);
            writer.Flush();
            writer.Close();
            content = sw.ToString();
            return new FileEnCorConfig(CreateReader(content));
        }

        private XmlReader CreateReader(string xmlContent)
        {
            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);

            // Create the XmlParserContext.
            XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
            return new XmlTextReader(xmlContent, XmlNodeType.Document, context);
        }
    }
}
