using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using System.Xml;
using EnCor.Config;
using EnCorTest.BetaModule;
using System.Collections;
using EnCor.ModuleLoader;

namespace EnCorTest
{
    /// <summary>
    /// Summary description for ServiceConfigTest
    /// </summary>
    [TestClass]
    public class ServiceConfigTest
    {
        public ServiceConfigTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private XmlReader LoadXmlData()
        {
            Stream fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("EnCorTest.ServiceConfigTest.xml");
            return new XmlTextReader(fs);
        }

        private XmlReader GetXmlSection(string sectionName)
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


            return xml;
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ComponentInterfaceTest1()
        {
            XmlReader xml = GetXmlSection("ComponentInterfaceTest1");
            xml.Read();

            ServiceConfig target = new ServiceConfig();
            target.DeserializeElement(xml);

            Assert.IsNotNull(target.ComponentsConfig[0].InterfaceType);

            Assert.AreEqual( target.ComponentsConfig[0].InterfaceType, typeof(BetaDataProvider) );
        }

        [TestMethod]
        public void ComponentInterfaceTest2()
        {
            XmlReader xml = GetXmlSection("ComponentInterfaceTest2");
            xml.Read();

            ServiceConfig target = new ServiceConfig();
            target.DeserializeElement(xml);

            Assert.IsNotNull(target.ComponentsConfig[0].InterfaceType);

            Assert.AreEqual( target.ComponentsConfig[0].InterfaceType, target.ComponentsConfig[0].Type );
        }

        [TestMethod]
        public void ComponentInterfaceTest3()
        {
            XmlReader xml = GetXmlSection("ComponentInterfaceTest3");
            xml.Read();

            ServiceConfig target = new ServiceConfig();
            target.DeserializeElement(xml);

            Assert.IsNotNull(target.ComponentsConfig[0].InterfaceType);

            Assert.AreEqual(target.ComponentsConfig[0].InterfaceType, typeof(IBetaDataProvider));
        }

        [TestMethod]
        public void ServiceInterfaceTest()
        {
            XmlReader xml = GetXmlSection("ServiceInterfaceTest");
            xml.Read();

            ServiceConfig target = new ServiceConfig();
            target.DeserializeElement(xml);
            target.VerifyConfig();
            Assert.IsTrue(target.Interfaces.Length == 1);

            Assert.AreEqual(target.Interfaces[0], typeof(BetaService));
        }

        [TestMethod]
        public void ServiceInterfaceTest2()
        {
            XmlReader xml = GetXmlSection("ServiceInterfaceTest2");
            xml.Read();

            ServiceConfig target = new ServiceConfig();
            target.DeserializeElement(xml);
            target.VerifyConfig();

            Assert.IsTrue(target.Interfaces.Length == 1);

            
        }
    }
}
