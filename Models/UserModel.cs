using Microsoft.AspNetCore.Identity;

public class UserModel : IdentityUser
{
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }

    public string Name {get;set;}   
}