using Microsoft.EntityFrameworkCore;
using GameConsole.Data.Entities;

namespace GameConsole.Data
{
    class SnakeDbContext : DbContext
    {

        public DbSet<SnakeTable> SnakeRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-V01UMR2\\SQLEXPRESS; Database=GameCenterDb; " +
                "Trusted_Connection=True; TrustServerCertificate=True");
        }
    }
}