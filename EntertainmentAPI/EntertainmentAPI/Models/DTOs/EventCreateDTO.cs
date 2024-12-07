namespace EntertainmentAPI.Models.DTOs
{
    public class EventCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }

    public class EventUpdateDTO : EventCreateDTO
    {
        public int EventId { get; set; }
    }


}
