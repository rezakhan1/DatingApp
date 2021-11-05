using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entity;
using Microsoft.EntityFrameworkCore;
namespace API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions option):base(option)
        {
            
        }
     public   DbSet<AppUser> User {get;set;}
     public   DbSet<Photo> Photos {get;set;}

     public DbSet<UserLike> UserLikes{get;set;}
      public DbSet<Message> Messages { get; set; }
      protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(x => new {x.SourceUserId,x.LikedUserId});
              

            builder.Entity<UserLike>()
                .HasOne(ur => ur.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(ur => ur.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

         builder.Entity<UserLike>()
                .HasOne(ur => ur.LikedUser)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(ur => ur.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);   
         
            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);        
        }
    }
}