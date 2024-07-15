using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Backend_Chat.Data
{
    public class MessagingContext : DbContext
    {
        public MessagingContext(DbContextOptions<MessagingContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.Id_sender)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.Id_receiver)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_user { get; set; }

        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [StringLength(120)]
        public string Email { get; set; }
    }

    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_message { get; set; }

        [Required]
        [StringLength(600)]
        public string Content { get; set; }

        public int Id_sender { get; set; }
        public int Id_receiver { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;

        [JsonIgnore]
        public User Sender { get; set; }

        [JsonIgnore]
        public User Receiver { get; set; }
    }
}
