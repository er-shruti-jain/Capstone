using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    private readonly UserManager<UserModel> _userManager;

    [BindProperty]
    public RegisterViewModel registerModel { get; set; }
    public RegisterModel(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        if(!UtilityService.IsValidInput(registerModel.Name,""))
        {
            ModelState.AddModelError(string.Empty,"Please enter valid Name");
            return Page();
        }

        var existingUser = _userManager.FindByEmailAsync(registerModel.Email).Result;
        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Email is already registered.");
            return Page();
        }
        var user = new UserModel {
            Name=registerModel.Name,
             UserName = registerModel.Email,
              Email = registerModel.Email, 
              IsActive=false,
              CreatedDate=DateTime.Now
               };
        var result = _userManager.CreateAsync(user, registerModel.Password).Result;
        if (result.Succeeded)
        {
            _userManager.AddToRoleAsync(user, "User").Wait();
            var claims = new List<Claim>
            {
                new Claim("Create", "True"),
                new Claim("Delete", "True"),
                new Claim("Read", "True"),
                new Claim("Update", "True")
            };
            _userManager.AddClaimsAsync(user, claims).Wait();
            return RedirectToPage("/login");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }

}