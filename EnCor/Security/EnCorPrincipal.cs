using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace EnCor.Security
{
    public class EnCorPrincipal : IPrincipal
    {
        private readonly EnCorIdentity _identity;

        private readonly List<string> _roles;
        /// <summary>
        /// Initializes a new instance of the <see cref="EnCorPrincipal"/> class.
        /// </summary>
        /// <param name="identity">The identity.</param>
        internal EnCorPrincipal(EnCorIdentity identity)
            : this(identity, new string[0])
        {
        }

        internal EnCorPrincipal(EnCorIdentity identity, IEnumerable<string> roles)
        {
            if (roles == null)
            {
                throw new ArgumentNullException("roles");
            }
            _identity = identity;
            _roles = new List<string>(roles);
        }

        #region IPrincipal

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.
        /// </returns>
        IIdentity IPrincipal.Identity
        {
            get
            {
                return _identity;
            }
        }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        public bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }

        #endregion

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.
        /// </returns>
        public EnCorIdentity Identity
        {
            get
            {
                return _identity;
            }
        }
    }
}
