using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RapidGames.Models; 

namespace RapidGames.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Review> Reviews { get; set; } 
        public DbSet<CategoryGames> CategoryGames { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<CategoryGames>()
                .HasOne(cg => cg.Category)      
                .WithMany(c => c.CategoryGames)  
                .HasForeignKey(cg => cg.CategoryId); 

            builder.Entity<CategoryGames>()
                .HasOne(cg => cg.Game)          
                .WithMany(g => g.CategoryGames)  
                .HasForeignKey(cg => cg.GameId);   
            
        }
    }
}