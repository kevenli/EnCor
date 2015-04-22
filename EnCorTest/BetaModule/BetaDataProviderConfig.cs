using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnCor.ObjectBuilder;
using EnCor.ModuleLoader;

namespace EnCorTest.BetaModule
{
    public class BetaDataProviderConfig : ServiceComponentConfig, IAssembler<object, ServiceComponentConfig>
    {
        public override IAssembler<object, ServiceComponentConfig> GetAssembler()
        {
            return this;
        }

        #region IAssembler<object,ServiceComponentConfig> Members

        public object Assemble(IBuilderContext context, ServiceComponentConfig objectConfiguration)
        {
            return Activator.CreateInstance(objectConfiguration.Type);
        }

        #endregion
    }
}
