using System.Collections.Generic;
using System.Linq;
using HolidayApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace HolidayApp.API.Data
{
    public class Seed
    {
        
        public static async System.Threading.Tasks.Task SeedUsersAsync(DataContext dataContext,UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>{
                    new Role {Name = "Member"},
                    new Role {Name = "Admin"}
                };

                foreach(var role in roles){
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                { 
                    await userManager.CreateAsync(user,"password");
                    await userManager.AddToRoleAsync(user,"Member");
                }

                //create admin user
                var adminUser = new User{
                    UserName = "Admin"
                };

                var result = userManager.CreateAsync(adminUser,"password").Result;
                if(result.Succeeded){
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    await userManager.AddToRolesAsync(admin,new []{"Admin"});
                }
                await dataContext.SaveChangesAsync();
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}