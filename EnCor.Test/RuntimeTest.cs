using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnCor.Test
{
    [TestClass]
    public class RuntimeTest
    {
        [TestMethod]
        [DeploymentItem("encor.config")]
        public void StartupTest()
        {
            Runtime.Startup();
            Runtime.Logging.Info("EnCor runtime started");
            Runtime.Stop();
        }
    }
}
