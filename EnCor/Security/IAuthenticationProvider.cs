namespace EnCor.Security
{
    /// <summary>
    /// AuthenticationProvider for implementing authenticate in EnCor environment.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Authenticate a credential
        /// </summary>
        /// <param name="credential">credential contains infomation for authenticating</param>
        /// <returns>A full ClaimSet contains all user info</returns>
        ClaimSet Authenticate(Credential credential);

        /// <summary>
        /// Doing something after authenticating, such as generate a token.
        /// </summary>
        /// <param name="claimSet">The result of authenticating</param>
        /// <returns>A full ClaimSet contains all user info</returns>
        ClaimSet PostAuthenticate(ClaimSet claimSet);
    }
}
