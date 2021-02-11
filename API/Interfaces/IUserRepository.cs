using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);

        //97 Add to improve queries, to not query the whole User from Database
        // 154 change <IEnumerable<MemberDto>> to <PagedList<MemberDto>> and () for (parameters) to know wich page show
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

        //97 Add to improve queries, to not query the whole User from Database
        
        Task<MemberDto> GetMemberAsync(string username);


    }
}