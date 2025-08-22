using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VideoApi.Dtos;
using VideoApi.Exceptions;
using VideoApi.Helpers;
using VideoApi.Models;
using VideoApi.Repositories;
using VideoApi.Responses;

namespace VideoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AuthRepository _authRepository;
        public AuthController(
            IMapper mapper,
            AuthRepository authRepository
        )
        {
            _mapper = mapper;
            _authRepository = authRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<BaseResponse> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            try
            {
                var validator = new RegisterValidator();
                var results = validator.Validate(registerDto);
                if (!results.IsValid)
                {
                    var messages = results.Errors.Select(error => error.ErrorMessage).ToList();
                    return new BaseResponse(false, messages);
                }

                var user = _mapper.Map<UserModel>(registerDto);

                await _authRepository.RegisterAsync(user, registerDto.Password);

                return new BaseResponse(true, "Pendaftaran Berhasil", null);
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

        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync()
        {
            await Task.Delay(200);

            return "LOGIN";
        }
    }
}