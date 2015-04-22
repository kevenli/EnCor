using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Security;
using EnCor;
using EnCor.Security;
using System.Security.Principal;
using EnCor.Security.Credentials;

namespace EnCorTest.Security
{
    /// <summary>
    /// Summary description for MembershipAuthenticationProvider
    /// </summary>
    [TestClass]
    public class MembershipAuthenticationProviderTest
    {
        public MembershipAuthenticationProviderTest()
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
        public void AuthenticateWithValidPasswordTest()
        {
            string username = "keven";
            string password = "keven";

            MembershipProvider provider = new MembershipProviderMock();
            AuthenticationProvider target = new MembershipAuthenticationProvider(provider, MembershipAuthenticationProvider.MembershipIdentityType.Username);

            UsernamePasswordCredential credential = new UsernamePasswordCredential(username, password );

            UserIdentity identity = target.Authenticate(credential);
            Assert.IsNotNull(identity);
        }

        [TestMethod]
        public void AuthenticateWithInvalidPasswordTest()
        {
            string username = "keven";
            string password = "kkkkk";

            MembershipProvider provider = new MembershipProviderMock();
            AuthenticationProvider target = new MembershipAuthenticationProvider(provider, MembershipAuthenticationProvider.MembershipIdentityType.Username);

            UsernamePasswordCredential credential = new UsernamePasswordCredential(username, password);

            UserIdentity identity = target.Authenticate(credential);
            Assert.IsNull(identity);
        }

        [TestMethod]
        public void IdentityType_Username_Test()
        {
            string username = "keven";
            string password = "keven";

            MembershipProvider provider = new MembershipProviderMock();
            AuthenticationProvider target = new MembershipAuthenticationProvider(provider, MembershipAuthenticationProvider.MembershipIdentityType.Username);

            UsernamePasswordCredential credential = new UsernamePasswordCredential(username, password);

            UserIdentity identity = target.Authenticate(credential);
            Assert.AreEqual(identity.Id, username);
            Assert.IsNotNull(identity);
        }

        [TestMethod]
        public void IdentityType_ProviderUserKey_Test()
        {
            string username = "keven";
            string password = "keven";
            string provideruserkey = "key";

            MembershipProvider provider = new MembershipProviderMock();
            AuthenticationProvider target = new MembershipAuthenticationProvider(provider, MembershipAuthenticationProvider.MembershipIdentityType.ProviderUserKey);

            UsernamePasswordCredential credential = new UsernamePasswordCredential(username, password);

            UserIdentity identity = target.Authenticate(credential);
            Assert.AreEqual(identity.Id, provideruserkey);
            Assert.IsNotNull(identity);
        }


    }
}
