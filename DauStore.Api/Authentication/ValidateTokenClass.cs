using DauStore.Core.Entities;
using DauStore.Infrastructure.Repositories;

namespace DauStore.Api.Authentication
{
    public class ValidateTokenClass
    {
        public static bool ValidateToken(string token)
        {
            if (token.StartsWith("Bearer"))
            {
                token = token.Replace("Bearer", "bearer");
            }
            BaseRepository<TokenAccount> tokenAccountRepo = new BaseRepository<TokenAccount>();
            var tokenAccount = tokenAccountRepo.GetByProp("Token", token);
            if (tokenAccount != null)
            {
                return true;
            }
            return false;
        }
    }
}
