using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public interface IServiceConfig
    {
        string Name { get; }

        Type ServiceType { get; }

        Type[] Contracts { get; }
    }
}
