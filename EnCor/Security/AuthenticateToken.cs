using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Runtime.Serialization;

namespace EnCor.Security
{
    [Serializable]
    public class AuthenticateToken : ISerializable
    {
        private EnCorIdentity _Identity;

        private string _Token;

        private AuthenticateToken(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");

            _Identity = (EnCorIdentity)info.GetValue("Identity", typeof(EnCorIdentity));
            _Token = info.GetString("Token");
        }

        public AuthenticateToken(EnCorIdentity identity, string token)
        {
            _Identity = identity;
            _Token = token;
        }

        public EnCorIdentity Identity
        {
            get
            {
                return _Identity;
            }
        }

        public string Token
        {
            get
            {
                return _Token;
            }
        }



        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");

            info.AddValue("Identity", _Identity);
            info.AddValue("Token", _Token);
        }

        #endregion
    }
}
