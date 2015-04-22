using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor.Security;

namespace EnCorTest.Security
{
    /// <summary>
    /// Summary description for TokenProviderFactoryTest
    /// </summary>
    [TestClass]
    public class TokenProviderFactoryTest : ConfigTestBase
    {
        public TokenProviderFactoryTest()
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
        public void BuildMemoryTokenProviderTest()
        {
            var configReader = this.GetXmlSection("BuildMemoryTokenProviderTest");

            MemoryCachedTokenProviderConfig config = new MemoryCachedTokenProviderConfig();
            config.DeserializeElement(configReader);

            AuthenticationProviderFactory target = new AuthenticationProviderFactory();

            IAuthenticationProvider provider = target.Create(null, config);


            Assert.IsNotNull(provider);

        }
    }
}
