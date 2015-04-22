using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor.Security;
using System.Security.Principal;
using EnCor.Security.Credentials;

namespace EnCorTest.Security
{
    /// <summary>
    /// Summary description for WindowsAuthenticationProvider
    /// </summary>
    [TestClass]
    public class WindowsAuthenticationProviderTest
    {
        public WindowsAuthenticationProviderTest()
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
        public void Authenticate_Normal_Test()
        {
            string[] group = new string[] { @"BUILTIN\Users" };
            WindowsAuthenticationProvider target = new WindowsAuthenticationProvider(group);

            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            Assert.IsNotNull( target.Authenticate( new WindowsCredential(identity) ) );
            
        }

        [TestMethod]
        public void Authenticate_Failed_Test()
        {
            string[] group = new string[] { @"Some Not Existing Group" };
            WindowsAuthenticationProvider target = new WindowsAuthenticationProvider(group);

            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            Assert.IsNull(target.Authenticate(new WindowsCredential(identity)));
            
        }

        [TestMethod]
        public void Authenticate_MultiGroup_Test()
        {
            string[] group = new string[] { @"BUILTIN\Users", @"Some Not Existing Group" };

            WindowsAuthenticationProvider target = new WindowsAuthenticationProvider(group);

            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            Assert.IsNotNull(target.Authenticate(new WindowsCredential(identity)));
            
        }
    }
}
