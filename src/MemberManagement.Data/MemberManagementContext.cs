using MemberManagement.AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemberManagement.Data
{
    public class MemberManagementContext : DbContext
    {
        public MemberManagementContext(DbContextOptions<MemberManagementContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Member>(ConfigureMember);
            builder.Entity<Member>()
                .HasIndex(m => m.UserName)
                .IsUnique();
        }

        private void ConfigureMember(EntityTypeBuilder<Member> builder)
        {
            builder.Property(m => m.UserName)
                .IsRequired()
                .HasMaxLength(12);

            builder.Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(75);

            builder.Property(m => m.PhoneNumber)
                .HasMaxLength(10);

            builder.Property(m => m.DateOfBirth)
                .IsRequired();
        }
    }
}
