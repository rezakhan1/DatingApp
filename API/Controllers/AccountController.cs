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
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController:BaseController
    {
        DataContext _datacontext;
        ITokenService _tokenservice;

        IMapper _mapper;
        public AccountController(DataContext dataContext,ITokenService tokenService,IMapper mapper)
        {
            _datacontext=dataContext;
            _tokenservice=tokenService;
           _mapper=mapper;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> register(RegisterDTO registerDTO){

          if(await isUserExists(registerDTO.UserName)) return BadRequest("User is already taken");
           var user=_mapper.Map<AppUser>(registerDTO);
           using var hash=new HMACSHA512();

           
               user.UserName=registerDTO.UserName.ToLower();
               user.HashPassword= hash.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
               user.SaltPassword=hash.Key;
        

          _datacontext.Add(user);
           await _datacontext.SaveChangesAsync();
     
           return new UserDTO{
                    UserName=user.UserName,
                    Token=_tokenservice.CreateToken(user),
                    KnownAs=user.KnownAs
                };
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> login(LoginDTO loginDTO){
                  var user=await _datacontext.User
                           .Include(p=>p.Photos)
                           .SingleOrDefaultAsync(x =>x.UserName ==loginDTO.UserName);

                  if(user ==null) return Unauthorized("User no found");

                  using var hmac=new HMACSHA512(user.SaltPassword);
                  var hashPassword=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

                  for(int i=0;i<hashPassword.Length;i++){
                     if(hashPassword[i] !=user.HashPassword[i]) return Unauthorized("Passowrd Wrong");
                  }

                return new UserDTO{
                    UserName=user.UserName,
                    Token=_tokenservice.CreateToken(user),
                    photoUrl=user.Photos.FirstOrDefault(x=>x.IsMain).Url,
                    KnownAs=user.KnownAs

                    
                };
        }

        private async  Task<bool> isUserExists(string userName){
             return await _datacontext.User.AnyAsync(x =>x.UserName==userName.ToLower()); 
        } 
    }
}