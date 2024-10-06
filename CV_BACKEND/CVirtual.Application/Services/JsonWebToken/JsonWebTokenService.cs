using CVirtual.Application.Configurations;
using CVirtual.Application.IServices.JsonWebToken;
using CVirtual.Dto.CuentaUsuario;
using CVirtual.Dto.JsonWebToken;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services.JsonWebToken
{
    public class JsonWebTokenService : IJsonWebTokenService
    {
        private readonly TokenConfigurations _TokenConfigurations;

        public JsonWebTokenService(IOptions<TokenConfigurations> tokenConfigurations)
        {
            _TokenConfigurations = tokenConfigurations.Value;
        }

        public async Task<JwtTokenResponse> CrearToken(JwtInformacionResponse _JwtInformacionResponse)
        {
            return BuildAccessToken(_JwtInformacionResponse);
        }

        private JwtTokenResponse BuildAccessToken(JwtInformacionResponse _JwtInformacionResponse)
        {

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _TokenConfigurations.Issuer,
                Audience = _TokenConfigurations.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_TokenConfigurations.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_TokenConfigurations.AccessTokenExpiration),
                //SigningCredentials = _signConfigurations.SignCredentials,
                //EncryptingCredentials = _encrypConfigurations.EncryptingCredentials,
                Subject = new ClaimsIdentity(GetClaims(_JwtInformacionResponse))
            };

            JwtTokenResponse _JwtTokenResponse = new JwtTokenResponse();

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
            var encryptedJwt = tokenHandler.WriteToken(securityToken);

            _JwtTokenResponse.Token = encryptedJwt;
            _JwtTokenResponse.Tipo = "Bearer";
            _JwtTokenResponse.ExpiraEn = descriptor.Expires;

            return _JwtTokenResponse;
        }

        private IEnumerable<Claim> GetClaims(JwtInformacionResponse _JwtInformacionResponse)
        {
            var claims = new List<Claim>
            {
                new Claim("TokenInfo", JsonConvert.SerializeObject(_JwtInformacionResponse)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, _JwtInformacionResponse.Id.ToString())
            };
            return claims;
        }
    }
}
