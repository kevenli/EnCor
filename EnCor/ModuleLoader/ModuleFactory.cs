﻿using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.ObjectBuilder;

namespace EnCor.ModuleLoader
{
    public class ModuleFactory : AssembleFactory<IEnCorModule, IModuleConfig>
    {
    }
}
