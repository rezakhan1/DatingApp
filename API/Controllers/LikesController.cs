using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Extension;
using API.Helper;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
   
    public class LikesController : BaseController
    {
        private readonly IUserRepository _IUserRepo;
        private readonly ILikesRepository _LikeRepo;

        public LikesController(IUserRepository IuserRepo, ILikesRepository IlikeRepo)
        {
            _IUserRepo=IuserRepo;
            _LikeRepo=IlikeRepo;
        }

       [HttpPost("{username}")]
       public async Task<ActionResult> AddLike(string userName){
        var sourceUserId = User.getUserId();
            var likedUser = await _IUserRepo.GetUserByUserName(userName);
            var sourceUser = await _LikeRepo.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == userName) return BadRequest("You cannot like yourself");

            var userLike = await _LikeRepo.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _IUserRepo.SavaAllAsync()) return Ok();

            return BadRequest("Failed to like user");
       }
     [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.getUserId();
            var users = await _LikeRepo.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage,
                users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}