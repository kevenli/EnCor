using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public sealed class ModuleStatusException : ModuleException
    {
        public ModuleStatusException(string message)
            : base(message)
        {
        }
    }
}
