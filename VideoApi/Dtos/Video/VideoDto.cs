namespace VideoApi.Dtos.Video
{
    public class VideoDto : BaseDto
    {
        public string VideoId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public UserDto? User { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? Duration { get; set; }
        public string? IsCommentActive { get; set; }
        public string? IsLikeVisible { get; set; }
        public string? IsDislikeVisible { get; set; }
    }
}