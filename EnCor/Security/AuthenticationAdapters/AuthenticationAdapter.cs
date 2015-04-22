namespace EnCor.Security.AuthenticationAdapters
{
    public abstract class AuthenticationAdapter : IAuthenticationAdapter
    {
        public abstract Credential GetCredential();

        public abstract void SetCredential(AuthenticateToken token);

        public abstract void ClearCredential();

        public virtual bool IsReadOnly { get; set; }
    }
}
