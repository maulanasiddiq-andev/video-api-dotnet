using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using VideoApi.Constants;
using VideoApi.Exceptions;
using VideoApi.Helpers;
using VideoApi.Models;
using Microsoft.Extensions.Options;
using VideoApi.Dtos;
using VideoApi.Settings;

namespace VideoApi.Repositories
{
    public class AuthRepository
    {
        private readonly VideoAppDBContext _dBContext;
        private readonly PasswordHasherHelper passwordHasherHelper;
        private readonly EmailSettingsModel emailSettings;
        private readonly JWTSettings jwtSettings;
        public AuthRepository(
            VideoAppDBContext dBContext,
            IOptions<EmailSettingsModel> emailOptions,
            IOptions<JWTSettings> jwtOptions
        ) {
            _dBContext = dBContext;
            emailSettings = emailOptions.Value;
            jwtSettings = jwtOptions.Value;
            passwordHasherHelper = new PasswordHasherHelper();
        }

        public async Task RegisterAsync(UserModel user, string password)
        {
            var userExists = await IsValidToCreateUser(user.Email, user.Username);

            if (!userExists)
            {
                throw new KnownException($"User dengan email {user.Email} atau username {user.Username} sudah ada");
            }

            user.UserId = Guid.NewGuid().ToString("N");
            user.HashedPassword = passwordHasherHelper.Hash(password);
            user.CreatedTime = DateTime.UtcNow;
            user.ModifiedTime = DateTime.UtcNow;
            user.RecordStatus = RecordStatusConstant.Active;

            Random rnd = new Random();
            var otpCode = rnd.Next(1000, 9999);

            var otp = new OtpModel
            {
                OtpId = Guid.NewGuid().ToString("N"),
                Email = user.Email,
                OtpCode = otpCode,
                CreatedTime = DateTime.UtcNow,
                ModifiedTime = DateTime.UtcNow
            };

            await _dBContext.AddAsync(otp);
            await _dBContext.SaveChangesAsync();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MS Developer Video App", "maulanasiddiqdeveloper@gmail.com"));
            message.To.Add(new MailboxAddress(user.Name, user.Email));
            message.Subject = "Kode OTP";

            message.Body = new TextPart("plain")
            {
                Text = $"Kode OTP Anda adalah {otpCode}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect(emailSettings.SmtpServer, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(emailSettings.Username, emailSettings.Password);

                client.Send(message);
                client.Disconnect(true);
            }

            await _dBContext.AddAsync(user);
            await _dBContext.SaveChangesAsync();
        }

        public async Task CheckOtpValidationAsync(CheckOtpDto checkOtpDto)
        {
            var otpExists = await _dBContext.Otp.AnyAsync(x => x.Email == checkOtpDto.Email &&
                                                            x.OtpCode == checkOtpDto.OtpCode &&
                                                            DateTime.UtcNow < x.ExpiredTime);
            if (otpExists == false)
            {
                throw new KnownException("Kode OTP tidak valid");
            }

            var user = await _dBContext.User.Where(x => x.Email == checkOtpDto.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new KnownException("User tidak ditemukan");
            }

            user.EmailVerifiedTime = DateTime.UtcNow;
            _dBContext.Update(user);
            await _dBContext.SaveChangesAsync();
        }

        public async Task<bool> IsValidToCreateUser(string email, string username)
        {
            bool user = await _dBContext.User
                .AnyAsync(user =>
                    (user.Email == email || user.Username.ToLower() == username.ToLower()) &&
                    user.RecordStatus.ToLower() == RecordStatusConstant.Active.ToLower());

            return !user;
        }

        public async Task<UserModel?> FindUserByEmailAsync(string email)
        {
            return await _dBContext.User.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task IncrementFailedLoginAttempts(UserModel user)
        {
            user.FailedLoginAttempts++;

            _dBContext.Update(user);
            await _dBContext.SaveChangesAsync();
        }

        public async Task<bool> IsLoginValidAsync(UserModel user, string password)
        {
            var isPasswordValid = passwordHasherHelper.IsPasswordValid(user.HashedPassword, password);
            if (!isPasswordValid && user.EmailVerifiedTime == null)
            {
                await IncrementFailedLoginAttempts(user);
            }

            return isPasswordValid && user.EmailVerifiedTime != null;
        }

        public async Task<TokenDto> GenerateAndSaveLoginToken(UserModel user, string userAgent)
        {
            DateTime expiredTime = DateTime.UtcNow.AddHours(jwtSettings.TokenExpiredTimeInHour);
            var jwtToken = AuthorizationHelper.GenerateJWTToken(jwtSettings, expiredTime, user);

            var userToken = new UserTokenModel
            {
                UserId = user.UserId,
                ExpiredTime = expiredTime,
                Token = jwtToken,
                Browser = "",
                Device = "",
                OsVersion = "",
                Location = "",
                UserAgent = userAgent,
                RecordStatus = RecordStatusConstant.Active,
                CreatedBy = user.UserId,
                ModifiedBy = user.UserId,
                RefreshToken = AuthorizationHelper.GenerateRandomAlphaNumeric(),
                RefreshTokenExpiredTime = DateTime.UtcNow.AddHours(jwtSettings.RefreshTokenExpiredTimeInHour),
                CreatedTime = DateTime.UtcNow,
                ModifiedTime = DateTime.UtcNow,
                Description = ""
            };

            await _dBContext.AddAsync(userToken);
            await _dBContext.SaveChangesAsync();

            var tokenDto = new TokenDto
            {
                Token = jwtToken,
                IsValidlLogin = true,
                RefreshToken = userToken.RefreshToken,
                RefreshTokenExpiredTime = userToken.RefreshTokenExpiredTime
            };

            return tokenDto;
        }
    }
}