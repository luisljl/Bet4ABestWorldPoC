using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
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

        public async Task InvalidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidTokenException();
            }
            await _blackListTokenRepository.AddAsync(new BlackListToken()
            {
                CreatedOn = DateTime.Now,
                InvalidToken = token
            });
        }

        public async Task<BlackListToken> GetInvalidTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidTokenException();
            }
            return await _blackListTokenRepository.GetAsync(token);
        }

        public string GetCurrentUserToken()
        {
            return _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }

        public int GetCurrentUserId()
        {
            var idClaim = GetCurrentUser().Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            return int.Parse(idClaim.Value);
        }

        public string GetCurrentUserEmail()
        {
            var emailClaim = GetCurrentUser().Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault();
            return emailClaim.Value;
        }

        public string GetCurrentUserUsername()
        {
            var usernameClaim = GetCurrentUser().Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault();
            return usernameClaim.Value;
        }

        private ClaimsIdentity GetCurrentUser()
        {
            if (string.IsNullOrEmpty(GetCurrentUserToken()))
            {
                throw new InvalidTokenException();
            }
            return _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        }
    }
}
