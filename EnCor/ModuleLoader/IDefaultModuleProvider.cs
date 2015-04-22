using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;

namespace EnCor.ModuleLoader
{
    public interface IDefaultModuleProvider
    {
        IModuleConfig GetDefaultModuleConfig(Type moduleType);
    }
}
