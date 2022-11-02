using DevFreela.Core.Enums;

namespace DevFreela.Core.Services
{
    public interface IAuthService
    {
        string ComputeSha256Hash(string password);
        string GenerateJwtToken(string email, RoleEnum role);
    }
}
