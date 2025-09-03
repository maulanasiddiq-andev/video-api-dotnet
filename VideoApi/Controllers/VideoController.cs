using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoApi.Dtos.Requests;
using VideoApi.Dtos.Video;
using VideoApi.Exceptions;
using VideoApi.Repositories;
using VideoApi.Responses;

namespace VideoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VideoController : ControllerBase
    {
        private readonly VideoRepository _videoRepository;
        public VideoController(VideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        [HttpPost]
        public async Task<BaseResponse> CreateVideoAsync([FromForm] VideoCreateRequestDto video)
        {
            try
            {
                var validator = new VideoCreateValidator();
                var results = validator.Validate(video);
                if (!results.IsValid)
                {
                    var messages = results.Errors.Select(x => x.ErrorMessage).ToList();
                    return new BaseResponse(false, messages);
                }

                var videoModel = await _videoRepository.CreateVideoAsync(video);

                return new BaseResponse(true, "Video berhasil ditambahkan", videoModel);
            }
            catch (KnownException ex)
            {
                return new BaseResponse(false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return new BaseResponse(false, ex.Message, null);
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetVideoAsync([FromQuery] SearchRequestDto search)
        {
            try
            {
                var result = await _videoRepository.GetDatasAsync(search);

                return new BaseResponse(true, "", result);
            }
            catch (Exception ex)
            {
                return new BaseResponse(false, ex.Message, null);
            }
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse> GetVideoByIdAsync([FromRoute] string id)
        {
            try
            {
                var video = await _videoRepository.GetDataById(id);

                return new BaseResponse(true, "", video);
            }
            catch (KnownException ex)
            {
                return new BaseResponse(false, ex.Message, null);
            }
            catch (Exception ex)
            {
                return new BaseResponse(false, ex.Message, null);
            }
        }
    }
}