using KnowledgeSharingApi.Infrastructures.Interfaces.Encrypts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Infrastructures.Encrypts
{
    public class KSEncript : IEncrypt
    {
        protected readonly IConfiguration _configuration;
        protected readonly string? _jwtSecret;
        protected readonly string? _jwtTokenValidInMinutes;
        protected readonly string? _jwtIssuer;
        protected readonly string? _jwtAudience;
        protected readonly string? _passwordHashSecretKey;
        protected readonly TokenValidationParameters tokenValidationParameters;
        readonly JwtSecurityTokenHandler tokenHandler = new();
        readonly SymmetricSecurityKey authSigningKey;
        readonly SigningCredentials signingCredentials;

        public KSEncript(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSecret = configuration["JWT:Secret"];
            _jwtTokenValidInMinutes = configuration["JWT:TokenValidityInMinutes"];
            _jwtIssuer = configuration["JWT:ValidIssuer"];
            _jwtAudience = configuration["JWT:ValidAudience"];
            _passwordHashSecretKey = configuration["JWT:PasswordHashSecretKey"];

            tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret ?? String.Empty)),
                ValidateLifetime = false
            };
            authSigningKey = new(Encoding.UTF8.GetBytes(_jwtSecret ?? String.Empty));
            signingCredentials = new(authSigningKey, SecurityAlgorithms.HmacSha256);
        }



        public virtual IEnumerable<Claim>? JwtDecrypt(string token)
        {
            ClaimsPrincipal? claimsPrincipal = JwtDecryptToClaimsPrincipal(token);
            return claimsPrincipal?.Claims;
        }

        public virtual ClaimsPrincipal? JwtDecryptToClaimsPrincipal(string token)
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                    token,
                    tokenValidationParameters,
                    out SecurityToken securityToken
                );

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }

        public virtual string? JwtEncrypt(IEnumerable<Claim> claims)
        {
            if (String.IsNullOrEmpty(_jwtSecret))
            {
                return null;
            }

            if (int.TryParse(_jwtTokenValidInMinutes, out int tokenValidityInMinutes))
            {
                JwtSecurityToken token = new(
                    issuer: _jwtIssuer,
                    audience: _jwtAudience,
                    expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                    claims: claims,
                    signingCredentials: signingCredentials
                );
                return tokenHandler.WriteToken(token);
            }

            return null;
        }

        public virtual string Sha256HashPassword(string username, string password)
        {
            string combineUser = $"{username} - {password} - {_passwordHashSecretKey}";
            
            byte[] combinedBytes = Encoding.UTF8.GetBytes(combineUser);
            byte[] hashedBytes = SHA256.HashData(combinedBytes);

            StringBuilder builder = new();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
