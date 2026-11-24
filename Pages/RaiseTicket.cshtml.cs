using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

[Authorize]
public class RaiseTicketModel : PageModel
{

    public string Email => User.FindFirstValue(ClaimTypes.Email);

    [BindProperty]
    public string Message { get; set; }

    private IConfiguration _config;

    public RaiseTicketModel(IConfiguration config)
    {
        _config = config;
    }

    public void OnGet()
    {
       
    }

    public IActionResult OnPost()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool isvalidinput = UtilityService.IsValidInput(Message, ".,-_?\\/\" ");
        if (!isvalidinput)
        {
            ModelState.AddModelError(string.Empty, "Invalid Message. please use only A-Z, a-z, 0-9, .,-_/\\?");
            return Page();
        }

        using SqlConnection sqlConnection = new SqlConnection(_config.GetConnectionString("CapstoneDB"));//test'',''submit''); update TicketMaster set status=''notSubmit'';--

        string query = @"insert into TicketMaster(Userid,Message,status,createddate)
        values(@userid,@message,@status,@createddate)";
        // values('" + userId + "','" + Message + "','submit','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')";
        using SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
        sqlCommand.Parameters.AddWithValue("@userid", userId);
        sqlCommand.Parameters.AddWithValue("@message", Message);
        sqlCommand.Parameters.AddWithValue("@status", "submit");
        sqlCommand.Parameters.AddWithValue("@createddate", DateTime.Now);

        sqlConnection.Open();
        var result = sqlCommand.ExecuteNonQuery();

        return RedirectToPage("ManageTickets");

    }
}