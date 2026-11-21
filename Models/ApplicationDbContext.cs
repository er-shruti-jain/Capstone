using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<UserModel>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

     protected override void  OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserModel>().ToTable("CRM_UserMaster");
        modelBuilder.Entity<IdentityRole>().ToTable("CRM_RoleMaster");

       modelBuilder.Entity<IdentityUserRole<string>>().ToTable("CRM_UserRole");
       modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("CRM_UserClaim");
       modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("CRM_UserLogin");
       modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("CRM_RoleClaim");
       modelBuilder.Entity<IdentityUserToken<string>>().ToTable("CRM_UserToken");
   }

}