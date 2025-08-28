using System.ComponentModel.DataAnnotations;

namespace VideoApi.Models
{
    public class UserTokenModel : BaseModel
    {
        public UserTokenModel()
        {
            RefreshTokenExpiredTime = DateTime.UtcNow;    
        }

        [Key]
        public string UserTokenId { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiredTime { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiredTime { get; set; }
        public string? Browser { get; set; }   
        public string? Device { get; set; }   
        public string? OsVersion { get; set; }   
        public string? UserAgent { get; set; }   
        public string? Location { get; set; }   
        public double? LocationLatitude { get; set; }   
        public double? LocationLongitude { get; set; }   
    }
}