namespace VideoApi.Dtos
{
    public class CheckOtpDto
    {
        public string Email { get; set; } = string.Empty;
        public int OtpCode { get; set; }
    }
}