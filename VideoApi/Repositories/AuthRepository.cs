using Microsoft.EntityFrameworkCore;
using VideoApi.Constants;
using VideoApi.Exceptions;
using VideoApi.Helpers;
using VideoApi.Models;

namespace VideoApi.Repositories
{
    public class AuthRepository
    {
        private readonly VideoAppDBContext _dBContext;
        private readonly PasswordHasherHelper passwordHasherHelper;
        public AuthRepository(VideoAppDBContext dBContext)
        {
            _dBContext = dBContext;
            passwordHasherHelper = new PasswordHasherHelper();
        }

        public async Task RegisterAsync(UserModel user, string password)
        {
            var existingUser = await FindUserByEmailAsync(user.Email);

            if (existingUser != null)
            {
                throw new KnownException($"User dengan email {user.Email} sudah ada");
            }

            user.UserId = Guid.NewGuid().ToString("N");
            user.HashedPassword = passwordHasherHelper.Hash(password);
            user.CreatedTime = DateTime.UtcNow;
            user.ModifiedTime = DateTime.UtcNow;
            user.RecordStatus = RecordStatusConstant.Active;

            await _dBContext.AddAsync(user);
            await _dBContext.SaveChangesAsync();
        }

        public async Task<UserModel?> FindUserByEmailAsync(string email)
        {
            UserModel? user = await _dBContext.User.Where(user => user.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

            return user;
        }
    }
}