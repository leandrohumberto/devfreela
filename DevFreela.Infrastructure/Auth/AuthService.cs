using DevFreela.Core.Enums;
using DevFreela.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DevFreela.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ComputeSha256Hash(string password)
        {
            using SHA256 sha256Hash = SHA256.Create();
            // ComputeHash - retorna byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Converte byte array para string
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2")); // x2 faz com que seja convertido em representação hexadecimal
            }
            return builder.ToString();
        }

        public string GenerateJwtToken(string email, RoleEnum role)
        {
            //
            // Recupera informações das configurações (arquivo appsettings.json na camada API)
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            //
            // Cria a chave simétrica e as credenciais de assinatura
            var encodedKey = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(encodedKey);
            var credentials = new SigningCredentials(
                key: securityKey,
                algorithm: SecurityAlgorithms.HmacSha256);

            // Define a lista de claims (informações exigidas) do usuário
            var claims = new List<Claim>
            {
                new Claim("userName", email),
                new Claim(ClaimTypes.Role, role.ToString()),
            };

            //
            // Instancia o token JWT
            var token = new JwtSecurityToken(issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            //
            // Serializa o token JWT em formato string
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}
