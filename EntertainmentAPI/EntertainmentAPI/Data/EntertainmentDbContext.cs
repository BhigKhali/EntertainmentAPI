namespace EntertainmentAPI.Data
{
    using Microsoft.EntityFrameworkCore;
    using EntertainmentAPI.Models;

    public class EntertainmentDbContext : DbContext
    {
        public EntertainmentDbContext(DbContextOptions<EntertainmentDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  One-to-Many relationship between User and Ticket
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures tickets are deleted when user is deleted

            //  One-to-Many relationship between Event and Ticket
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures tickets are deleted when event is deleted

            // might configure other relationships like User-Feedback (One-to-Many)
            //modelBuilder.Entity<Feedback>()
               // .HasOne(f => f.User)
               // .WithMany(u => u.Feedbacks)
               // .HasForeignKey(f => f.UserId)
               // .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
