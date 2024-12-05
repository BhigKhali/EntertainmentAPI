namespace EntertainmentAPI.Models
{

    using System.Net.Sockets;

    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } //store hashed passedword
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
