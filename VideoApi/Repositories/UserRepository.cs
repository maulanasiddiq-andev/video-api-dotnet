using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VideoApi.Constants;
using VideoApi.Dtos;
using VideoApi.Dtos.Requests;
using VideoApi.Exceptions;
using VideoApi.Models;
using VideoApi.Responses;

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

        public async Task<SearchResponse> GetUsersAsync(SearchRequestDto search)
        {
            IQueryable<UserModel> listUserQuery = _dBContext.User
                .Where(x => x.RecordStatus.ToLower().Equals(RecordStatusConstant.Active.ToLower()))
                .AsQueryable();

            #region Ordering
            string orderBy = search.OrderBy;
            string orderDir = search.OrderDir;

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

            var response = new SearchResponse();
            response.TotalItems = await listUserQuery.CountAsync();
            response.CurrentPage = search.CurrentPage;
            response.PageSize = search.PageSize;

            var skip = search.PageSize * search.CurrentPage;
            var take = search.PageSize;
            var listUser = await listUserQuery.Skip(skip).Take(take).ToListAsync();
            
            response.Items = _mapper.Map<List<UserDto>>(listUser);

            return response;
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