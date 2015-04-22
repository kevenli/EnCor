using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.EnCor;
using Mars.EnCorTest.AlphaModule;

namespace Mars.EnCorTest.BetaModule
{
    public class BetaService : BizService, IBetaService, IBetaService2
    {
        AlphaService _AlphaService;
        IBetaDataProvider _DataProvider;


        [Dependency]
        public AlphaService AlphaService
        {
            set
            {
                _AlphaService = value;
            }
            get
            {
                return _AlphaService;
            }
        }

        [Dependency]
        public IBetaDataProvider DataProvider
        {
            set
            {
                _DataProvider = value;
            }
            get
            {
                return _DataProvider;
            }
        }

        public bool Ready()
        {
            if (_AlphaService != null && _DataProvider != null)
            {
                return true;
            }
            return false;
        }

        public BetaService()
        { 
            
        }
    }
}
