namespace EnCor.Security.AuthenticationProviders
{
    public abstract class AuthenticationProvider : IAuthenticationProvider
    {

        /// <summary>
        /// Authenticate User Credential
        /// </summary>
        /// <param name="credential">Credential</param>
        /// <returns>Return authenticated identity, null for can't authenticate</returns>
        public abstract ClaimSet Authenticate(Credential credential);

        public virtual ClaimSet PostAuthenticate(ClaimSet claimSet)
        {
            return claimSet;
        }
    }
}
