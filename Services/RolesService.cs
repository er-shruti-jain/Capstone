
using Microsoft.AspNetCore.Identity;

public class RolesService
{
    private RoleManager<IdentityRole> _rolemanager;
    private string[] roles = {"Admin", "User"};

    public RolesService(RoleManager<IdentityRole> roleManager)
    {
        _rolemanager = roleManager;
    }

  public async Task InitializeRolesAsync()
{
    foreach (string role in roles)
    {
        IdentityRole roleExists =
            await _rolemanager.FindByNameAsync(role);

        if (roleExists == null)
        {
            await _rolemanager.CreateAsync(new IdentityRole(role));
        }
    }
}
}