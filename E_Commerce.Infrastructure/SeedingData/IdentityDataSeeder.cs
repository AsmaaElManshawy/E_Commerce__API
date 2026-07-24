using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.SeedingData
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext ,
            UserManager<ApplicationUser> userManager , 
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingmigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pendingmigrations.Any())
                    await _dbContext.Database.MigrateAsync(ct);
                // Succeeded
                if (!await _roleManager.Roles.AnyAsync())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }


                // Error
                // dose not seed user always : Can not Assign Role To User
                if (!await _userManager.Users.AnyAsync())
                {
                    var admin = new ApplicationUser
                    {
                        DisplayName = "AsmaaSayed",
                        Email = "asmaa@gmail.com",
                        UserName = "Asmaa Sayed",
                        PhoneNumber = "01125478963"
                    };
                    var createResult = await _userManager.CreateAsync(admin, "p@ssw0rd");

                    if (createResult.Succeeded)
                        await _userManager.AddToRoleAsync(admin, "SuperAdmin");
                    else
                        Console.WriteLine($"Can not Assign Role To User");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
