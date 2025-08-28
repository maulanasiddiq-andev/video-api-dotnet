using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VideoApi.Constants;
using VideoApi.Dtos;
using VideoApi.Exceptions;
using VideoApi.Models;
using VideoApi.Repositories;
using VideoApi.Responses;
using VideoApi.Settings;

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
            AuthRepository authRepository,
            IOptions<JWTSettings> options
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
        [Route("otp")]
        public async Task<BaseResponse> CheckOtpValidationAsync([FromBody] CheckOtpDto checkOtpDto)
        {
            try
            {
                await _authRepository.CheckOtpValidationAsync(checkOtpDto);

                return new BaseResponse(true, "Kode OTP berhasil di verifikasi, silakan login", null);
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
        public async Task<BaseResponse> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                var validator = new LoginValidator();
                var results = validator.Validate(loginDto);
                if (!results.IsValid)
                {
                    var messages = results.Errors.Select(x => x.ErrorMessage).ToList();
                    return new BaseResponse(false, messages);
                }

                var user = await _authRepository.FindUserByEmailAsync(loginDto.Email);
                if (user is null)
                {
                    throw new KnownException(ErrorMessageConstant.InvalidLogin);
                }

                var isLoginValid = await _authRepository.IsLoginValidAsync(user, loginDto.Password);
                if (!isLoginValid)
                {
                    throw new KnownException(ErrorMessageConstant.InvalidLogin);
                }

                var userAgent = string.IsNullOrEmpty(Request.Headers["User-Agent"]) ? "" : Request.Headers["User-Agent"].ToString();

                TokenDto tokenDto = await _authRepository.GenerateAndSaveLoginToken(user, userAgent);

                return new BaseResponse(true, "Login Berhasil", tokenDto);
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