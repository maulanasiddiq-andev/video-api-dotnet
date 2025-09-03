using Microsoft.EntityFrameworkCore;

namespace VideoApi.Models
{
    public class VideoAppDBContext : DbContext
    {
        public VideoAppDBContext(DbContextOptions<VideoAppDBContext> options) : base(options) { }

        public DbSet<UserModel> User { get; set; }
        public DbSet<OtpModel> Otp { get; set; }
        public DbSet<UserTokenModel> UserToken { get; set; }
        public DbSet<VideoModel> Video { get; set; }
    }
}