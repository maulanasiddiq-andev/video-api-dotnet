namespace VideoApi.Dtos
{
    public class UserDto : BaseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime EmailVerifiedTime { get; set; }
        public string? ProfileImage { get; set; }
        public string? CoverImage { get; set; }
        public DateTime LastloginTime { get; set; }
        public int FailedLoginAttempts { get; set; }
    }
}