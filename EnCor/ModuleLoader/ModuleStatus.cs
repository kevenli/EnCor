using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public enum ModuleStatus
    {
        New = 0,
        Initialized = 1,
        Start = 8,
        Stop = 16
    }
}
