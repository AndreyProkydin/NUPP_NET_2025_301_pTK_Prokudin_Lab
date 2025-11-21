using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restouran.Infrastructure;
using Restouran.Infrastructure.Models;
using System.Threading.Tasks;



namespace Restouran.REST
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<RestouranContext>(options =>
                options.UseSqlite(connectionString));

            


            builder.Services.AddScoped(typeof(IRepository<>), typeof(RestouranRepository<>));
            builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(RestouranCrudService<>));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthorization();

            builder.Services.AddIdentityApiEndpoints<UsersModels>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<RestouranContext>();

            




            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<RestouranContext>();
                    var userManager = services.GetRequiredService<UserManager<UsersModels>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await context.Database.MigrateAsync();

                    string[] roleNames = { "Administrator", "Moderator", "CommonUser" };
                    foreach (var roleName in roleNames)
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            await roleManager.CreateAsync(new IdentityRole(roleName));
                        }
                    }

                    string adminEmail = "admin@restouran.com";
                    string adminPassword = "Admin123!";

                    var adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        adminUser = new UsersModels
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            Name = "Admin", 
                            BirthDate = DateTime.UtcNow.AddYears(-30), 
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(adminUser, adminPassword);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(adminUser, "Administrator");
                        }
                    }
                    else
                    {
                        if (!await userManager.IsInRoleAsync(adminUser, "Administrator"))
                        {
                            await userManager.AddToRoleAsync(adminUser, "Administrator");
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapPost("/api/register-user", async (
                 [FromBody] RegisterDto registration,
                 [FromServices] UserManager<UsersModels> userManager,
                 [FromServices] RoleManager<IdentityRole> roleManager,
                 [FromServices] ILogger<Program> logger) =>
            {
                var user = new UsersModels
                {
                    UserName = registration.Email,
                    Email = registration.Email,
                    EmailConfirmed = true 
                };

                var result = await userManager.CreateAsync(user, registration.Password);

                if (!result.Succeeded)
                {
                    return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
                }

                try
                {
                    await userManager.AddToRoleAsync(user, "CommonUser");
                }
                catch (Exception ex)
                {}

                return Results.Ok(new { Message = "User registered successfully and assigned CommonUser role." });
            });


            app.MapControllers();
            app.MapIdentityApi<UsersModels>();
            app.Run();
        }
    }
}
