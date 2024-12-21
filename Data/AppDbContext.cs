using filmizleme.Entities;
using Microsoft.EntityFrameworkCore;

namespace filmizleme.Data
{
    public class AppDbContext : DbContext
    {
        public required DbSet<Film> Films { get; set; }
        public required DbSet<User> Users { get; set; }
        public required DbSet<WatchedFilm> WatchedFilms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WatchedFilm>()
                .HasKey(wf => wf.Id);

            modelBuilder.Entity<WatchedFilm>()
                .HasOne<User>()  
                .WithMany()      
                .HasForeignKey(wf => wf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchedFilm>()
                .HasOne<Film>()  
                .WithMany()      
                .HasForeignKey(wf => wf.FilmId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Film>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();  
      
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique(); 

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
