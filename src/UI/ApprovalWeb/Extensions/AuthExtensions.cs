using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using UserService.Domain.Entities;
using ApprovalWeb.Interfaces;

namespace ApprovalWeb.Extensions;

public static class AuthExtensions
{
    // IServiceCollection 확장 메서드
     public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
     {
          services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
          {
               options.Events.OnTokenValidated = async context =>
               {
                    var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService<User>>();

                    var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;

                    var email = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value
                              ?? claimsIdentity?.FindFirst("preferred_username")?.Value;

                    if (string.IsNullOrEmpty(email)) return;

                    var userinfo = await userService.UserLoginAndInformationAsync(email);

                    if (userinfo != null && userinfo.Email.Length > 0)
                    {
                         var claims = new List<Claim>
                         {
                              new Claim(ClaimTypes.NameIdentifier, userinfo?.EmployeeNumber ?? string.Empty),
                              new Claim(ClaimTypes.Name, userinfo?.Name ?? string.Empty),
                              new Claim("UserName", userinfo?.Name ?? string.Empty),
                              new Claim(ClaimTypes.Role, userinfo?.Role ?? string.Empty),
                              new Claim("Position", userinfo?.Position ?? string.Empty),
                              new Claim("Department", userinfo?.Department ?? string.Empty),
                         };

                         var existingNameIdentifier = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                         if (existingNameIdentifier != null)
                         {
                              claimsIdentity?.RemoveClaim(existingNameIdentifier);
                         }

                         claimsIdentity?.AddClaims(claims);
                    }
                    else
                    {
                         context.Fail("User not found or invalid email.");
                    }
               };
          });

          return services;
     }
}