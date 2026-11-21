using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

[Authorize]
public class RaiseTicketModel : PageModel
{

    [BindProperty]
    public string Email { get; set; }

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

    public void OnPost()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        using SqlConnection sqlConnection = new SqlConnection(_config.GetConnectionString("CapstoneDB"));//test'',''submit''); update TicketMaster set status=''notSubmit'';--

        string query = @"insert into TicketMaster(Userid,Message,status,createddate)
        values(@userid,@message,@status,@createddate)";
       // values('" + userId + "','" + Message + "','submit','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')";
      using  SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
        sqlCommand.Parameters.AddWithValue("@userid",userId);
        sqlCommand.Parameters.AddWithValue("@message",Message);
        sqlCommand.Parameters.AddWithValue("@status","submit");
        sqlCommand.Parameters.AddWithValue("@createddate",DateTime.Now);

        sqlConnection.Open();
        var result = sqlCommand.ExecuteNonQuery();


    }
}