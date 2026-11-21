using System.Security.Claims;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "Admin")]
//[Authorize(Policy = "CanRead")]
public class ManageUsersModel : PageModel
{
    [BindProperty]
    public List<UserDto> Users { get; set; }
    private UserManager<UserModel> _userManager;

    public ManageUsersModel(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        var users = _userManager.Users.ToList();
        Users = users.Select(u =>
        {
            var roles = _userManager.GetRolesAsync(u).Result;
            var claims = _userManager.GetClaimsAsync(u).Result;
            return new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate,
                Roles = string.Join(",", roles),
                claims = claims.Select(claim => $"{claim.Type}:{claim.Value}").ToList()

                // _userManager.GetRolesAsync(u).Result.FirstOrDefault()
            };
        }
        ).ToList();

    }

    public async Task<IActionResult> OnGetActivateAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        user.IsActive = !user.IsActive;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            // Handle errors
            return BadRequest(result.Errors);
        }

        return RedirectToAction("OnGet");
    }
public async Task<IActionResult> OnGetUserPermissionAsync(string userId)
    {
        var claims = await _userManager.GetClaimsAsync( await _userManager.FindByIdAsync(userId));


    var result = new
    {
        UserId = userId,
        Create = claims.Any(c => c.Type == "Create" && c.Value == "True"),
        Read = claims.Any(c => c.Type == "Read" && c.Value == "True"),
        Update = claims.Any(c => c.Type == "Update" && c.Value == "True"),
        Delete = claims.Any(c => c.Type == "Delete" && c.Value == "True")
    };

        return new JsonResult(result);
        
    }

    public async Task<IActionResult> OnPostUpdateClaimAsync(string UserId,bool create, bool read, bool update, bool delete)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        var userClaims = await _userManager.GetClaimsAsync(user);

var permissions = new Dictionary<string,bool>
{
  {"Read",read},  
  {"Create",create},
  {"Update",update},
  {"Delete",delete}
};

foreach(var permission in permissions)
        {
            var oldclaim = userClaims.FirstOrDefault(c=>c.Type==permission.Key);
            var newclaim = new Claim(permission.Key,permission.Value.ToString());
            if(oldclaim != null)
            {
               await _userManager.ReplaceClaimAsync(user,oldclaim,newclaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user,newclaim);
            }
        }

        return RedirectToPage("ManageUsers");
    }



}