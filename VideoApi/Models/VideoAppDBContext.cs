using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoApi.Models.Identity;

namespace VideoApi.Models
{
    public class VideoAppDBContext : DbContext
    {
        public VideoAppDBContext(DbContextOptions<VideoAppDBContext> options) : base(options) { }

        public DbSet<UserModel> User { get; set; }
    }
}