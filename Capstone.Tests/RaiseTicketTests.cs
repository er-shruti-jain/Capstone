using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Capstone.Tests;

public class RaiseTicketTests
{

 private readonly IConfiguration _config;

    public RaiseTicketTests()
    {
        // Arrange configuration required by RaiseTicketModel
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:CapstoneDB", "TestConnectionString" }
            })
            .Build();
    }

    private RaiseTicketModel CreateModel(string email, string userId)
    {
        var model = new RaiseTicketModel(_config);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId)
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        model.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext()
            {
                User = principal
            }
        };

        return model;
    }

    [Fact]
    public void Email_AuthenticatedUser_ReturnsEmail()
    {
        // Arrange
        var model = CreateModel(
            "test@gmail.com", "101");

        // Act
        var result = model.Email;

        // Assert
        Assert.Equal("test@gmail.com", result);
    }

    [Fact]
    public void OnPost_InvalidMessage_ReturnsPage()
    {
        // Arrange
        var model = CreateModel(
            "test@gmail.com", "101");

        model.Message = "<script>alert(1)</script>";

        // Act
        var result = model.OnPost();

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.False(model.ModelState.IsValid);

        Assert.Contains(
            model.ModelState[string.Empty]!.Errors,
            error => error.ErrorMessage.Contains("Invalid Message"));
    }
}
