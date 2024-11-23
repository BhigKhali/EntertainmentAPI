namespace EntertainmentAPI.Models
{

    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}