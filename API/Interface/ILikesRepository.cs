using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interface
{
    public interface ILikesRepository
    {
         Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
         Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams);
    }
}