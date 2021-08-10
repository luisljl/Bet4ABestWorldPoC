using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService)
        {
            var token = tokenService.GetCurrentUserToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var userId = tokenService.GetCurrentUserId();
                var invalidToken = await tokenService.GetInvalidTokenAsyncByUserIdAsync(userId);
                if (invalidToken != null)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                    var result = JsonSerializer.Serialize(new { message = "Unauthorized" });
                    await context.Response.WriteAsync(result);
                    return;
                }
                else
                {
                    context.Items["userId"] = userId;
                }
            }
            await _next(context);
        }
    }
}
