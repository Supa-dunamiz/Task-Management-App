using Microsoft.AspNetCore.Identity;
using System.Net;
using TaskApp.Data.Enum;
using TaskApp.Models;

namespace TaskApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.UserTasks.Any())
                {
                    context.UserTasks.AddRange(new List<UserTask>()
                    {
                        new UserTask()
                        {
                            Title = "Send Mails",
                            Description = "This is the description of the first task",
                            TaskPriority = TaskPriority.Urgent_And_Important,
                           // DateOfTask = "2023/10/03"
                        },
                        new UserTask()
                        {
                            Title = "Send Errands",
                            Description = "This is the description of the second task",
                            TaskPriority = TaskPriority.Urgent_And_Important,
                           // DateOfTask = "2023/10/03"
                        },
                        new UserTask()
                        {
                            Title = "Cook Meals",
                            Description = "This is the description of the third task",
                            TaskPriority = TaskPriority.Urgent_And_Important,
                           // DateOfTask = "2023/10/03"
                        },


                    });
                    context.SaveChanges();
                }
            }
        }
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "abakugodpower@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        FirstName = "Dunamis",
                        LastName = "Benjamin",
                        State = "Lagos",
                        Country = "Nigeria",
                        UserName = "Supa_Dunamis",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {

                        FirstName = "appUser",
                        LastName = "appUser",
                        State = "Lagos",
                        Country = "Nigeria",
                        UserName = "appUser",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.Admin);
                }
            }
        }
    }
    
}

