using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public class ModuleException : EnCorException
    {
        public ModuleException(string message)
            : base(message)
        {
        }

        public ModuleException(string messsage, Exception innerException)
            : base(messsage, innerException)
        {
        }
    }
}
