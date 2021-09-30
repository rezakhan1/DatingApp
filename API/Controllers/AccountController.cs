using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController:BaseController
    {
        DataContext _datacontext;
        public AccountController(DataContext dataContext)
        {
            _datacontext=dataContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> register(RegisterDTO registerDTO){

          if(await isUserExists(registerDTO.UserName)) return BadRequest("User is already taken");
           using var hash=new HMACSHA512();

           var registerUser=new AppUser(){
               UserName=registerDTO.UserName.ToLower(),
               HashPassword= hash.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
               SaltPassword=hash.Key
           };

          _datacontext.Add(registerUser);
           await _datacontext.SaveChangesAsync();

          return registerUser;
        }

        private async  Task<bool> isUserExists(string userName){
             return await _datacontext.User.AnyAsync(x =>x.UserName==userName.ToLower()); 
        } 
    }
}