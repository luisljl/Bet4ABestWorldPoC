using Bet4ABestWorldPoC.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bet4ABestWorldPoC.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BlackListToken> BlackListTokens { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Bet> Bets { get; set; }

        public AppDbContext(DbContextOptions options) : base (options)
        {

        }
    }
}
