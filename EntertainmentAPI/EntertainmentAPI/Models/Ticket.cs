namespace EntertainmentAPI.Models
{

    public class Ticket
    {
        public int TicketId { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}