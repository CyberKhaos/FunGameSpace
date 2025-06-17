using Microsoft.EntityFrameworkCore;
using UserApi.Data.Builder;

namespace UserApi.Data;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }
    
    public DbSet<Models.Account.User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");
        modelBuilder.BuildUser();
    }
}