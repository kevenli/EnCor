using System;
using System.Collections.Generic;
using EnCor.ModuleLoader;
using EnCor.Logging;

namespace EnCor
{
    /// <summary>
    /// Base class of modules. 
    /// It is recommanded to derive from this class to build custom module.
    /// </summary>
    public class Module : IEnCorModule
    {
        private IServiceContainer _ServiceContainer;
        private string _ModuleName;

        protected void RegisterBizService(object serviceInstance, string serviceName, Type contract)
        {
            _ServiceContainer.RegisterService(this._ModuleName, serviceName, serviceInstance, contract);
        }

        public void SetServiceContainer(IServiceContainer serviceContainer)
        {
            _ServiceContainer = serviceContainer;
        }

        public void SetName(string moduleName)
        {
            _ModuleName = moduleName;
        }

        #region Virtual Members

        public virtual void Init()
        {
        }

        public virtual void Start()
        {
            
        }

        public virtual void Stop()
        {
            
        }


        #endregion
    }
}
