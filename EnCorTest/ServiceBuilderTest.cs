using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EnCor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor.Config;
using EnCor.ServiceLocator;
using EnCor.ModuleLoader;
using System.Xml;
using System.IO;
using System.Reflection;
using EnCorTest.BetaModule;

namespace EnCorTest
{
    /// <summary>
    /// Summary description for ServiceBuilderTest
    /// </summary>
    [TestClass]
    public class ServiceBuilderTest : ConfigTestBase
    {


        public ServiceBuilderTest()
        {
            //
            // TODO: Add constructor logic here
            //
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
        public void BuildServiceTest()
        {
            XmlReader xml = GetXmlSection("BuildServiceTest");

            ServiceConfig config = new ServiceConfig();
            config.DeserializeElement(xml);
            config.VerifyConfig();

            IServiceContainer container = new UnityServiceContainer();

            ServiceBuilder_Accessor target = new ServiceBuilder_Accessor(container);

            BizService expect = target.BuildService(config);

            Assert.IsNotNull(expect);

            Assert.IsTrue(config.Interfaces.Length > 0);
            foreach (Type type in config.Interfaces)
            {
                Assert.IsTrue(type.IsAssignableFrom(expect.GetType()));
            }

        }

        [TestMethod]
        public void BuildServiceComponentsTest()
        {
            XmlReader xml = GetXmlSection("BuildServiceComponentsTest");
            ServiceConfig config = new ServiceConfig();
            config.DeserializeElement(xml);


            IServiceContainer container = new UnityServiceContainer();

            ServiceBuilder_Accessor target = new ServiceBuilder_Accessor(container);

            BetaService expect = target.BuildService(config) as BetaService;

            Assert.IsNotNull(expect);

            Assert.IsNotNull(expect.DataProvider);
             
        }
    }
}
