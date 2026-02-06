using AutoMapper;
using Service.Tasks.API.Models.Base;
using Service.Tasks.API.Models.Task;
using Service.Tasks.Domain.Models;

namespace Service.Tasks.API;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TaskModel, TaskDto>()
            .ReverseMap();

        CreateMap<TaskCreateDto, TaskModel>();

        CreateMap<TaskModel, TaskUpdateDto>()
            .ForMember(dest => dest.ChildIds, opt => opt.MapFrom(src => src.Children
                .Select(model => model.Id)
                .ToList()))
            .ReverseMap()
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.ChildIds
                .Select(id => new TaskModel { Id = id })
                .ToList()));

        CreateMap<TaskModel, CreateActionResultDto>();
    }
}
