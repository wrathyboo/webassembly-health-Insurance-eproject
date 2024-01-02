using HealthInsurance.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using System.Reflection.Metadata;


namespace HealthInsurance.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyApprovalDetails> PolicyApprovalDetails { get; set; }
        public DbSet<PolicyRequest> PolicyRequests { get; set; }
        public DbSet<PolicyOnEmployee> PolicyOnEmployees { get; set; }
        public DbSet<PolicyTotalDescription> PolicyTotalDescriptions { get; set; }
        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// Overrides tables properties
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //User Represents the user.
            builder.Entity<ApplicationUser>(i =>
            {
                i.ToTable(name: "Users");
                i.HasOne(p => p.PolicyOnEmployee);
            });

            //Set default created date
            builder.Entity<ApplicationUser>().Property(b => b.CreatedAt)
                   .HasDefaultValueSql("getdate()");
            builder.Entity<PolicyRequest>().Property(b => b.CreatedAt)
                   .HasDefaultValueSql("getdate()");

            //Role    Represents a role.
            builder.Entity<IdentityRole>(i =>
            {
                i.ToTable("Roles");
            });

            //UserClaim Represents a claim that a user possesses.
            builder.Entity<IdentityUserClaim<string>>(i =>
            {
                i.ToTable("UserClaims");
            });

            //UserRole A join entity that associates users and roles.
            builder.Entity<IdentityUserRole<string>>(i =>
            {
                i.ToTable("UserRoles");
            });

            //UserLogin Associates a user with a login.
            builder.Entity<IdentityUserLogin<string>>(i =>
            {
                i.ToTable("UserLogins");
            });

            //UserToken Represents an authentication token for a user.
            builder.Entity<IdentityUserToken<string>>(i =>
            {
                i.ToTable("UserTokens");
            });

            //RoleClaim Represents a claim that's granted to all users within a role.
            builder.Entity<IdentityRoleClaim<string>>(i =>
            {
                i.ToTable("RoleClaims");
            });

            ///One company can have many policies and many policies can only have one company
            builder.Entity<Company>()
                   .HasMany(e => e.Policies)
                   .WithOne(e => e.Company)
                    .HasForeignKey(e => e.CompanyId)
                    .IsRequired(false);
            ///One company can have many PolicyOnEmployees
            builder.Entity<Company>()
                    .HasMany(e => e.PolicyOnEmployees)
                    .WithOne(e => e.Company)
                    .HasForeignKey(e => e.CompanyId)
                    .IsRequired(false);

            builder.Entity<Policy>()
                .HasKey(e => e.Id);

            builder.Entity<Company>()
                .HasKey(e => e.Id);

            //builder.Entity<ApplicationUser>()
            //    .HasOne(e => e.PolicyOnEmployee)
            //    .WithOne(e => e.ApplicationUser)
            //    .HasForeignKey<PolicyOnEmployee>(e => e.EmployeeId)
            //    .IsRequired();

            ///One to one relationship
            builder.Entity<PolicyOnEmployee>()
                .HasOne(e => e.ApplicationUser)
                .WithOne(e => e.PolicyOnEmployee)
                .HasForeignKey<ApplicationUser>(e => e.PolicyId)
                .IsRequired(false);

            ///One user can have many requests
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.PolicyRequests)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey(e => e.EmployeeId)
                .IsRequired(false);

            ///One user can only have one policyOnEmployee
            builder.Entity<ApplicationUser>()
                .HasOne(e => e.PolicyOnEmployee)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey<PolicyOnEmployee>(e => e.EmployeeId)
                .IsRequired(false);
        }
    }


}

