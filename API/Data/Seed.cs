using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entity;
using Microsoft.EntityFrameworkCore;
using System.Text;
namespace API.Data
{
    public class Seed
    {
        public static async Task seedUser(DataContext data){
            if(await data.User.AnyAsync())return;
            var userData=await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var Users= JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach(AppUser user in Users){
                  using var hash=new HMACSHA512();

               user.UserName=user.UserName.ToLower();
              user.HashPassword= hash.ComputeHash(Encoding.UTF8.GetBytes("Pa&&wo0rd"));
               user.SaltPassword=hash.Key;
              data.User.Add(user);
            }
            await data.SaveChangesAsync();
        }
    }
}