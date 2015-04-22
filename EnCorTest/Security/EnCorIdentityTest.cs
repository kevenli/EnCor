using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EnCorTest.Security
{
    /// <summary>
    /// Summary description for EnCorIdentityTest
    /// </summary>
    [TestClass]
    public class EnCorIdentityTest
    {
        public EnCorIdentityTest()
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
        public void SerializeTest()
        {
            string userid = "0010203";
            string name = "keven";
            string authenticationType = "password";
            bool isAuthenticated = true;
            EnCorIdentity identity = new EnCorIdentity(userid, name, authenticationType, isAuthenticated);

            
            BinaryFormatter bf = new BinaryFormatter();
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EnCorIdentity));
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, identity);

            stream.Flush();
            stream.Position = 0;
            

            EnCorIdentity deserialized = (EnCorIdentity)bf.Deserialize(stream);

            Assert.AreEqual(identity.UserId, deserialized.UserId);
            Assert.AreEqual(identity.Name, deserialized.Name);
            Assert.AreEqual(identity.IsAuthenticated, deserialized.IsAuthenticated);
            Assert.AreEqual(identity.AuthenticationType, deserialized.AuthenticationType);
        }
    }
}
