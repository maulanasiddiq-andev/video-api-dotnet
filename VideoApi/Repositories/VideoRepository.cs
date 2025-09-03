using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VideoApi.Constants;
using VideoApi.Dtos.Requests;
using VideoApi.Dtos.Video;
using VideoApi.Exceptions;
using VideoApi.Extensions;
using VideoApi.Models;
using VideoApi.Responses;

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

        public async Task<SearchResponse> GetDatasAsync(SearchRequestDto search)
        {
            IQueryable<VideoModel> listVideoQuery = _dBContext.Video
                .Where(x => x.RecordStatus.ToLower().Equals(RecordStatusConstant.Active.ToLower()))
                .Include(x => x.User)
                .AsQueryable();

            #region Ordering
            string orderBy = search.OrderBy;
            string orderDir = search.OrderDir;

            if (orderBy.Equals("createdTime"))
            {
                if (orderDir.Equals("asc"))
                {
                    listVideoQuery = listVideoQuery.OrderBy(x => x.CreatedTime).AsQueryable();
                }
                else if (orderDir.Equals("desc"))
                {
                    listVideoQuery = listVideoQuery.OrderByDescending(x => x.CreatedTime).AsQueryable();
                }
            }
            #endregion

            var response = new SearchResponse();
            response.TotalItems = await listVideoQuery.CountAsync();
            response.CurrentPage = search.CurrentPage;
            response.PageSize = search.PageSize;

            var skip = search.PageSize * search.CurrentPage;
            var take = search.PageSize;
            var listVideo = await listVideoQuery.Skip(skip).Take(take).ToListAsync();
            
            response.Items = _mapper.Map<List<VideoDto>>(listVideo);

            return response;
        }

        public async Task<VideoDto> GetDataById(string id)
        {
            VideoModel? video = await _dBContext.Video
                .Where(x => x.VideoId.Equals(id))
                .Where(x => x.RecordStatus.ToLower().Equals(RecordStatusConstant.Active.ToLower()))
                .FirstOrDefaultAsync();

            if (video == null)
            {
                throw new KnownException("Data tidak ditemukan");
            }

            return _mapper.Map<VideoDto>(video);
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
            videoModel.VideoUrl = Path.Combine("videos", videoName).Replace("\\", "/"); ;
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