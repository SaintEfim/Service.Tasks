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

        CreateMap<TaskUpdateDto, TaskModel>()
            .ReverseMap();

        CreateMap<TaskModel, CreateActionResultDto>();
    }
}
