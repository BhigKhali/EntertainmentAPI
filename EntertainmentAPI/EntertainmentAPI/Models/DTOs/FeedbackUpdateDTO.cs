namespace EntertainmentAPI.Models.DTOs
{
    public class FeedbackUpdateDTO
    {
        public int FeedbackId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
