using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnCor.Tests.ModuleAlpha
{
    public class AlphaService : MarshalByRefObject, IAlphaService
    {
        public string Echo(string message)
        {
            return message;
        }
    }
}
