using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.ModuleLoader;

namespace EnCor.Security
{
    public class SecurityModuleAssembler : IModuleAssembler
    {
        #region IAssembler<IEnCorModule,ModuleConfig> Members
        public IEnCorModule Assemble(ObjectBuilder.IBuilderContext context, IModuleConfig objectConfiguration)
        {
            return new SecurityModule(null, null);
        }

        #endregion
    }
}
