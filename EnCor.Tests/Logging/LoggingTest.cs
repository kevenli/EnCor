using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EnCor.Logging;

namespace EnCor.Tests.Logging
{
    [TestFixture]
    public class LoggingTest
    {
        [Test]
        public void LoggingServiceTest()
        {
            ILogging target = Runtime.Logging;
            Assert.IsNotNull(target);
        }
    }
}
