using CSC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Services
{
    public class ApplicationSignInManager : SignInManager<User>
    {
        public ApplicationSignInManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<User>> logger, IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            try
            {
                var user = UserManager.FindByNameAsync(userName).Result;
                if (user.Demissao == null)
                {
                    return base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
                }
                else
                {
                    return Task.FromResult(SignInResult.NotAllowed);
                }
            }
            catch (NullReferenceException)
            {
                return Task.FromResult(SignInResult.Failed);
            }
        }
    }
}
