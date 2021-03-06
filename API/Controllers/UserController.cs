using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Extension;
using API.Helper;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   
   [Authorize]
    public class UserController:BaseController
    {
        IUserRepository _IUserRepo;
        IMapper _mapper;
        IPhotoService _photo;
        public UserController(IUserRepository userRepository,IMapper mapper,IPhotoService photo)
        {
            _IUserRepo=userRepository;
            _mapper=mapper;
            _photo=photo;
        }
 
        [HttpGet("allusers")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> getUsers([FromQuery]UserParams userParams ){
           userParams.CurrentUserName=User.getUserName();
           var gender=await _IUserRepo.getMmeberGender(User.getUserName());
           if(string.IsNullOrEmpty(userParams.Gender)){
               userParams.Gender=gender=="male"?"female":"male";
           }
           
            var users=await _IUserRepo.GetMembersAsync(userParams);
           // var mappedUsers=_mapper.Map<IEnumerable<MemberDTO>>(users);
           Response.AddPaginationHeader(users.CurrentPage,users.PageSize
                                       ,users.TotalCount,users.TotalPages);
           return Ok(users);
        }
        //api/user/2
        [HttpGet("{username}" , Name ="GetUser")]
        public async Task<ActionResult<MemberDTO>> getUsersbyId(string username){
            var user=await _IUserRepo.GetMemberAsync(username.ToLower()); 
            return user;
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateMemberDTO memberUpdateDto)
        {
           var username= User.getUserName();
            var user = await _IUserRepo.GetUserByUserName(username);

            _mapper.Map(memberUpdateDto, user);

            _IUserRepo.Update(user);
            if(await _IUserRepo.SavaAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> addPhoto(IFormFile file){
        var user=await _IUserRepo.GetUserByUserName(User.getUserName());
        var result=await _photo.AddPhotoAsync(file);

        if(result.Error != null) return BadRequest(result.Error.Message+ " Error While Uploading Image(s)");
          var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _IUserRepo.SavaAllAsync())
            {
                return CreatedAtRoute("GetUser", new {username= user.UserName},_mapper.Map<PhotoDTO>(photo));  
            }
            return BadRequest("Problem addding photo");            
        }
        
        [HttpPost("set-photo-main/{photoId}")]
        public async Task<ActionResult> setMainPhoto(int photoId){
           var user=await _IUserRepo.GetUserByUserName(User.getUserName());
           var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);
           if(photo.IsMain) return BadRequest("This is already your main photo");
           var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _IUserRepo.SavaAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

          [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _IUserRepo.GetUserByUserName(User.getUserName());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photo.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _IUserRepo.SavaAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
   
    }
    
}