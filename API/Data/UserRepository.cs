using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        /*  97 Add to improve queries, to not query the whole User from Database
         *  In order to get just the fields required manually its posssible to use projection
         *  
         *  istead of use Select(user-> new MemberDto {Id= user.Id, Username = user.Username})SingleOrDefaultAsync();
         *  
         *  ProjectTo<MemberDto>(mapper).SingleOrDefaultAsync();
         *  When Projection is used, there's no need to use Include(p->p.sfasf) because EF works out query to join the table
         */
        
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }



        //97 Add to improve queries, to not query the whole User from Database
        // 154 change <IEnumerable<MemberDto>> to <PagedList<MemberDto>> and () for (parameters) to know wich page show
        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            // query is a IQueriable -> EF is gonna build this query to an expression tree
            // and then when we execute the ToList() it get executed into database
            //use context.Users and not Members, as it's necessary to access to all fields
            var query = _context.Users.AsQueryable();
            
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            //query = query.Where(u => u.UserName != userParams.CurrentUsername); //dont show user itself
            //query = query.Where(u => u.Gender == userParams.Gender);            //filter Gender
            //query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            // .AsNoTracking turns off Tracking in EF (we just need to read it)
            IQueryable<MemberDto> member = query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking();
            
            // CreateAsync takes ( source, pageNumber,Pagesize )
            return await PagedList<MemberDto>.CreateAsync(member, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)                             //eager loading to include photos in the query
                .SingleOrDefaultAsync(x => x.UserName == username); //Returns the only element of a sequence or default > error if it's duplicated
        }



        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)                             //eager loading to include photos in the query
                .ToListAsync();                                     //creates a List<> from an IQueriable<>
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}