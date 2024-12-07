namespace EntertainmentAPI.Models.DTOs
{
    public class FeedbackCreateDTO
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
