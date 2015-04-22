namespace EnCor.Security.Credentials
{
    public abstract class TokenCredential : Credential
    {
        public abstract string Token { get; }
    }
}
