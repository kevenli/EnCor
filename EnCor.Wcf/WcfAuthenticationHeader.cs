using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace EnCor.Wcf
{
    [DataContract]
    public class WcfAuthenticationHeader
    {
        private string _Username;
        private string _Token;

        [DataMember]
        public string Token
        {
            get
            {
                return _Token;
            }
            set
            {
                _Token = value;
            }
        }

        [DataMember]
        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                _Username = value;
            }
        }
    }
}
