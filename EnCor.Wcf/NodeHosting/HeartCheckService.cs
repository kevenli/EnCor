using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Hosting;
using EnCor.ObjectBuilder;
using System.Configuration;
using System.ServiceModel.Configuration;
using EnCor.Wcf.Hosting;

namespace EnCor.Wcf.NodeHosting
{
    public class HeartCheckService : IHeartCheckService
    {
        #region IHeartCheckService Members

        public bool IsAlive()
        {
            return true;
        }

        #endregion
    }
}
