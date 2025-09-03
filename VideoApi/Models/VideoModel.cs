using System.ComponentModel.DataAnnotations;

namespace VideoApi.Models
{
    public class VideoModel : BaseModel
    {
        [Key]
        public string VideoId { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string? UserId { get; set; }
        public UserModel? User { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? Duration { get; set; }
        public bool IsCommentActive { get; set; }
        public bool IsLikeVisible { get; set; }      
        public bool IsDislikeVisible { get; set; }  
    }
}