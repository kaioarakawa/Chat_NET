using ChatApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Message>(b =>
            {
                b.HasOne<AppUser>(a => a.Sender)
                .WithMany(d => d.Messages)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull);

                b.HasOne<AppUser>(d => d.ToUser)
               .WithMany(p => p.MessagesToUsers)
               .HasForeignKey(d => d.ToUserId)
               .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<Friends>(b =>
			{
				b.HasKey(x => new { x.UserId, x.UserFriendId });

				b.HasOne(x => x.User)
					.WithMany(x => x.Friends)
					.HasForeignKey(x => x.UserId)
					.OnDelete(DeleteBehavior.Restrict);

				b.HasOne(x => x.UserFriend)
					.WithMany(x => x.FriendsOf)
					.HasForeignKey(x => x.UserFriendId)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}

        public DbSet<Message> Messages { get; set; }
		public DbSet<Friends> Friends { get; set; }
	}
}