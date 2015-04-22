using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public class ServiceBuildException : ModuleException
    {
        private const string ConDefaultMessage = "Error when build BizService";
        public ServiceBuildException()
            : base(ConDefaultMessage)
        {
        }

        public ServiceBuildException(string message)
            : base(message)
        {
        }

        public ServiceBuildException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
