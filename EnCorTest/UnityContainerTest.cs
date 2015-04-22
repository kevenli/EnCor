using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor.ServiceLocator;
using EnCorTest.BetaModule;
using EnCorTest.AlphaModule;

namespace EnCorTest
{
    /// <summary>
    /// Summary description for UnityContainerTest
    /// </summary>
    [TestClass]
    public class UnityContainerTest
    {
        public UnityContainerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private IServiceContainer Target = new UnityServiceContainer();

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
        public void RegisterTest()
        {
            IBetaService betaservice = new BetaService(null, null);

            Target.RegisterService<IBetaService>(betaservice);

            Assert.AreEqual(Target.GetService<IBetaService>(), betaservice);
        }

        //[TestMethod]
        //public void PropertyInjectionTest()
        //{
        //    AlphaService alphaservice = new AlphaService();

        //    Target.RegisterService<AlphaService>(alphaservice);

        //    Target.RegisterPropertyInjection(typeof(BetaService), "AlphaService");

        //    BetaService betaservice = new BetaService(null, null);

        //    Target.BuildUp(typeof(BetaService), betaservice);

        //    Assert.IsNotNull(betaservice.AlphaService);

        //    Assert.AreEqual(betaservice.AlphaService, alphaservice);
        //}
    }
}
