using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnCor;
using EnCorTest.AlphaModule;

namespace EnCorTest.BetaModule
{
    public class BetaService : BizService, IBetaService, IBetaService2
    {
        AlphaService _AlphaService;
        IBetaDataProvider _DataProvider;


        public AlphaService AlphaService
        {
            get
            {
                return _AlphaService;
            }
        }

        public IBetaDataProvider DataProvider
        {
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

        public BetaService(IBetaDataProvider betaDataProvider, AlphaService alphaService)
        {
            _DataProvider = betaDataProvider;
            _AlphaService = alphaService;
        }

        public string Echo(string message)
        {
            return message;
        }
    }
}
