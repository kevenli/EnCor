using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor.Security;
using System.Xml;
using EnCor.Config;
using EnCor.ObjectBuilder;
using System.Web.Security;
using System.Configuration;

namespace EnCorTest.Security
{
    /// <summary>
    /// Summary description for AuthenticationProviderFactoryTest
    /// </summary>
    [TestClass]
    public class AuthenticationProviderFactoryTest : ConfigTestBase
    {
        public AuthenticationProviderFactoryTest()
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
        public void CreateAuthenticationProviderTest()
        {
            XmlReader xml = GetXmlSection("CreateAuthenticationProviderTest");
            AuthenticationProviderConfigCollection configCollection = new AuthenticationProviderConfigCollection();
            configCollection.DeserializeElement(xml);
            BuilderContext builderContext = new BuilderContext();
            foreach (MembershipProvider membershipProvider in Membership.Providers)
            {
                builderContext.AddExtension<MembershipProvider>(membershipProvider.Name, membershipProvider); 
            }

            AuthenticationProviderFactory target = new AuthenticationProviderFactory();

            Assert.IsTrue(configCollection.Count > 0);
            foreach (AuthenticationProviderConfig config in configCollection)
            {
                IAuthenticationProvider provider = target.Create(builderContext, config);
                Assert.IsNotNull(provider);
            }

            
            
            
        }
    }
}
