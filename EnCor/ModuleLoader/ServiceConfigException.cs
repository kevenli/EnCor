using System;
using System.Configuration;

namespace EnCor.ModuleLoader
{
    public class ServiceConfigException : EnCorException
    {
        private const string ConDefaultMessage = "Service config wrong.";
        public ServiceConfigException()
            : base(ConDefaultMessage)
        {
        }

        public ServiceConfigException(string message)
            : base(message)
        {
        }

        public ServiceConfigException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
