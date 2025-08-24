using Microsoft.EntityFrameworkCore;
using VideoApi.Models;

namespace VideoApi.Models
{
    public class VideoAppDBContext : DbContext
    {
        public VideoAppDBContext(DbContextOptions<VideoAppDBContext> options) : base(options) { }

        public DbSet<UserModel> User { get; set; }
        public DbSet<OtpModel> Otp { get; set; }
    }
}