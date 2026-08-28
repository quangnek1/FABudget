using ItemMaster.Server.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ItemMaster.Server.Data
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Division> Divisions { get; set; }
    }
}
