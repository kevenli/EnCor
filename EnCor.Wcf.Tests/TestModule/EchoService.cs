using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnCor.Wcf.Tests.TestModule
{
    public class EchoService : IEchoService
    {
        #region IEchoService Members

        public string Echo(string message)
        {
            return message;
        }

        #endregion
    }
}
