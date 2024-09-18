using Microsoft.EntityFrameworkCore;
using PingPong.Domain.Entity;

namespace PingPong.Infrastructure.Context
{
    public class PingPongContext(DbContextOptions<PingPongContext> options) : DbContext(options)
    {
        public DbSet<PingCount> PingCounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PingCount>().Property((x => x.Id)).IsRequired().ValueGeneratedOnAdd();
        }
    }
}
