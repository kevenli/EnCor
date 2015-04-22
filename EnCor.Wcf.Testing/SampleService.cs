using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EnCor.Wcf.Testing
{
    [ServiceContract(Namespace="http://encor.codeplex.com/test/sampeservice/")]
    public interface ISampleService
    {
        [OperationContract]
        string Echo(string message);

    }
    
    public class SampleService : ISampleService
    {
        #region ISampleService Members

        public string Echo(string message)
        {
            if (message == "Exception")
            {
                throw new Exception("Exception");
            }
            return message;
        }

        #endregion
    }
}
