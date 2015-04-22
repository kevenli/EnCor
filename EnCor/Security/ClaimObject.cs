using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.Security
{
    public class ClaimObject
    {
        private string _ClaimType;
        private object _Resource;
        private string _Right;

        public ClaimObject(string claimType, object resource, string right)
        {
            _ClaimType = claimType;
            _Resource = resource;
            _Right = right;
        }

        public string ClaimType
        {
            get
            {
                return _ClaimType;
            }
        }

        public object Resource
        {
            get
            {
                return _Resource;
            }
        }

        public string Right
        {
            get
            {
                return _Right;
            }
        }

        public static ClaimObject CreateRoleClaim(string role)
        {
            return new ClaimObject(ClaimTypes.Role, role, Rights.PossessProperty);
        }
    }
}
