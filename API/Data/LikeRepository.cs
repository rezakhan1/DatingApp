using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Extension;
using API.Helper;
using API.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikeRepository : ILikesRepository
    {
        private readonly DataContext _dataContext;
        public LikeRepository(DataContext dtcontext)
        {
            _dataContext=dtcontext;
        }
       async Task<UserLike> ILikesRepository.GetUserLike(int sourceUserId, int likedUserId)
        {
          return await  _dataContext.UserLikes.FindAsync(sourceUserId,likedUserId);
        }

       async Task<PagedList<LikeDTO>> ILikesRepository.GetUserLikes(LikesParams likesParams)
        {
              var users = _dataContext.User.OrderBy(u => u.UserName).AsQueryable();
            var likes = _dataContext.UserLikes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var  likedUsers=  users.Select(user => new LikeDTO
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.calculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

             return await PagedList<LikeDTO>.CreateAsync(likedUsers, 
                 likesParams.PageNumber, likesParams.PageSize);
        }

      async  Task<AppUser> ILikesRepository.GetUserWithLikes(int userId)
        {
          return await   _dataContext.User.Include(x=>x.LikedUsers)
                         .FirstOrDefaultAsync(x=>x.Id==userId);

            
        }
    }
}