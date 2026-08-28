using ItemMaster.Server.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ItemMaster.Server.Data
{
    public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<BudgetSharing>().HasKey(c => new { c.BudgetNo, c.BudgetSharingNo });
			//modelBuilder.Entity<IdentityRole>().Property(p => p.Id).HasMaxLength(50).IsUnicode(false);
			//modelBuilder.Entity<User>().Property(p => p.Id).HasMaxLength(50).IsUnicode(false);


			//   modelBuilder.HasSequence("FABudget");

		}
        public DbSet<Group> Groups { get; set; }
        public DbSet<FABudget> FABudgets { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<FABudgetHistory> FABudgetHistories { get; set; }
        public DbSet<GroupEmailSend> GroupEmailSends { get; set; }
        public DbSet<BudgetSharing> BudgetSharings { get; set; }
        public DbSet<PODataUpload> PODataUploads { get; set; }
    }
}

