using FluentValidation;

namespace VideoApi.Dtos.Video
{
    public class VideoCreateRequestDto : BaseDto
    {
        public VideoCreateRequestDto()
        {
            IsCommentActive = true;
            IsLikeVisible = true;
            IsDislikeVisible = true;
        }
        public string Title { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public IFormFile? Thumbnail { get; set; }
        public IFormFile? Video { get; set; }
        public string Duration { get; set; } = string.Empty;
        public bool IsCommentActive { get; set; }
        public bool IsLikeVisible { get; set; }
        public bool IsDislikeVisible { get; set; }
    }

    public class VideoCreateValidator : AbstractValidator<VideoCreateRequestDto>
    {
        public VideoCreateValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Judul tidak boleh kosong");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Deskripsi tidak boleh kosong");
            RuleFor(x => x.Thumbnail)
                .NotNull().WithMessage("Thumbnail tidak boleh kosong")
                .Must(y => y?.Length > 0).WithMessage("Thumbnail tidak boleh kosong");
            RuleFor(x => x.Video)
                .NotNull().WithMessage("Video tidak boleh kosong")
                .Must(y => y?.Length > 0).WithMessage("Video tidak boleh kosong");
        }
    }
}