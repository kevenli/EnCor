using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor.Security;
using System.Web;
using System.IO;
using System.Web.Security;
using EnCor.Aspnet.Security;

namespace EnCorTest.Aspnet
{
    /// <summary>
    /// Summary description for AspnetFormsAuthenticationAdapterTest
    /// </summary>
    [TestClass]
    public class AspnetFormsAuthenticationAdapterTest
    {
        public AspnetFormsAuthenticationAdapterTest()
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
        public void NoHttpSessionTest()
        {
            AspnetTokenAdapter target = new AspnetTokenAdapter();

            Assert.IsNull(target.GetCredential());
            //
            // TODO: Add test logic	here
            //
        }

        [TestMethod]
        public void NoCookieTest()
        {
            AspnetTokenAdapter target = new AspnetTokenAdapter();

            TextWriter output = new StringWriter();
            HttpContext context = new HttpContext(new SimulatedHttpRequest("", "", "", "", output, "mock"));

            HttpContext.Current = context;

            Assert.IsNotNull(HttpContext.Current);
            Assert.IsNull(target.GetCredential());
        }

        [TestMethod]
        public void TicketTest()
        {
            string token = Guid.NewGuid().ToString();

            AspnetTokenAdapter target = new AspnetTokenAdapter();

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "test", DateTime.Now, DateTime.Now, true, token);

            TextWriter output = new StringWriter();
            SimulatedHttpRequest request = new SimulatedHttpRequest("", "", "", "", output, "mock");
            HttpContext.Current = new HttpContext(request);

            EnCor.Aspnet.Security.AspnetTokenAdapter.SetAuthenticationCookie("", token);

            AspnetTokenCredential tokenCredential = target.GetCredential() as AspnetTokenCredential;

            Assert.IsNotNull(tokenCredential);
            Assert.AreEqual(token, tokenCredential.Ticket.UserData);
        }
    }
}
