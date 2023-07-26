using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime When { get; set; }

        public string UserID { get; set; }
        public virtual AppUser Sender { get; set; }
        //public string FriendId { get; set; }
        //public virtual AppUser Friend { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}
