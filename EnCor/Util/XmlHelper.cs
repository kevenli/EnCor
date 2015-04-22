using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace EnCor.Util
{
    public static class XmlHelper
    {
        public static XmlReader BuildXmlReader(string xmlContent)
        {
            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);

            // Create the XmlParserContext.
            XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
            return new XmlTextReader(xmlContent, XmlNodeType.Element, context);
        }
    }
}
