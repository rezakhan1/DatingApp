using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _dataContext;
        public readonly IMapper _mapper;
        public UserRepository(DataContext dataContext,IMapper mapper)
        {
            _dataContext=dataContext;
            _mapper=mapper;
            
          //  _dataContext.Photos.ToListAsync();
            //_dataContext.User.ToListAsync();
        }

        public async Task<MemberDTO> GetMemberAsync(string username)
        {
           return await  _dataContext.User.
            Where(X=>X.UserName==username)
           .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)//project to map
           .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
          return await _dataContext.User.
            ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int Id)
        {
          return await _dataContext.User.FindAsync(Id);
        }

      public async  Task<AppUser> GetUserByUserName(string userName)
        {
           return await _dataContext.User.Include(p=>p.Photos).SingleOrDefaultAsync(x=>x.UserName ==userName);
        }

       public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {

            return await _dataContext.User.Include(p=>p.Photos).ToListAsync();
        }

      public async  Task<bool> SavaAllAsync()
        {
          return await _dataContext.SaveChangesAsync()>0;
        }

        void IUserRepository.Update(AppUser appUser)
        {
           _dataContext.Entry(appUser).State=EntityState.Modified;
        }

        
    }
}