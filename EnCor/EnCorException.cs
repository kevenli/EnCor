using System;

namespace EnCor
{
    public class EnCorException : SystemException
    {
        public EnCorException(string message)
            : base(message)
        {
        }

        public EnCorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
