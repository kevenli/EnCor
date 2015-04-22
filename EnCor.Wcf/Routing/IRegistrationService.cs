using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace EnCor.Wcf.Routing
{
    [ServiceContract(Namespace = "EnCor.Wcf.Routing.IRegistrationService")]
    public interface IRegistrationService
    {
        [OperationContract]
        void Register(RegistrationInfo regInfo, int proportion);

        [OperationContract]
        void Unregister(RegistrationInfo regInfo);


        [OperationContract]
        int HeartCheck(string baseAddress);
    }
}
