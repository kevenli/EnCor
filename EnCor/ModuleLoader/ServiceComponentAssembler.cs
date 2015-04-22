using System;
using System.Collections.Generic;
using System.Text;
using EnCor.ObjectBuilder;

namespace EnCor.ModuleLoader
{
    public class ServiceComponentAssembler : IAssembler<object, ServiceComponentConfig>
    {
        #region IAssembler<object,ServiceComponentConfig> Members
        public object Assemble(
        IBuilderContext context, ServiceComponentConfig objectConfiguration)
        {
            return Activator.CreateInstance(objectConfiguration.Type);
        }

        #endregion
    }
}
