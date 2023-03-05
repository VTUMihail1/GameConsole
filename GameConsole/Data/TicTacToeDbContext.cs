using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameConsole.Data.Entities;

namespace GameConsole.Data
{
    class TicTacToeDbContext : DbContext
    {

        public DbSet<TicTacToeTable> TicTacToeRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-V01UMR2\\SQLEXPRESS; Database=GameCenterDb; " +
                "Trusted_Connection=True; TrustServerCertificate=True");
        }
    }
}
