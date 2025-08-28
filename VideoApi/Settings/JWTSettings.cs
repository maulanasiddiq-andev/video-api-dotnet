namespace VideoApi.Settings
{
    public class JWTSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int RefreshTokenExpiredTimeInHour { get; set; }
        public int TokenExpiredTimeInHour { get; set; }
    }
}