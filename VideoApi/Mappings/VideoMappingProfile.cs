using AutoMapper;
using VideoApi.Dtos.Video;
using VideoApi.Models;

namespace VideoApi.Mappings
{
    public class VideoMappingProfile : Profile
    {
        public VideoMappingProfile()
        {
            CreateMap<VideoCreateRequestDto, VideoModel>();
        }
    }
}