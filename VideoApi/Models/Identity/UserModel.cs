using System.ComponentModel.DataAnnotations;

namespace VideoApi.Models.Identity
{
    public class UserModel : BaseModel
    {
        [Key]
        public String UserId { get; set; } = string.Empty;
        [Required]
        public String Name { get; set; } = string.Empty;
        [Required]
        public String Email { get; set; } = string.Empty;
        [Required]
        public String HashedPassword { get; set; } = string.Empty;
        public DateTime EmailVerifiedTime { get; set; }
        public String? ProfileImage { get; set; }
        public String? CoverImage { get; set; }
        public DateTime LastLoginTime { get; set; }
        public int FailedLoginAttempts { get; set; } = 0;
    }
}