using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entity;
using API.Helper;

namespace API.Interface
{
    public interface IUserRepository
    {
        void Update(AppUser appUser);
        Task<bool> SavaAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int Id);

       Task<AppUser> GetUserByUserName(string userName);
        Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);
        Task<MemberDTO> GetMemberAsync(string username);

        Task<string>    getMmeberGender(string username);
    }
}