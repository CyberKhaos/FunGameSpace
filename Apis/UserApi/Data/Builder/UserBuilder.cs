using Microsoft.EntityFrameworkCore;

namespace UserApi.Data.Builder;

public static class UserBuilder
{
    public static void BuildUser(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Account.User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("user");
            entity.Property(e => e.Id).HasMaxLength(36).HasColumnName("Id");
            entity.Property(e => e.Name).HasMaxLength(25).HasColumnName("Name");
            entity.Property(e => e.Email).HasMaxLength(50).HasColumnName("Email");
            entity.Property(e => e.EmailHashed).HasMaxLength(128).HasColumnName("EmailHash");
            entity.Property(e => e.PasswordHashed).HasMaxLength(128).HasColumnName("PasswordHash");
        });
    }
}