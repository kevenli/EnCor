using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EnCor.Wcf.Tests.TestModule;
using NUnit.Framework;

namespace EnCor.Wcf.Tests
{
    /// <summary>
    /// Summary description for RuntimeStartTest
    /// </summary>
    [TestFixture]
    public class RuntimeStartTest
    {
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [TestFixtureSetUp()]
        public static void MyClassInitialize()
        {
            Runtime.Startup();
        }

        [TestFixtureTearDown()]
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

        [Test]
        public void StartTest()
        {

        }

        [Test]
        public void ClientCall_Http_Test()
        {
            string message = "Hello";
            string endpointAddress = "http://localhost:9101/RouterService";
            BindingType bindingType = BindingType.WsHttp;
            ISampleService client = ClientFactory.CreateClient<ISampleService>(endpointAddress, bindingType);
            Assert.AreEqual(message, client.Echo("Hello"));

        }

        [Test]
        public void ClientCall_Nettcp_Test()
        {
            string message = "Hello";
            string endpointAddress = "net.tcp://localhost:9103/RouterService";
            ISampleService client = ClientFactory.CreateClient<ISampleService>(endpointAddress);
            Assert.AreEqual(message, client.Echo(message));

        }


        [Test]
        public void ClientCall_Https_Test()
        {
            string message = "Hello";
            string endpointAddress = "https://localhost:8011/RouterService";
            ISampleService client = ClientFactory.CreateClient<ISampleService>(endpointAddress);
            Assert.AreEqual(message, client.Echo(message));
        }


        //[Test]
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

        [Test]
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
