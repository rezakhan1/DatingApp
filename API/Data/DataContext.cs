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
    }
}