namespace VideoApi.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiredTime { get; set; }
        public bool IsValidlLogin { get; set; }
    }
}