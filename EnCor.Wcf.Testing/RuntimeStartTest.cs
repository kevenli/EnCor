using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EnCor.Wcf.Testing.TestModule;

namespace EnCor.Wcf.Testing
{
    /// <summary>
    /// Summary description for RuntimeStartTest
    /// </summary>
    [TestClass]
    public class RuntimeStartTest
    {
        public RuntimeStartTest()
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Runtime.Startup();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            Runtime.Stop();
        }

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
        [DeploymentItem("EnCor.config")]
        public void StartTest()
        {

        }

        [TestMethod]
        [DeploymentItem("EnCor.config")]
        public void ClientCall_Http_Test()
        {
            string message = "Hello";
            string endpointAddress = "http://localhost:9101/RouterService";
            BindingType bindingType = BindingType.WsHttp;
            ISampleService client = ClientFactory.CreateClient<ISampleService>(endpointAddress, bindingType);
            Assert.AreEqual(message, client.Echo("Hello"));

        }

        [TestMethod]
        [DeploymentItem("EnCor.config")]
        public void ClientCall_Nettcp_Test()
        {
            string message = "Hello";
            string endpointAddress = "net.tcp://localhost:9103/RouterService";
            ISampleService client = ClientFactory.CreateClient<ISampleService>(endpointAddress);
            Assert.AreEqual(message, client.Echo(message));

        }


        [TestMethod]
        [DeploymentItem("EnCor.config")]
        public void ClientCall_Https_Test()
        {
            string message = "Hello";
            string endpointAddress = "https://localhost:8011/RouterService";
            ISampleService client = ClientFactory.CreateClient<ISampleService>(endpointAddress);
            Assert.AreEqual(message, client.Echo(message));
        }


        //[TestMethod]
        //public void ClientCall_Nettcp_Exception_Test()
        //{
        //    string message = "Exception";
        //    Binding binding = new NetTcpBinding(SecurityMode.None);
        //    EndpointAddress address = new EndpointAddress("net.tcp://localhost:9103/RouterService");
        //    using (ChannelFactory<ISampleService> factory = new ChannelFactory<ISampleService>(binding, address))
        //    {
        //        try
        //        {
        //            var client = factory.CreateChannel();
        //            client.Echo(message);
        //            Assert.Fail();
        //        }
        //        catch (Exception ex)
        //        {

        //            Console.WriteLine(ex);
        //            Assert.IsTrue(ex.Message == message);

        //        }
        //    }
        //}

        [TestMethod]
        [DeploymentItem("EnCor.config")]
        public void ClientCall_Nettcp_Test2()
        {
            string message = "Hello";
            string endpointAddress = "net.tcp://localhost:9103/RouterService";
            BindingType bindingType = BindingType.NetTcp;
            IEchoService client = ClientFactory.CreateClient<IEchoService>(endpointAddress);
            Assert.AreEqual(message, client.Echo(message));

        }
    }
}
