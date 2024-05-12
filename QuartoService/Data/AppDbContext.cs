using Microsoft.EntityFrameworkCore;
using QuartoService.Models;

namespace QuartoService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Quarto> Quartos { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Hotel>()
                .HasMany(h => h.Quartos)
                .WithOne(h => h.Hotel!)
                .HasForeignKey(h => h.IdHotel);

            modelBuilder
                .Entity<Quarto>()
                .HasOne(h => h.Hotel!)
                .WithMany(h => h.Quartos)
                .HasForeignKey(q => q.IdHotel); 
        }
    }
}