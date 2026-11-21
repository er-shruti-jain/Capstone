using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<RolesService>();

builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("CapstoneDB")));

builder.Services.AddIdentity<UserModel,IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login"; // Set the login page path
    options.LogoutPath = "/login"; // Set the logout page path
    options.AccessDeniedPath = "/login"; // Set the access denied page path
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("CanCreate", policy =>
        policy.RequireClaim("Create", "True"))
    .AddPolicy("CanRead", policy =>
        policy.RequireClaim("Read", "True"))
    .AddPolicy("CanUpdate", policy =>
        policy.RequireClaim("Update", "True"))
    .AddPolicy("CanDelete", policy =>
        policy.RequireClaim("Delete", "True"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleService = scope.ServiceProvider
        .GetRequiredService<RolesService>();

    await roleService.InitializeRolesAsync();
}


app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

string[] roles = new string[] { "Admin", "User" };
