using KnowledgeSharingApi.Domains.Models.Dtos;
using KnowledgeSharingApi.Domains.Models.Entities;
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
    public class KSEncrypt : IEncrypt
    {
        protected readonly IConfiguration _configuration;
        protected readonly string? _jwtSecret;
        protected readonly string? _jwtTokenValidInMinutes;
        protected readonly string? _jwtIssuer;
        protected readonly string? _jwtAudience;
        protected readonly string? _passwordHashSecretKey;

        readonly JwtSecurityTokenHandler tokenHandler = new();
        readonly SymmetricSecurityKey authSigningKey;
        readonly SigningCredentials signingCredentials;

        public KSEncrypt(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSecret = configuration["JWT:Secret"];
            _jwtTokenValidInMinutes = configuration["JWT:TokenValidityInMinutes"];
            _jwtIssuer = configuration["JWT:ValidIssuer"];
            _jwtAudience = configuration["JWT:ValidAudience"];
            _passwordHashSecretKey = configuration["JWT:PasswordHashSecretKey"];

            authSigningKey = new(Encoding.UTF8.GetBytes(_jwtSecret ?? String.Empty));
            signingCredentials = new(authSigningKey, SecurityAlgorithms.HmacSha256);
        }


        #region functional methods:
        public static string? GetClaimValue(ClaimsPrincipal principal, string key)
        {
            Claim? claim = principal.FindFirst(key);
            return claim?.Value;
        }
        #endregion

        #region En-De Between token and JwtTokenDto
        public virtual JwtTokenDto? JwtDecrypt(string token, bool isValidateLifeTime)
        {
            ClaimsPrincipal? principal = JwtDecryptToClaimsPrincipal(token, isValidateLifeTime);
            if (principal == null) return null;

            string? username = principal.Identity?.Name;
            string? userId = GetClaimValue(principal, ClaimTypes.NameIdentifier);
            string? role = GetClaimValue(principal, ClaimTypes.Role);
            string? sessionId = GetClaimValue(principal, ClaimTypes.Sid);

            if (username != null && userId != null && role != null && sessionId != null)
            {
                return new JwtTokenDto()
                {
                    Username = username,
                    Role = role,
                    UserId = Guid.Parse(userId),
                    SessionId = Guid.Parse(sessionId)
                };
            }

            return null;
        }

        public virtual string? JwtEncrypt(JwtTokenDto tokenDto)
        {
            List<Claim> authClaims = [
                new (ClaimTypes.Name, tokenDto.Username),
                new (ClaimTypes.Role, tokenDto.Role),
                new (ClaimTypes.NameIdentifier, tokenDto.UserId.ToString()),
                new (ClaimTypes.Sid, tokenDto.SessionId.ToString()),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];

            return JwtEncrypt(authClaims);
        }
        #endregion



        #region En-De between string and ClaimPrincipal
        public string? JwtEncrypt(List<Claim> claims)
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

        public List<Claim>? JwtDecryptToListClaims(string token, bool isValidateLifeTime)
        {
            ClaimsPrincipal? principal = JwtDecryptToClaimsPrincipal(token, isValidateLifeTime);
            return principal?.Claims.ToList();
        }

        public ClaimsPrincipal? JwtDecryptToClaimsPrincipal(string token, bool isValidateLifetime)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigningKey,
                ValidateLifetime = isValidateLifetime
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                    token, tokenValidationParameters, out SecurityToken securityToken
                );

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        } 
        #endregion

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
