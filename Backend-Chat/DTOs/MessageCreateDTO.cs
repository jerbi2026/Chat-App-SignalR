namespace Backend_Chat.DTOs
{
    public class MessageCreateDTO
    {
        public string Content { get; set; }
        public int Id_sender { get; set; }
        public int Id_receiver { get; set; }
    }
}
