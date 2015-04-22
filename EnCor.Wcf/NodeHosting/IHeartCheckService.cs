using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Hosting;
using EnCor.ObjectBuilder;
using System.Configuration;
using System.ServiceModel.Configuration;
using EnCor.Wcf.Hosting;
using System.ServiceModel;

namespace EnCor.Wcf.NodeHosting
{
    [ServiceContract(Namespace = "EnCor.Wcf.NodeHosting.IHeartCheckService")]
    public interface IHeartCheckService
    {
        [OperationContract]
        bool IsAlive();
    }
}
