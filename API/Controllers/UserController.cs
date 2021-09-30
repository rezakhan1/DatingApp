using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   
    public class UserController:BaseController
    {
        DataContext _DataContext;
        public UserController(DataContext dataContext)
        {
            _DataContext=dataContext;
        }
        [HttpGet("allusers")]
        public async Task<ActionResult<IEnumerable<AppUser>>> getUsers(){
           return await _DataContext.User.ToListAsync();
        }
        //api/user/2
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> getUsersbyId(int id){
           return await _DataContext.User.FindAsync(id);
        }
    }
}