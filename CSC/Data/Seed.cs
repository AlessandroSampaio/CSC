using CSC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CSC.Data
{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            //adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin", "Analista", "Financeiro", "Supervisor" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //creating a super user who could maintain the web app
            var poweruser = new User
            {
                UserName = Configuration.GetSection("AppSettings")["UserName"],
                Email = Configuration.GetSection("AppSettings")["UserEmail"]
            };

            string userPassword = Configuration.GetSection("AppSettings")["UserPassword"];
            var user = await UserManager.FindByEmailAsync(Configuration.GetSection("AppSettings")["UserEmail"]);

            if (user == null)
            {
                try
                {
                    var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                    if (createPowerUser.Succeeded)
                    {
                        //here we tie the new user to the "Admin" role 
                        await UserManager.AddToRoleAsync(poweruser, "Admin");
                        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                        await UserManager.ConfirmEmailAsync(poweruser, code);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
