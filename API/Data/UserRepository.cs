using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Helper;
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

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams  )
        {
          var query=  _dataContext.User.AsQueryable();

          query=query.Where(x=>x.UserName !=userParams.CurrentUserName).
                      Where(x=>x.Gender ==userParams.Gender);
       
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            query=userParams.OrderBy switch{
              "created"=>query.OrderByDescending(u=>u.Created),
              _=>query.OrderByDescending(u=>u.LastActive)
            };
               return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                                                            .AsNoTracking(),
                    userParams.PageNumber, userParams.PageSize);

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