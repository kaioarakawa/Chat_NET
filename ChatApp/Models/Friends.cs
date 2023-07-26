namespace ChatApp.Models
{
	public class Friends
	{
		public int ID { get; set; }
		public bool IsConfirmed { get; set; }

		public string UserId { get; set; }
		public virtual AppUser User { get; set; }

		public string UserFriendId { get; set; }
		public virtual AppUser UserFriend { get; set; }
	}
}
