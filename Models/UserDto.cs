public class UserDto
{
    public string Id {get;set;}
    public string Name {get;set;}
    public string Email{get;set;}

    public string Roles{get;set;}

    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }

    public List<string> claims{get;set;}
}