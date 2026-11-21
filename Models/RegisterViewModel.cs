using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    public string Name {get;set;}
    [Required]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}