using System;

namespace EnCor.Security
{
    public sealed class SecurityContext
    {
        [ThreadStatic]
        private static SecurityContext _current;
        public static SecurityContext Current
        {
            get
            {
                return _current ?? (_current = new SecurityContext(new EnCorPrincipal(EnCorIdentity.Anonymous)));
            }

            internal set
            {
                _current = value;
            }
        }

        private ClaimSet _ClaimSet;

        internal SecurityContext(EnCorPrincipal principal)
        {
            _Principal = principal;
        }

        internal SecurityContext(EnCorPrincipal principal, ClaimSet claimSet)
        {
            _Principal = principal;
            _ClaimSet = claimSet;
        }

        internal SecurityContext(ClaimSet claimSet)
        {
            _ClaimSet = claimSet;
        }

        private EnCorPrincipal _Principal;

        public EnCorPrincipal Principal
        {
            get
            {
                return _Principal;
            }
        }

        private EnCorIdentity _Identity;

        public EnCorIdentity Identity
        {
            get
            {
                return _Principal.Identity;
            }
        }

        public ClaimSet ClaimSet
        {
            get
            {
                return _ClaimSet;
            }
        }

        public bool IsInRole(string role)
        {
            return _Principal.IsInRole(role);
        }

        public bool IsAnonymous
        {
            get
            {
                return this._Identity == EnCorIdentity.Anonymous;
            }
        }

        public bool HasPermission(string permission)
        {
            throw new NotImplementedException();
        }
    }
}
