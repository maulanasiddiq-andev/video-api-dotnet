using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VideoApi.Dtos;
using VideoApi.Exceptions;
using VideoApi.Models;

namespace VideoApi.Repositories
{
    public class UserRepository
    {
        private readonly VideoAppDBContext _dBContext;
        private readonly IMapper _mapper;
        public UserRepository(VideoAppDBContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            IQueryable<UserModel> listUserQuery = _dBContext.User.AsQueryable();

            #region Ordering
            string orderBy = "createdTime";
            string orderDir = "desc";

            if (orderBy.Equals("createdTime"))
            {
                if (orderDir.Equals("asc"))
                {
                    listUserQuery = listUserQuery.OrderBy(x => x.CreatedTime).AsQueryable();
                }
                else if (orderDir.Equals("desc"))
                {
                    listUserQuery = listUserQuery.OrderByDescending(x => x.CreatedTime).AsQueryable();
                }
            }
            #endregion
            
            var listUser = await listUserQuery.ToListAsync();
            var userDtos = _mapper.Map<List<UserDto>>(listUser);

            return userDtos;
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            UserModel? user = await _dBContext.User.Where(user => user.UserId == id).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new KnownException("User tidak ditemukan");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}