using Logistics.Data.UnitofWork;
using Logistics.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.BusinessCore.Services
{
    public class TokenGeneratorService
    {
        private IUnitOfWorkNoSql _unitOfWorkNoSql;
        private AutenticationConfigurationDTO _configuration;

        public TokenGeneratorService(IUnitOfWorkNoSql _unitOfWorkNoSql, AutenticationConfigurationDTO configuration)
        {
            _unitOfWorkNoSql = _unitOfWorkNoSql;
            _configuration = configuration;
        }

        public string GenerateToken(ClaimsUserDTO claimsDTO)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime fechaInicio = DateTime.UtcNow;
            DateTime expiracion = DateTime.UtcNow.AddMinutes(_configuration.ExpirationMinutes);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("userId", claimsDTO.UserId),
                new Claim("userName", claimsDTO.UserName),
            };

          
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration.Issuer,
                _configuration.Audience,
                claims,
                fechaInicio,
                expiracion,
                credentials
            );

            string tokenuser = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenuser;
        }
    }
}
