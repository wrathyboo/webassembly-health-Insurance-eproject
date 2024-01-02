using HealthInsurance.Server.Data;
using HealthInsurance.Server.Repository;
using HealthInsurance.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                                    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //Issuer is the server
            ValidateIssuer = true,
            //Audience is client side
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"]!))
        };
    });
builder.Services.Configure<IdentityOptions>(options =>
{
    //All users must have a unique email
    options.User.RequireUniqueEmail = true;

});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
#pragma warning restore ASP0014 // Suggest using top level route registrations
app.UseRouting();


app.MapRazorPages();
app.MapFallbackToFile("index.html");

///Automatically migrate on start up
using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //await context.Database.EnsureDeletedAsync();
    await context.Database.MigrateAsync();
}

///Assign roles on start up if not exist
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Administrator", "Member", "User" }; //Pre assigning roles on start up
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role)) //If given role names don't exit then add new role to db
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

/* On start up we will pre-create an admin account if there aren't any matching email to that account
Login credentials: 
Username = admin
Password = Admin@123
Role = Administrator 
*/
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string username = "admin";
    string email = "admin@gmail.com";
    string password = "Admin@123";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            UserName = username,
            Email = email
        };
        ///Add to database
        await userManager.CreateAsync(user, password);
        ///Assign admin role
        await userManager.AddToRoleAsync(user, "Administrator");
    }
}

//Now run 
app.Run();
