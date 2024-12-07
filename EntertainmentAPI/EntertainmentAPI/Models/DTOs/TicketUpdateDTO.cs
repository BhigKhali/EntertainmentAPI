namespace EntertainmentAPI.Models.DTOs
{
    public class TicketUpdateDTO
    {
        public int TicketId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
