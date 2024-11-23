namespace EntertainmentAPI.Data
{

    using Microsoft.EntityFrameworkCore;
    using EntertainmentAPI.Models;
    using Microsoft.Extensions.Logging;
    using System.Net.Sockets;

    public class EntertainmentDbContext : DbContext
    {
        public EntertainmentDbContext(DbContextOptions<EntertainmentDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
