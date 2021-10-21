using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   
   [Authorize]
    public class UserController:BaseController
    {
        IUserRepository _IUserRepo;
        IMapper _mapper;
        public UserController(IUserRepository userRepository,IMapper mapper)
        {
            _IUserRepo=userRepository;
            _mapper=mapper;
        }
 
        [HttpGet("allusers")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> getUsers(){

            var users=await _IUserRepo.GetMembersAsync();
           // var mappedUsers=_mapper.Map<IEnumerable<MemberDTO>>(users);
           return Ok(users);
        }
        //api/user/2
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> getUsersbyId(string username){
            var user=await _IUserRepo.GetMemberAsync(username.ToLower()); 
            return user;
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateMemberDTO memberUpdateDto)
        {
           var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _IUserRepo.GetUserByUserName(username);

            _mapper.Map(memberUpdateDto, user);

            _IUserRepo.Update(user);
            if(await _IUserRepo.SavaAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }
    }
}