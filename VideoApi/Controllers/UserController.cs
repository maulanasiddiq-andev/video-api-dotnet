using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoApi.Exceptions;
using VideoApi.Models;
using VideoApi.Repositories;
using VideoApi.Responses;

namespace VideoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<BaseResponse> GetUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();

                return new BaseResponse(true, null, users);
            }
            catch (Exception ex)
            {
                return new BaseResponse(false, ex.Message, null);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<BaseResponse> GetUserByIdAsync([FromRoute] string id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                return new BaseResponse(true, null, user);
            }
            catch (KnownException ex)
            {
                return new BaseResponse(false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return new BaseResponse(false, ex.Message, null);
            }
        }
    }
}