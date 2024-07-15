using Microsoft.AspNetCore.Mvc;
using Backend_Chat.Data;
using Backend_Chat.DTOs;
using Backend_Chat.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;

namespace Backend_Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessagingContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(MessagingContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        public IActionResult AddMessage([FromBody] MessageCreateDTO messageDTO)
        {
            if (messageDTO == null)
            {
                return BadRequest();
            }

            // Check if sender and receiver exist
            var senderExists = _context.Users.Any(u => u.Id_user == messageDTO.Id_sender);
            var receiverExists = _context.Users.Any(u => u.Id_user == messageDTO.Id_receiver);

            if (!senderExists || !receiverExists)
            {
                return NotFound("Sender or receiver not found");
            }

            var message = new Message
            {
                Content = messageDTO.Content,
                Id_sender = messageDTO.Id_sender,
                Id_receiver = messageDTO.Id_receiver,
                Time = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            // Notify clients about the new message
            _hubContext.Clients.All.SendAsync("ReceiveMessage", messageDTO.Id_sender.ToString(), messageDTO.Content);

            return Ok(message);
        }

        [HttpGet("{idSender}/{idReceiver}")]
        public IActionResult GetMessages(int idSender, int idReceiver)
        {
            var messages = _context.Messages
                .Where(m => (m.Id_sender == idSender || m.Id_sender == idReceiver) && (m.Id_receiver == idReceiver || m.Id_receiver == idSender))
                .OrderBy(m => m.Time)
                .ToList();
            return Ok(messages);
        }
    }
}
