using FileManagement.Authentication.Models;
using FileManagement.Authentication.Services;
using FileManagement.IdentityServer.In_Memory;
using FileManagement.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
string migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/
builder.Services.AddDbContext<AuthDbContext>(s=>s.UseSqlServer(configuration.GetConnectionString("FileManagementSystem"),
            sql => sql.MigrationsAssembly(migrationsAssembly)));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddIdentityServer()
           .AddTestUsers(IdentityConfig.TestUsers)
           .AddConfigurationStore(options =>
           {
               options.ConfigureDbContext = b => b.UseSqlServer(configuration.GetConnectionString("FileManagementSystem"),
               sql => sql.MigrationsAssembly(migrationsAssembly));
           })
           .AddOperationalStore(options =>
           {
               options.ConfigureDbContext = b => b.UseSqlServer(configuration.GetConnectionString("FileManagementSystem"),
               sql => sql.MigrationsAssembly(migrationsAssembly));
           })
           .AddAspNetIdentity<User>().AddDeveloperSigningCredential();

var app = builder.Build();

DatabaseConfig.InitializeDatabase(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


