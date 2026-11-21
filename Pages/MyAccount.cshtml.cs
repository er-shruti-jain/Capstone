using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize]
public class MyAccountModel : PageModel
{
    public void OnGet()
    {
    }
}