using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnCor.Wcf.Testing.TestModule
{
    public class EchoService : Service, IEchoService
    {
        #region IEchoService Members

        public string Echo(string message)
        {
            return message;
        }

        #endregion
    }
}
