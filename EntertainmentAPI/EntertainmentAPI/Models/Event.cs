namespace EntertainmentAPI.Models
{

    using System.Net.Sockets;

    public class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}