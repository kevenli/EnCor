using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnCor.Tests.ModuleAlpha;

namespace EnCor.Tests.ModuleBeta
{
    public class BetaService : MarshalByRefObject
    {
        private IAlphaService _AlplaService;

        public BetaService()
        { 
            
        }

        public BetaService(IAlphaService alphaService)
        {
            _AlplaService = alphaService;
        }

        public string Echo(string message)
        {
            return _AlplaService.Echo(message);
        }


    }
}
