namespace DauStore.Api.Authentication
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string phone, string password);
    }
}
