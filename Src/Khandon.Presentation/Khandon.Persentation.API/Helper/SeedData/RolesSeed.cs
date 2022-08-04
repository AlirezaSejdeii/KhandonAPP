using Khandon.Infrastructure.Users.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Khandon.Persentation.API.Helper.SeedData
{
    public class RolesSeed
    {
        public RolesSeed(RoleManager<ApplicationRole> roleManager)
        {
            roleManager.CreateAsync(new ApplicationRole() { Name = "Admin" });
        }
    }
}
