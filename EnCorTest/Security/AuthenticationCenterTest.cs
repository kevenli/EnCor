using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor.Security;
using EnCor.ObjectBuilder;
using EnCor;

namespace EnCorTest.Security
{
    /// <summary>
    /// Summary description for AuthenticationCenterTest
    /// </summary>
    [TestClass]
    public class AuthenticationCenterTest
    {
        public AuthenticationCenterTest()
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


        private AuthenticationCenter BuildAuthenticationCenter()
        {
            throw new NotImplementedException();
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

        ///// <summary>
        /////A test for GetSerializerForTransferCredential
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("EnCor.dll")]
        //public void GetSerializerForTransferCredentialTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    AuthenticationCenter_Accessor target = new AuthenticationCenter_Accessor(param0); // TODO: Initialize to an appropriate value
        //    TransferCredential credential = null; // TODO: Initialize to an appropriate value
        //    ITransferCredentialSerializer expected = null; // TODO: Initialize to an appropriate value
        //    ITransferCredentialSerializer actual;
        //    actual = target.GetSerializerForTransferCredential(credential);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetProviderForCredential
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("EnCor.dll")]
        //public void GetProviderForCredentialTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    AuthenticationCenter_Accessor target = new AuthenticationCenter_Accessor(param0); // TODO: Initialize to an appropriate value
        //    Credential credential = null; // TODO: Initialize to an appropriate value
        //    IAuthenticationProvider expected = null; // TODO: Initialize to an appropriate value
        //    IAuthenticationProvider actual;
        //    actual = target.GetProviderForCredential(credential);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GenerateTokenString
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("EnCor.dll")]
        //public void GenerateTokenStringTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    AuthenticationCenter_Accessor target = new AuthenticationCenter_Accessor(param0); // TODO: Initialize to an appropriate value
        //    EnCorIdentity identity = null; // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.GenerateTokenString(identity);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for BuildAuthenticationProviders
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("EnCor.dll")]
        //public void BuildAuthenticationProvidersTest()
        //{
        //    PrivateObject param0 = null; // TODO: Initialize to an appropriate value
        //    AuthenticationCenter_Accessor target = new AuthenticationCenter_Accessor(param0); // TODO: Initialize to an appropriate value
        //    AuthenticationProviderConfigCollection config = null; // TODO: Initialize to an appropriate value
        //    BuilderContext builderContext = null; // TODO: Initialize to an appropriate value
        //    IList<IAuthenticationProvider> expected = null; // TODO: Initialize to an appropriate value
        //    IList<IAuthenticationProvider> actual;
        //    actual = target.BuildAuthenticationProviders(config, builderContext);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Authenticate
        /////</summary>
        //[TestMethod()]
        //public void AuthenticateTest1()
        //{
        //    AuthenticationConfig config = null; // TODO: Initialize to an appropriate value
        //    AuthenticationCenter target = new AuthenticationCenter(config); // TODO: Initialize to an appropriate value
        //    TransferCredential transferCredential = null; // TODO: Initialize to an appropriate value
        //    AuthenticateToken expected = null; // TODO: Initialize to an appropriate value
        //    AuthenticateToken actual;
        //    actual = target.Authenticate(transferCredential);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Authenticate
        /////</summary>
        //[TestMethod()]
        //public void AuthenticateTest()
        //{
        //    AuthenticationConfig config = null; // TODO: Initialize to an appropriate value
        //    AuthenticationCenter target = new AuthenticationCenter(config); // TODO: Initialize to an appropriate value
        //    Credential credential = null; // TODO: Initialize to an appropriate value
        //    AuthenticateToken expected = null; // TODO: Initialize to an appropriate value
        //    AuthenticateToken actual;
        //    actual = target.Authenticate(credential);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for AuthenticationCenter Constructor
        /////</summary>
        //[TestMethod()]
        //public void AuthenticationCenterConstructorTest()
        //{
        //    AuthenticationConfig config = null; // TODO: Initialize to an appropriate value
        //    AuthenticationCenter target = new AuthenticationCenter(config);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

    }
}
