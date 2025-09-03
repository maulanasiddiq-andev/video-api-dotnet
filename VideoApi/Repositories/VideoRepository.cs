using AutoMapper;
using VideoApi.Constants;
using VideoApi.Dtos.Video;
using VideoApi.Extensions;
using VideoApi.Models;

namespace VideoApi.Repositories
{
    public class VideoRepository
    {
        private readonly VideoAppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly string? userId = "";
        public VideoRepository(
            VideoAppDBContext dBContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _dBContext = dBContext;
            _mapper = mapper;

            if (httpContextAccessor != null)
            {
                userId = httpContextAccessor.HttpContext?.GetUserId();
            }
        }

        public async Task<VideoModel> CreateVideoAsync(VideoCreateRequestDto video)
        {
            var thumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "thumbnails");

            if (!Directory.Exists(thumbnailPath))
            {
                Directory.CreateDirectory(thumbnailPath);
            }

            var thumbnailName = Guid.NewGuid() + Path.GetExtension(video.Thumbnail!.FileName);
            var thumbnailFilepath = Path.Combine(thumbnailPath, thumbnailName);
            using (var stream = new FileStream(thumbnailFilepath, FileMode.Create))
            {
                await video.Thumbnail.CopyToAsync(stream);
            }

            var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");

            if (!Directory.Exists(videoPath))
            {
                Directory.CreateDirectory(videoPath);
            }

            var videoName = Guid.NewGuid() + Path.GetExtension(video.Video!.FileName);
            var videoFilepath = Path.Combine(videoPath, videoName);
            using (var stream = new FileStream(videoFilepath, FileMode.Create))
            {
                await video.Video.CopyToAsync(stream);
            }

            VideoModel videoModel = _mapper.Map<VideoModel>(video);

            videoModel.VideoId = Guid.NewGuid().ToString("N");
            videoModel.ThumbnailUrl = Path.Combine("thumbnails", thumbnailName).Replace("\\", "/");
            videoModel.VideoUrl = Path.Combine("videos", videoName).Replace("\\", "/");;
            videoModel.CreatedTime = DateTime.UtcNow;
            videoModel.ModifiedTime = DateTime.UtcNow;
            videoModel.UserId = userId;
            videoModel.CreatedBy = userId ?? "";
            videoModel.ModifiedBy = userId ?? "";
            videoModel.RecordStatus = RecordStatusConstant.Active;

            await _dBContext.AddAsync(videoModel);
            await _dBContext.SaveChangesAsync();

            return videoModel;
        }
    }
}