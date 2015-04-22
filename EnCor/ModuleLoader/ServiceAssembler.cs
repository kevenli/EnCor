using System;
using System.Collections.Generic;
using System.Text;
using EnCor.ObjectBuilder;

namespace EnCor.ModuleLoader
{
    public class ServiceAssembler : IAssembler<object, IServiceConfig>
    {
        #region IAssembler<IService,IServiceConfig> Members
        public object Assemble(IBuilderContext context, IServiceConfig objectConfiguration)
        {
            IServiceContainer serviceContainer = context.GetExtension<IServiceContainer>();
            return serviceContainer.BuildUp(objectConfiguration.ServiceType);
        }

        #endregion
    }
}
