using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor;
using EnCorTest.BetaModule;

namespace EnCorTest
{
    /// <summary>
    /// Summary description for RuntimeTest
    /// </summary>
    [TestClass]
    public class RuntimeTest
    {
        public RuntimeTest()
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
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
            Runtime.Start();
        }


        [ClassCleanup()]
        public static void MyClassCleanup() 
        {
            Runtime.Stop();
        }


        [TestMethod]
        public void BeginRequestTest()
        {
            Runtime.InitializeSecurityContext();
        }
        
        
        [TestMethod]
        [DeploymentItem("EnCor.Config")]
        public void ServiceCallTest()
        {
            Runtime.InitializeSecurityContext();
            IBetaService target = Runtime.GetService<IBetaService>();

            Assert.IsNotNull(target);

            string message = "abc";

            Assert.AreEqual(message, target.Echo(message));
        }
    }
}
