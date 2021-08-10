using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        private readonly IBlackListTokenRepository _blackListTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TokenService(IOptions<AppSettings> appSettings, IBlackListTokenRepository blackListTokenRepository, IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings.Value;
            _blackListTokenRepository = blackListTokenRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(User user)
        {
            if (user == null)
            {
                throw new InvalidUserDataException();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task InvalidateTokenAsync(int userId)
        {
            var token = GetCurrentUserToken();

            await _blackListTokenRepository.CreateAsync(new BlackListToken()
            {
                CreatedOn = DateTime.Now,
                InvalidToken = token,
                UserId = userId,
            });
        }

        public async Task DeleteInvalidTokenAsync(int userId)
        {
            var tokenEntity = await _blackListTokenRepository.FirstOrDefaultAsync(w => w.UserId == userId);
            if (tokenEntity != null)
            {
                await _blackListTokenRepository.DeleteAsync(tokenEntity);
            }
        }

        public async Task<BlackListToken> GetInvalidTokenAsyncByUserIdAsync(int userId)
        {
            return await _blackListTokenRepository.FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public string GetCurrentUserToken()
        {
            return _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        public int GetCurrentUserId()
        {
            var idClaim = GetCurrentUserClaims().Where(x => x.Type == "nameid").FirstOrDefault();
            return int.Parse(idClaim.Value);
        }

        public string GetCurrentUserEmail()
        {
            var emailClaim = GetCurrentUserClaims().Where(x => x.Type == "email").FirstOrDefault();
            return emailClaim.Value;
        }

        public string GetCurrentUserUsername()
        {
            var usernameClaim = GetCurrentUserClaims().Where(x => x.Type == "name").FirstOrDefault();
            return usernameClaim.Value;
        }

        private IEnumerable<Claim> GetCurrentUserClaims()
        {
            var tokenEncoded = GetCurrentUserToken();
            if (string.IsNullOrEmpty(tokenEncoded))
            {
                throw new InvalidTokenException();
            }
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenEncoded);
            return token.Claims;
        }
    }
}
