using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController:BaseController
    {
        DataContext _datacontext;
        ITokenService _tokenservice;
        public AccountController(DataContext dataContext,ITokenService tokenService)
        {
            _datacontext=dataContext;
            _tokenservice=tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> register(RegisterDTO registerDTO){

          if(await isUserExists(registerDTO.UserName)) return BadRequest("User is already taken");
           using var hash=new HMACSHA512();

           var registerUser=new AppUser(){
               UserName=registerDTO.UserName.ToLower(),
               HashPassword= hash.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
               SaltPassword=hash.Key
           };

          _datacontext.Add(registerUser);
           await _datacontext.SaveChangesAsync();
     
           return new UserDTO{
                    UserName=registerUser.UserName,
                    Token=_tokenservice.CreateToken(registerUser)
                };
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> login(LoginDTO loginDTO){
                  var user=await _datacontext.User
                           .SingleOrDefaultAsync(x =>x.UserName ==loginDTO.UserName);

                  if(user ==null) return Unauthorized("User no found");

                  using var hmac=new HMACSHA512(user.SaltPassword);
                  var hashPassword=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

                  for(int i=0;i<hashPassword.Length;i++){
                     if(hashPassword[i] !=user.HashPassword[i]) return Unauthorized("Passowrd Wrong");
                  }

                return new UserDTO{
                    UserName=user.UserName,
                    Token=_tokenservice.CreateToken(user)
                };
        }

        private async  Task<bool> isUserExists(string userName){
             return await _datacontext.User.AnyAsync(x =>x.UserName==userName.ToLower()); 
        } 
    }
}