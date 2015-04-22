using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EnCor.Wcf.Testing.TestModule
{
    [ServiceContract(Namespace = "http://encor.codeplex.com/test/sampeservice/")]
    public interface IEchoService
    {
        [OperationContract]
        string Echo(string message);

    }
}
