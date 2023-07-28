using Microsoft.AspNetCore.Identity;

namespace ChatApp.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser() 
        {
            Messages = new HashSet<Message>();
            MessagesToUsers = new HashSet<Message>();
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        // 1 - * AppUser || Message
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Message> MessagesToUsers { get; set; }
        //public virtual ICollection<Message> MessagesOf { get; set; }
        public ICollection<Friends> FriendsOf { get; set; }
		public ICollection<Friends> Friends { get; set; }
	}
}
