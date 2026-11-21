
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

public class ManageTickets : PageModel
{
    [BindProperty]
    public List<TicketModel> Tickets {get;set;} = [];

    private IConfiguration _config;

    public ManageTickets(IConfiguration config)
    {
        _config = config;
    }

    public void OnGet()
    {

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        using SqlConnection sqlConnection = new SqlConnection(_config.GetConnectionString("CapstoneDB"));

        string query = @"select tm.TicketId,tm.userID,um.Name,um.Email, tm.Message, tm.status, tm.Createddate
          from TicketMaster as tm join CRM_UserMaster as um on tm.UserId = um.Id 
          where tm.userId like @userid";

        using SqlCommand sqlCommand = new SqlCommand(query,sqlConnection);
        sqlCommand.Parameters.AddWithValue("@userid",userId);

        sqlConnection.Open();
        using var reader =  sqlCommand.ExecuteReader();

        while (reader.Read())
        {
        
            Tickets.Add(new TicketModel
            {

                Id = reader["TicketId"].ToString(),
                UserId = reader["UserId"].ToString(),
                UserName = reader["Name"].ToString(),
                 Email = reader["Email"].ToString(),
                Message = reader["Message"].ToString(),
                Status = reader["Status"].ToString(),
                CreatedDate = Convert.ToDateTime(reader["Createddate"]),
              

            });
        }
    }

    
}