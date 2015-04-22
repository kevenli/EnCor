using System;
using System.Security.Principal;
using System.Runtime.Serialization;

namespace EnCor.Security
{
    [Serializable]
    public sealed class EnCorIdentity : UserIdentity, IIdentity, ISerializable
    {
        public static readonly EnCorIdentity Anonymous = new EnCorIdentity("", "unknown", "", false);
        private readonly string _authenticationType;

        private readonly bool _isAuthenticated;

        public EnCorIdentity(string userId, string name, string authenticationType, bool isAuthenticated)
            : base(userId, name)
        {
            _authenticationType = authenticationType;
            _isAuthenticated = isAuthenticated;
        }

        private EnCorIdentity(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            _isAuthenticated = info.GetBoolean("IsAuthenticated");
            _authenticationType = info.GetString("AuthenticationType");
        }

        #region IIdentity 成员

        public string AuthenticationType
        {
            get
            {
                return _authenticationType;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }
        }

        #endregion


        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("AuthenticationType", _authenticationType);
            info.AddValue("IsAuthenticated", _isAuthenticated);
        }

        #endregion
    }
}
