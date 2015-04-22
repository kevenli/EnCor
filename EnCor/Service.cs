using System;
using EnCor.ModuleLoader;
using EnCor.ObjectBuilder;

namespace EnCor
{
    [AssembleConfig(typeof(ServiceConfig))]
    public abstract class Service : MarshalByRefObject
    {
        protected internal virtual void Start()
        {
        }

        protected internal virtual void Stop()
        {
        }
    }
}
