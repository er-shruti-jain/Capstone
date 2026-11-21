using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class LoginModel : PageModel
{
    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }
    private readonly ILogger<LoginModel> _logger;
    private readonly SignInManager<UserModel> _signInManager;
    private readonly UserManager<UserModel> _userManager;

    public LoginModel(ILogger<LoginModel> logger, SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Email);

        var result = await _signInManager.PasswordSignInAsync
        (Email, Password, isPersistent: false, lockoutOnFailure: false);

        // Perform login logic here
        // For example, you can use the UserManager to validate the user's credentials
        if (result.Succeeded)
            return RedirectToPage("/MyAccount");
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }

    public IActionResult OnGetLogoutAsync()
    {
        _signInManager.SignOutAsync().Wait();
        return RedirectToPage("/Index");
    }

}