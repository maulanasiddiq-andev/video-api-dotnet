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
            user.HashedPassword = passwordHasherHelper.Hash(password);

            await _dBContext.AddAsync(user);
            await _dBContext.SaveChangesAsync();
        }
    }
}